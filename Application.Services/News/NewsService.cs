using System;
using System.Collections.Generic;
using System.Linq;
using Application.Dal;
using Application.Dal.Domain;
using Application.Dal.Domain.News;
using Application.Services.Utils;

namespace Application.Services.News
{
 

    public class NewsService : INewsService
    {
        private readonly IRepository<NewsItem> _newsRepository;

        public NewsService(IRepository<NewsItem> newsRepository)
        {
            _newsRepository = newsRepository;
        }

        public List<NewsItem> GetAllNews()
        {
            var testList = new List<NewsItem>()
            {
                new NewsItem()
                {
                    AutorFio = "Петров А.С.",
                    MenuItemName = "Учебная и методическая деятельность",
                    MenuItemAction = "#",
                    UrlImg = "/content/layout_icons/information.svg",
                    LastChangeDate = DateTime.Today,
                    NameAction = "#",
                    Name = "Правила работы с информационно-образовательной средой Петрогневного универа женского счастья и любви для чайников и умных"
                },
                new NewsItem()
                {
                    AutorFio = "Петров А.С.",
                    MenuItemName = "Учебная и методическая деятельность",
                    MenuItemAction = "#",
                    UrlImg = "/content/layout_icons/information.svg",
                    LastChangeDate = DateTime.Today,
                    NameAction = "#",
                    Name = "Правила работы с информационно-образовательной средой Петрогневного универа женского счастья и любви для чайников и умных"
                },
                new NewsItem()
                {
                    AutorFio = "Петров А.С.",
                    MenuItemName = "Учебная и методическая деятельность",
                    MenuItemAction = "#",
                    UrlImg = "/content/layout_icons/information.svg",
                    LastChangeDate = DateTime.Today,
                    NameAction = "#",
                    Name = "Правила работы с информационно-образовательной средой Петрогневного универа женского счастья и любви для чайников и умных"
                },
                new NewsItem()
                {
                    AutorFio = "Петров А.С.",
                    MenuItemName = "Учебная и методическая деятельность",
                    MenuItemAction = "#",
                    UrlImg = "/content/layout_icons/information.svg",
                    LastChangeDate = DateTime.Today,
                    NameAction = "#",
                    Name = "Правила работы с информационно-образовательной средой Петрогневного универа женского счастья и любви для чайников и умных"
                },
                new NewsItem()
                {
                    AutorFio = "Петров А.С.",
                    MenuItemName = "Учебная и методическая деятельность",
                    MenuItemAction = "#",
                    UrlImg = "/content/layout_icons/information.svg",
                    LastChangeDate = DateTime.Today,
                    NameAction = "#",
                    Name = "Правила работы с информационно-образовательной средой Петрогневного универа женского счастья и любви для чайников и умных"
                },
                new NewsItem()
                {
                    AutorFio = "Петров А.С.",
                    MenuItemName = "Учебная и методическая деятельность",
                    MenuItemAction = "#",
                    UrlImg = "/content/layout_icons/information.svg",
                    LastChangeDate = DateTime.Today,
                    NameAction = "#",
                    Name = "Правила работы с информационно-образовательной средой Петрогневного универа женского счастья и любви для чайников и умных"
                },
                new NewsItem()
                {
                    AutorFio = "Петров А.С.",
                    MenuItemName = "Учебная и методическая деятельность",
                    MenuItemAction = "#",
                    UrlImg = "/content/layout_icons/information.svg",
                    LastChangeDate = DateTime.Today,
                    NameAction = "#",
                    Name = "Правила работы с информационно-образовательной средой Петрогневного универа женского счастья и любви для чайников и умных"
                },
                new NewsItem()
                {
                    AutorFio = "Петров А.С.",
                    MenuItemName = "Учебная и методическая деятельность",
                    MenuItemAction = "#",
                    UrlImg = "/content/layout_icons/information.svg",
                    LastChangeDate = DateTime.Today,
                    NameAction = "#",
                    Name = "Правила работы с информационно-образовательной средой Петрогневного универа женского счастья и любви для чайников и умных"
                },
            };

            return testList;
        }

        #region Crud

        public void CreateNews(NewsItem item)
        {
            if (item == null) throw new NullReferenceException(nameof(item));
            _newsRepository.Add(item);
        }

        public void UpdateNews(NewsItem item)
        {
            if (item == null) throw new NullReferenceException(nameof(item));
            _newsRepository.Update(item);

        }


        public void DeleteNews(NewsItem item)
        {
            if (item == null) throw new NullReferenceException(nameof(item));
            _newsRepository.Delete(item);

        }

        public NewsItem GetNewsItem(string itemId)
        {
            if (string.IsNullOrEmpty(itemId)) return null;
            return _newsRepository.Get(itemId);
        }

        /// <summary>
        /// Поиск новостей 
        /// </summary>
        /// <remarks> Поиск по дате осуществляется только в период с<paramref name= "startDate" />
        /// по < paramref name= "endDate" /></remarks> 
        /// <param name="authorId">Автор</param>
        /// <param name="category">Категория</param>
        /// <param name="startDate">С даты</param>
        /// <param name="endDate">По дату</param>
        public IEnumerable<NewsItem> GetNewsItem(string authorId = null, string category = null,
            DateTime? startDate = null, DateTime? endDate = null, bool isNewest = true)
        {
            var collection = _newsRepository.GetAll();
            if (authorId != null) collection = collection.Where(a => a.LastChangeAuthor == authorId);
            if (category != null) collection = collection.Where(c => c.Category == category);

            collection = collection.SortByNewestOrOldest(isNewest, item => item.CreatedDate);

            //if (startDate != null)
            //{
            //    collection = collection.Where(c => c.CreatedDate >= startDate || ((BaseEntity) c).LastChangeDate >= startDate);
            //    if (endDate != null)
            //    {
            //        collection = collection.Where(c => c.CreatedDate <= endDate || ((BaseEntity) c).LastChangeDate <= endDate);
            //    }
            //}

            return collection.ToList();
        }
        #endregion
    }
}
