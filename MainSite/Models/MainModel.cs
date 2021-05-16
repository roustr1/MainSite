using System;
using System.Collections.Generic;
using System.Linq;
using Application.Dal;
using Application.Dal.Domain.Files;
using Application.Dal.Domain.Menu;
using Application.Dal.Domain.News;
using Application.Dal.Infrastructure;
using Application.Services.Files;
using Application.Services.Infrastructure;
using Application.Services.Menu;
using Application.Services.News;
using Application.Services.Settings;
using Application.Services.Users;
using MainSite.Extensions;
using MainSite.ViewModels.Common;
using MainSite.ViewModels.News;
using Microsoft.AspNetCore.Http;

namespace MainSite.Models
{
    public class MainModel
    {
        private readonly INewsService _newsService;
        private readonly IFileDownloadService _downloadService;
        private readonly ISettingsService _settingsService;
        private readonly IMenuService _menuService;
        private readonly IUsersService _usersService;
        private readonly PinNewsService _pinNewsService;
        private readonly IAppFileProvider _fileProvider;

        public MainModel(INewsService newsService, IFileDownloadService downloadService, ISettingsService settingsService, IMenuService menuService, IUsersService usersService, PinNewsService pinNewsService, IAppFileProvider fileProvider)
        {
            _newsService = newsService;
            _downloadService = downloadService;
            _settingsService = settingsService;
            _menuService = menuService;
            _usersService = usersService;
            _pinNewsService = pinNewsService;
            _fileProvider = fileProvider;
        }

        public NewsItemViewModel GetNewsItemViewModel(string id)
        {
            return GetNewsItemViewModel(_newsService.GetNewsItem(id));
        }

        public NewsItemViewModel GetNewsItemViewModel(NewsItem newsItem)
        {
            if (newsItem == null)
            {
                return null;
            }

            var filesResult = new List<FileViewModel>();
            var files = _downloadService.GetFilesByNewsId(newsItem.Id).ToList();

            foreach (var newsItemFile in files)
            {
                filesResult.Add(GetDownloadedFileViewModel(newsItemFile));
            }

            string categoryName = "";
            if (newsItem.Category != null)
            {
                var cat = _menuService.Get(newsItem.Category);
                categoryName = cat == null ? "" : cat.Name;
            }

            return new NewsItemViewModel()
            {
                Id = newsItem.Id,
                Header = string.IsNullOrWhiteSpace(newsItem.Header) ? "" : newsItem.Header,
                Description = string.IsNullOrWhiteSpace(newsItem.Description) ? "" : newsItem.Description,
                CategoryId = newsItem.Category,
                Category = categoryName,
                Author = newsItem.AutorFio,
                CreatedDate = newsItem.CreatedDate,
                LastChangeDate = newsItem.LastChangeDate,
                Files = filesResult,
                IsMessage = !filesResult.Any(),
                IsAdvancedEditor = newsItem.IsAdvancedEditor
            };
        }

        public void EditNewNewsItem(NewsItemViewModel model)
        {
            var dataTimeNow = DateTime.Now;
            var entity = _newsService.GetNewsItem(model.Id);

            entity.Header = model.Header;
            entity.LastChangeDate = dataTimeNow;
            entity.AutorFio = _usersService.GetUserBySystemName(model.Author)?.FullName ?? "Автор не указан";
            entity.Description = model.Description;

            var collection = new List<File>();

            if (model.Files != null) {
                foreach (var file in model.Files)
                {
                    var entityFile = _downloadService.GetDownloadById(file.Id);
                    if (model.IsAdvancedEditor && entity.Description.Contains(file.Id))
                    {
                        collection.Add(entityFile);
                    }
                    else
                    {
                        if (model.IsAdvancedEditor)
                        {
                            _downloadService.DeleteDownload(entityFile);
                        }
                        else if (model.UploadedFiles.Count() == 0)
                        {
                            collection.Add(entityFile);
                        }
                        else
                        {
                            _downloadService.DeleteDownload(entityFile);
                        }
                    }
                }
            }

            foreach (var file in model.UploadedFiles)
            {

                var fileName = file.FileName;
                //remove path (passed in IE)
                fileName = _fileProvider.GetFileName(fileName);

                var download = new File
                {
                    Id = Guid.NewGuid().ToString(),
                    FileBinary = _downloadService?.GetDownloadBits(file) ?? null,
                    MimeType = file.ContentType,
                    //we store filename without extension for downloads
                    OriginalName = _fileProvider.GetFileNameWithoutExtension(fileName),
                    LastPart = _fileProvider.GetFileExtension(fileName)
                };


                if (model.IsAdvancedEditor)
                {
                    var newDescription = entity.Description.Replace(file.Name, download.Id);
                    entity.Description = newDescription;
                }
                _downloadService?.InsertDownload(download);
                collection.Add(download);
            }

            entity.Files = collection;
            _newsService.UpdateNews(entity);
        }

