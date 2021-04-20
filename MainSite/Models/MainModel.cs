using System;
using System.Collections.Generic;
using System.Linq;
using Application.Dal;
using Application.Dal.Domain.Menu;
using Application.Dal.Domain.News;
using Application.Services.Files;
using Application.Services.Infrastructure;
using Application.Services.Menu;
using Application.Services.News;
using Application.Services.Settings;
using Application.Services.Users;
using MainSite.Controllers;
using MainSite.Extensions;
using MainSite.ViewModels.Common;
using MainSite.ViewModels.News;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace MainSite.Models
{
    public class MainModel
    {
        private readonly ILogger<HomeController> _logger;
        private readonly INewsService _newsService;
        private readonly IFileDownloadService _downloadService;
        private readonly IFileUploadService _uploadService;
        private readonly ISettingsService _settingsService;
        private readonly IMenuService _menuService;
        private readonly IUsersService _usersService;
        private readonly PinNewsService _pinNewsService;

        public MainModel(ILogger<HomeController> logger, INewsService newsService, IFileDownloadService downloadService,
            IFileUploadService uploadService, ISettingsService settingsService, IMenuService menuService, IUsersService usersService,
            PinNewsService pinNewsService)
        {
            _logger = logger;
            _newsService = newsService;
            _downloadService = downloadService;
            _uploadService = uploadService;
            _settingsService = settingsService;
            _menuService = menuService;
            _usersService = usersService;
            _pinNewsService = pinNewsService;
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
            var files = _uploadService.GetFilesByNewsId(newsItem.Id).ToList();

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
                IsMessage = !filesResult.Any()
            };
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

        public byte[] GeDownloadedFile(string newsItemId)
        {
            var file = _uploadService.GetFileById(newsItemId);

            if (file == null) return new byte[0];

            return _uploadService.LoadFileBinary(file);
        }

        public void CreateNewNewsItem(NewsItemViewModel newsItemViewModel)
        {
            var dataTimeNow = DateTime.Now;
            var entity = new NewsItem
            {
                Header = newsItemViewModel.Header,
                Description = newsItemViewModel.Description,
                AutorFio = _usersService.GetUserBySystemName(newsItemViewModel.Author)?.FullName?? "Автор не указан",
                CreatedDate = dataTimeNow,
                LastChangeDate = dataTimeNow,
                Category = newsItemViewModel.CategoryId,
            };
            var collection = new List<Application.Dal.Domain.Files.File>();
            //uploadFiles 
            foreach (var file in newsItemViewModel?.UploadedFiles)
            {
                collection.Add(_uploadService.InsertFile(file));
            }

            entity.Files = collection;
            _newsService.CreateNews(entity);
            newsItemViewModel.Id = entity.Id;
        }

        public void DeleteNewsItem(string id)
        {
            var item = _newsService.GetNewsItem(id);
            _newsService.DeleteNews(item);

            foreach (var file in item.Files)
            {
                _downloadService.DeleteDownload(file);
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

            foreach (var newsItem in newsItems)
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
            if(newsItem == null) return;

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
    }
}