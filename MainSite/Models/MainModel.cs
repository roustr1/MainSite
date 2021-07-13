using Application.Dal;
using Application.Dal.Domain.Menu;
using Application.Dal.Domain.News;
using Application.Dal.Infrastructure;
using Application.Services.Files;
using Application.Services.Menu;
using Application.Services.News;
using Application.Services.Settings;
using Application.Services.Users;
using MainSite.Extensions;
using MainSite.ViewModels.Common;
using MainSite.ViewModels.News;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text.RegularExpressions;
using System.Xml;
using Application.Dal.Repositories.Infrastructure;
using Application.Services.Permissions;
using Application.Services.Utils;
using Microsoft.Win32;
using File = Application.Dal.Domain.Files.File;

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
        private readonly bool StoreInDb = false;
        private readonly IPermissionService _permissionService;

        public MainModel(
            INewsService newsService,
            IFileDownloadService downloadService,
            ISettingsService settingsService,
            IMenuService menuService,
            IUsersService usersService,
            PinNewsService pinNewsService,
            IAppFileProvider fileProvider,
            IPermissionService permissionService)
        {
            _newsService = newsService;
            _downloadService = downloadService;
            _settingsService = settingsService;
            _menuService = menuService;
            _usersService = usersService;
            _pinNewsService = pinNewsService;
            _fileProvider = fileProvider;
            _permissionService = permissionService;

        }

        #region News

        public NewsItemViewModel GetNewsItemViewModel(string id)
        {
            return GetNewsItemViewModel(_newsService.GetNewsItem(id));
        }

        private NewsItemViewModel GetNewsItemViewModel(NewsItem newsItem)
        {
            if (newsItem == null)
            {
                return null;
            }

            string categoryName = "";
            if (newsItem.Category != null)
            {
                var cat = _menuService.Get(newsItem.Category);
                categoryName = cat == null ? "" : cat.Name;
            }

            return new NewsItemViewModel
            {
                Id = newsItem.Id,
                Header = string.IsNullOrWhiteSpace(newsItem.Header) ? "" : newsItem.Header,
                Description = string.IsNullOrWhiteSpace(newsItem.Description) ? "" : newsItem.Description,
                CategoryId = newsItem.Category,
                Category = categoryName,
                Author = newsItem.AutorFio,
                CreatedDate = newsItem.CreatedDate,
                LastChangeDate = newsItem.LastChangeDate,
                IsMessage = newsItem.Files != null ? !newsItem.Files.Any() : true,
                Files = newsItem.Files?.Select(s => new FileViewModel
                {
                    Name = s.Filename,
                    MimeType = s.ContentType,
                    Id = s.Id,
                    Extension = s.Extension
                }).ToList(),
                IsAdvancedEditor = newsItem.IsAdvancedEditor
            };
        }

        public void EditNewNewsItem(NewsItemViewModel model, ClaimsPrincipal author)
        {

            var entity = _newsService.GetNewsItem(model.Id);

            entity.Header = model.Header;
            entity.LastChangeDate = DateTime.Now;
            entity.AutorFio = _usersService.GetUserBySystemName(author)?.FullName ?? "Автор не указан";
            entity.Description = model.Description;

            foreach (var file in entity.Files.ToList())
            {
                _fileProvider.DeleteFile(file.DownloadUrl);
                _downloadService.DeleteDownload(file);

            }

            _newsService.UpdateNews(entity);
            //uploadFiles 
            UploadFiles(model.UploadedFiles, entity.Id);

        }

        public void CreateNewNewsItem(NewsItemViewModel newsItemViewModel, ClaimsPrincipal author)
        {
            var entity = new NewsItem
            {
                Id = Guid.NewGuid().ToString(),
                Header = newsItemViewModel.Header,
                Description = newsItemViewModel.Description,
                AutorFio = _usersService.GetUserBySystemName(author)?.FullName ?? "Автор не указан",
                CreatedDate = DateTime.Now,
                Category = newsItemViewModel.CategoryId,
                IsAdvancedEditor = newsItemViewModel.IsAdvancedEditor
            };
            _newsService.CreateNews(entity);
            newsItemViewModel.Id = entity.Id;

            //uploadFiles 
            UploadFiles(newsItemViewModel.UploadedFiles, entity.Id);
        }

        private void UploadFiles(ICollection<IFormFile> httpPostedFile, string newsItemId)
        {
            foreach (var file in httpPostedFile)
            {
                var fileBinary = _downloadService.GetDownloadBits(file);
                var fileName = file.FileName;
                //remove path (passed in IE)
                fileName = _fileProvider.GetFileNameWithoutExtension(fileName).Replace('.', '_');
                var contentType = file.ContentType;
                var fileExtension = _fileProvider.GetFileExtension(file.FileName);
                if (!string.IsNullOrEmpty(fileExtension))
                    fileExtension = fileExtension.ToLowerInvariant();

                var download = new File
                {
                    ContentType = contentType,
                    Filename = _fileProvider.GetFileNameWithoutExtension(fileName),
                    Extension = fileExtension,
                    NewsItemId = newsItemId,
                    Name = file.Name
                };
                if (!StoreInDb) //if file saving into filesystem
                {
                    var pathToFile = _downloadService.SaveFileInFileSystem(fileBinary, fileBinary.GetHashCode().ToString());

                    download.DownloadUrl = _fileProvider.GetFileName(pathToFile);
                }
                else
                {
                    download.DownloadUrl = string.Empty;
                    download.DownloadBinary = fileBinary;
                }
                _downloadService.InsertDownload(download);
            }
        }


        public void InsertAdvancedNewsItem(NewsItemViewModel model)
        {
            ReplaceImg(model);
        }

        private void ReplaceImg(NewsItemViewModel item)
        {
            var imgRegex = new Regex("<img [^>]+>", RegexOptions.IgnoreCase | RegexOptions.Compiled);
            var base64Regex = new Regex("data:[^/]+/(?<ext>[a-z]+);base64,(?<base64>.+)", RegexOptions.IgnoreCase);

            foreach (Match? match in imgRegex.Matches(item.Description))
            {

                var doc = new XmlDocument();

                var matchValue = match.Value.EndsWith("/>") ? match.Value : match.Value.Replace(">", "/>");

                doc.LoadXml($"<root>{matchValue}</root>");

                var img = doc.FirstChild.FirstChild;
                var srcNode = img.Attributes["src"];
                string mime = MimeTypes.ImageJpeg;
                try
                {
                    mime = srcNode.Value.Split(";").First().Substring(5);
                }
                catch { }

                var fileExt = GetDefaultExtension(mime);
                var base64Match = base64Regex.Match(srcNode.Value);
                if (base64Match.Success)
                {
                    var bytes = Convert.FromBase64String(base64Match.Groups["base64"].Value);
                    var file = _downloadService.SaveFileInFileSystem(bytes, img.Attributes["id"].Value + fileExt, AppMediaDefaults.PathToNewsMedia);

                    srcNode.Value = _fileProvider.GetVirtualPath(file);

                    item.Description = item.Description.Replace(match.Value, img.OuterXml, StringComparison.OrdinalIgnoreCase);
                }
            }
        }


        private string GetDefaultExtension(string mimeType)
        {
            var key = Registry.ClassesRoot.OpenSubKey(@"MIME\Database\Content Type\" + mimeType, false);
            var value = key?.GetValue("Extension", null);
            var result = value != null ? value.ToString() : string.Empty;

            if (result == ".jfif" || string.IsNullOrEmpty(result)) result = ".jpg";//костылёчек-костылек

            return result;
        }
        public FileContentResult GetDownloadFile(string fileId)
        {
            var download = _downloadService.GetDownloadByIdOrName(fileId);
            var fileName = !string.IsNullOrWhiteSpace(download.Filename) ? download.Filename : download.Id;
            var contentType = !string.IsNullOrWhiteSpace(download.ContentType)
                ? download.ContentType
                : MimeTypes.ApplicationOctetStream;

            var filePath = _downloadService.GetFileLocalPath(download.DownloadUrl);
            var fileBytes = !StoreInDb ? _fileProvider.ReadAllBytes(filePath) : download.DownloadBinary;

            return new FileContentResult(fileBytes, contentType)
            {
                FileDownloadName = fileName + download.Extension
            };
        }

        private IList<NewsItemViewModel> GetNewsItemsViewModel(IEnumerable<NewsItem> newsItems)
        {
            var result = new List<NewsItemViewModel>();

            foreach (var newsItem in newsItems.ToList())
            {

                var newsItemViewModel = GetNewsItemViewModel(newsItem);

                if (newsItemViewModel == null) continue;

                result.Add(newsItemViewModel);
            }

            return result;
        }

        private IList<NewsItemViewModel> GetManyNewsItemViewModel(string categoryId, int skip, int take)
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
                PinnedNewsIds = pinnedNewsIds,
                Skip = skip,
                Take = take
            };
            var newsCollection = _newsService.GetNewsItem(filterNewsItemParameters);
            return GetNewsItemsViewModel(newsCollection);
        }

        private IList<PinnedNewsViewModel> GetAllPinnedNewsByCategory(string categoryId)
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
            var newsCount = _newsService.GetTotalNewsCount(category);
            var records = GetManyNewsItemViewModel(category, pageIndex * pageSize, pagesize.GetValueOrDefault(5));

            var list = new PagedList<NewsItemViewModel>(records, pageIndex, pageSize, newsCount);
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

        public void DeleteNewsItem(string id)
        {
            var item = _newsService.GetNewsItem(id);
            if (item.Files != null)
            {
                foreach (var file in item.Files.ToList())
                {
                    _downloadService.DeleteDownload(file);
                }
            }
            _newsService.DeleteNews(item);
        }

        #region Pined news

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

        #endregion

        #endregion

        #region Search
        public IList<NewsItemViewModel> GetManySearchResultNewsItemViewModel(string query)
        {
            return GetNewsItemsViewModel(_newsService.FindFreeText(query));
        }


        #endregion

        #region Permissions

        public bool GetUserPermissionForCategory(string categoryId, ClaimsPrincipal User)
        {
            var category = _menuService.Get(categoryId);
            var permission = _permissionService.GetPermissionRecordBySystemName(
                new TranslitMethods.Translitter().Translit(category.Name, TranslitMethods.TranslitType.Gost));
            return _permissionService.Authorize(permission, User);
        }

        #endregion


        public int GetSettingNewsPerPage()
        {
            int pagesize = 3;
            if (_settingsService.SettingsDictionary.TryGetValue("Page.PageSize", out var _value))
            {
                int.TryParse(_value, out pagesize);
            }

            return pagesize;
        }

    }
}