        public IList<NewsItemViewModel> GetManyNewsItemViewModel(string categoryId)
        {
            var categoryIds = new List<string>();
            var pinnedNewsIds = new List<string>();


            var childrenCategory = _menuService.GetRecursionAllChildren(categoryId);
            var pinnedNews = _pinNewsService.GetAllPinnedNewsByCategory(categoryId);

            categoryIds.Add(categoryId);
            categoryIds.AddRange(childrenCategory.Select(menuItem => menuItem.Id));
            pinnedNewsIds.AddRange(pinnedNews.Select(p => p.NewsItemId));

            var filterNewsItemParameters = new FilterNewsItemParameters()
            {
                CategoryIds = categoryIds,
                PinnedNewsIds = pinnedNewsIds
            };

            return GetNewsItemsViewModel(_newsService.GetNewsItem(filterNewsItemParameters));
        }

        public IList<PinnedNewsViewModel> GetAllPinnedNewsByCategory(string categoryId)
        {
            var result = new List<PinnedNewsViewModel>();

            foreach (var pn in _pinNewsService.GetAllPinnedNewsByCategory(categoryId))
            {
                var pnvm = new PinnedNewsViewModel();
                pnvm.Index = pn.Index;

                var newsItem = _newsService.GetNewsItem(pn.NewsItemId);
                if (newsItem == null)
                {
                    _pinNewsService.UnpinNews(pn.NewsItemId);
                }
                else
                {
                    pnvm.NewsItem = GetNewsItemViewModel(newsItem);

                    result.Add(pnvm);
                }
            }

            return result;
        }

        public NewsListViewModel GetNewsListViewModel(int? page, int? pagesize, string category = null)
        {
            int pageIndex = 0;
            if (page > 0)
            {
                pageIndex = page.Value - 1;
            }
            var pageSize = pagesize.GetValueOrDefault(10);

            var pinnedNews = GetAllPinnedNewsByCategory(category);
            var records = GetManyNewsItemViewModel(category).ToList();

            var list = new PagedList<NewsItemViewModel>(records, pageIndex, pageSize);
            var model = new NewsListViewModel
            {
                CategoryId = category,
                News = list,
                PinnedNews = pinnedNews,
                PagerModel = new PagerViewModel
                {
                    PageSize = list.PageSize,
                    TotalRecords = list.TotalCount,
                    PageIndex = list.PageIndex,
                    RouteValues = new RouteValues { page = pageIndex, category = category },
                }
            };

            PageExtensions.Pager(model.PagerModel);
            return model;
        }

        public FileViewModel GetDownloadedFileViewModel(string id)
        {
            return GetDownloadedFileViewModel(_downloadService.GetDownloadById(id));
        }

        public FileViewModel GetDownloadedFileViewModel(Application.Dal.Domain.Files.File file)
        {
            if (file == null)
            {
                return null;
            }

            return new FileViewModel()
            {
                Id = file.Id,
                Name = file.OriginalName + file.LastPart,
                MimeType = file.MimeType
            };
        }

        public File GeDownloadedFile(string newsItemId)
        {
            return _downloadService.GetDownloadById(newsItemId);
        }

        public void CreateNewNewsItem(NewsItemViewModel newsItemViewModel)
        {
            var dataTimeNow = DateTime.Now;
            var entity = new NewsItem
            {
                Header = newsItemViewModel.Header,
                Description = newsItemViewModel.Description,
                AutorFio = _usersService.GetUserBySystemName(newsItemViewModel.Author)?.FullName ?? "Автор не указан",
                CreatedDate = dataTimeNow,
                LastChangeDate = dataTimeNow,
                Category = newsItemViewModel.CategoryId,
                IsAdvancedEditor = newsItemViewModel.IsAdvancedEditor
            };

            //uploadFiles 
            var collection = new List<File>();
            foreach (var file in newsItemViewModel.UploadedFiles)
            {

                var fileName = file.FileName;
                //remove path (passed in IE)
                fileName = _fileProvider.GetFileName(fileName);

                var download = new File
                {
                    Id = Guid.NewGuid().ToString(),
                    FileBinary = _downloadService?.GetDownloadBits(file) ?? null,
                    MimeType = file.ContentType,
                    //we store filename without extension for downloads
                    OriginalName = _fileProvider.GetFileNameWithoutExtension(fileName),
                    LastPart = _fileProvider.GetFileExtension(fileName)
                };
#warning  Некорректно сохраняется ссылка на файл
                //todo Исправить ссылку
                if (newsItemViewModel.IsAdvancedEditor)
                {
                    var newDescription = entity.Description.Replace(file.Name, download.Id);
                    entity.Description = newDescription;
                }
                _downloadService?.InsertDownload(download);
                collection.Add(download);
            }

            entity.Files = collection;
            _newsService.CreateNews(entity);
            newsItemViewModel.Id = entity.Id;
        }

        public void DeleteNewsItem(string id)
        {
            var item = _newsService.GetNewsItem(id);
            _newsService.DeleteNews(item);
            if (item.Files != null)
            {
                foreach (var file in item.Files)
                {
                    _downloadService.DeleteDownload(file);
                }
            }
        }

        public int GetSettingNewsPerPage()
        {
            int pagesize = 3;
            if (_settingsService.SettingsDictionary.TryGetValue("Page.PageSize", out var _value))
            {
                int.TryParse(_value, out pagesize);
            }

            return pagesize;
        }

        public IList<NewsItemViewModel> GetManySearchResultNewsItemViewModel(string query)
        {
            return GetNewsItemsViewModel(_newsService.FindFreeText(query));
        }

        private IList<NewsItemViewModel> GetNewsItemsViewModel(IEnumerable<NewsItem> newsItems)
        {
            var result = new List<NewsItemViewModel>();

            foreach (var newsItem in newsItems.ToList())
            {
                if (newsItem == null) continue;

                var newsItemViewModel = GetNewsItemViewModel(newsItem);

                if (newsItemViewModel == null) continue;

                result.Add(newsItemViewModel);
            }

            return result;
        }

        public void PinNewsItem(string newsItemId)
        {
            var newsItem = _newsService.GetNewsItem(newsItemId);
            if (newsItem == null) return;

            var categoryId = newsItem.Category;

            _pinNewsService.PinNews(newsItemId, categoryId);
        }

        public void UnpinNewsItem(string newsItemId)
        {
            var newsItem = _newsService.GetNewsItem(newsItemId);
            if (newsItem == null) return;

            var categoryId = newsItem.Category;

            _pinNewsService.UnpinNews(newsItemId);
        }

        public File SaveFile(IFormFile file)
        {

            var fileBinary = _downloadService.GetDownloadBits(file);
            var fileName = file.FileName;
            //remove path (passed in IE)
            fileName = _fileProvider.GetFileName(fileName);
            var contentType = file.ContentType;

            var download = new File
            {
                Id = Guid.NewGuid().ToString(),
                FileBinary = fileBinary,
                MimeType = contentType,
                //we store filename without extension for downloads
                OriginalName = _fileProvider.GetFileNameWithoutExtension(fileName),
                LastPart = _fileProvider.GetFileExtension(fileName)
            };
            _downloadService.InsertDownload(download);
            return download;
        }

    }
}