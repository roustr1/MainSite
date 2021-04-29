using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Application.Dal;
using Application.Dal.Domain.News;
using Application.Services.Infrastructure;
using Application.Services.News;
using Moq;
using NUnit.Framework;

namespace UnitTests
{
    [TestFixture]
    public class NewsServiceTests
    {
        [TestCase("")]
        [TestCase("00000000-0000-0000-0000-000000000000")]
        public void GetNewsItem_RequestWithInvalidId_ReturnNull(string id)
        {
            Mock<IRepository<NewsItem>> mock = new Mock<IRepository<NewsItem>>();

            NewsService newsService = new NewsService(mock.Object);
            NewsItem result = newsService.GetNewsItem(id);

            Assert.IsTrue(result == null);
        }

        [Test]
        public void GetNewsItem_RequestOneExistNewsItem_ReturnNewsItem()
        {
            string guid = Guid.NewGuid().ToString();

            Mock<IRepository<NewsItem>> mock = new Mock<IRepository<NewsItem>>();
            mock.Setup(m => m.Get(guid)).Returns(new NewsItem()
            {
                Id = guid,
                Category = null,
                Description = "Description1",
                Files = null,
                Header = "Header1",
            });

            NewsService newsService = new NewsService(mock.Object);
            NewsItem result = newsService.GetNewsItem(guid);

            Assert.IsTrue(result != null);
            Assert.IsTrue(result.Id == guid);
        }

        [Test]
        public void GetNewsItem_RequestOneNotExistNewsItem_ReturnNull()
        {
            string guid = Guid.NewGuid().ToString();

            Mock<IRepository<NewsItem>> mock = new Mock<IRepository<NewsItem>>();
            mock.Setup(m => m.Get(guid)).Returns(new NewsItem()
            {
                Id = Guid.NewGuid().ToString(),
                Category = null,
                Description = "Description1",
                Files = null,
                Header = "Header1",
            });

            NewsService newsService = new NewsService(mock.Object);
            NewsItem result = newsService.GetNewsItem(Guid.NewGuid().ToString());

            Assert.IsTrue(result == null);
        }

        [Test]
        public void GetNewsItem_RequestAllItem_ReturnAllItems()
        {
            var listItems = new List<NewsItem>()
            {
                new NewsItem()
                {
                    Id = Guid.NewGuid().ToString(),
                    Category = null,
                    Description = "Description1",
                    Files = null,
                    Header = "Header1",
                },
                new NewsItem()
                {
                    Id = Guid.NewGuid().ToString(),
                    Category = null,
                    Description = "Description2",
                    Files = null,
                    Header = "Header2",
                },
                new NewsItem()
                {
                    Id = Guid.NewGuid().ToString(),
                    Category = null,
                    Description = "Description2",
                    Files = null,
                    Header = "Header2",
                }
            };
            Mock<IRepository<NewsItem>> mock = new Mock<IRepository<NewsItem>>();
            mock.Setup(m => m.GetAll).Returns(listItems);

            NewsService newsService = new NewsService(mock.Object);
            List<NewsItem> result = newsService.GetNewsItem(new FilterNewsItemParameters()).ToList();

            Assert.IsTrue(result != null);
            Assert.IsTrue(result.Count == listItems.Count);
        }

        [Test]
        public void GetNewsItem_RequestManyItem_ReturnAllItemsAndOldestFirst()
        {
            DateTime data2019 = new DateTime(2019, 01, 01, 12, 00, 00);
            DateTime data2020 = new DateTime(2020, 01, 01, 12, 00, 00);
            DateTime data2021 = new DateTime(2021, 01, 01, 12, 00, 00);

            NewsItem newsItem1 = new NewsItem()
            {
                Id = Guid.NewGuid().ToString(),
                Category = null,
                Description = "Description1",
                Files = null,
                Header = "Header1",
                CreatedDate = data2021,
                LastChangeDate = data2021
            };
            NewsItem newsItem2 = new NewsItem()
            {
                Id = Guid.NewGuid().ToString(),
                Category = null,
                Description = "Description2",
                Files = null,
                Header = "Header2",
                CreatedDate = data2019,
                LastChangeDate = data2019
            };
            NewsItem newsItem3 = new NewsItem()
            {
                Id = Guid.NewGuid().ToString(),
                Category = null,
                Description = "Description2",
                Files = null,
                Header = "Header2",
                CreatedDate = data2020,
                LastChangeDate = data2020
            };

            Mock<IRepository<NewsItem>> mock = new Mock<IRepository<NewsItem>>();
            mock.Setup(m => m.GetAll).Returns(new List<NewsItem>()
            {
                newsItem1, newsItem2, newsItem3
            });

            NewsService newsService = new NewsService(mock.Object);
            List<NewsItem> result = newsService.GetNewsItem(new FilterNewsItemParameters(){IsNewest = false}).ToList();

            Assert.IsTrue(result != null);
            Assert.IsTrue(result[0].CreatedDate == data2019);
            Assert.IsTrue(result[1].CreatedDate == data2020);
            Assert.IsTrue(result[2].CreatedDate == data2021);
        }

        [Test]
        public void GetNewsItem_RequestManyItem_ReturnAllItemsAndNewestFirst()
        {
            DateTime data2019 = new DateTime(2019, 01, 01, 12, 00, 00);
            DateTime data2020 = new DateTime(2020, 01, 01, 12, 00, 00);
            DateTime data2021 = new DateTime(2021, 01, 01, 12, 00, 00);

            NewsItem newsItem1 = new NewsItem()
            {
                Id = Guid.NewGuid().ToString(),
                Category = null,
                Description = "Description1",
                Files = null,
                Header = "Header1",
                CreatedDate = data2021,
                LastChangeDate = data2021
            };
            NewsItem newsItem2 = new NewsItem()
            {
                Id = Guid.NewGuid().ToString(),
                Category = null,
                Description = "Description2",
                Files = null,
                Header = "Header2",
                CreatedDate = data2019,
                LastChangeDate = data2019
            };
            NewsItem newsItem3 = new NewsItem()
            {
                Id = Guid.NewGuid().ToString(),
                Category = null,
                Description = "Description2",
                Files = null,
                Header = "Header2",
                CreatedDate = data2020,
                LastChangeDate = data2020
            };

            Mock<IRepository<NewsItem>> mock = new Mock<IRepository<NewsItem>>();
            mock.Setup(m => m.GetAll).Returns(new List<NewsItem>()
            {
                newsItem1, newsItem2, newsItem3
            });

            NewsService newsService = new NewsService(mock.Object);
            List<NewsItem> result = newsService.GetNewsItem(new FilterNewsItemParameters()).ToList();

            Assert.IsTrue(result != null);
            Assert.IsTrue(result[0].CreatedDate == data2021);
            Assert.IsTrue(result[1].CreatedDate == data2020);
            Assert.IsTrue(result[2].CreatedDate == data2019);
        }

        [Test]
        public void GetNewsItem_ManyItemWithLastChangeDate_ReturnAllItemsAndOldestFirst()
        {
            DateTime createData2019 = new DateTime(2019, 01, 01, 12, 00, 00);
            DateTime createData2020 = new DateTime(2020, 01, 01, 12, 00, 00);
            DateTime createData2021 = new DateTime(2021, 01, 01, 12, 00, 00);

            DateTime lastChangeData20211101 = new DateTime(2021, 11, 01, 12, 00, 00);
            DateTime lastChangeData20211102 = new DateTime(2021, 11, 02, 12, 00, 00);
            DateTime lastChangeData20211103 = new DateTime(2021, 11, 03, 12, 00, 00);

            NewsItem newsItem1 = new NewsItem()
            {
                Id = Guid.NewGuid().ToString(),
                Category = null,
                Description = "Description1",
                Files = null,
                Header = "Header1",
                CreatedDate = createData2021,
                LastChangeDate = lastChangeData20211101,
            };
            NewsItem newsItem2 = new NewsItem()
            {
                Id = Guid.NewGuid().ToString(),
                Category = null,
                Description = "Description2",
                Files = null,
                Header = "Header2",
                CreatedDate = createData2019,
                LastChangeDate = lastChangeData20211102,
            };
            NewsItem newsItem3 = new NewsItem()
            {
                Id = Guid.NewGuid().ToString(),
                Category = null,
                Description = "Description2",
                Files = null,
                Header = "Header2",
                CreatedDate = createData2020,
                LastChangeDate = lastChangeData20211103,
            };

            Mock<IRepository<NewsItem>> mock = new Mock<IRepository<NewsItem>>();
            mock.Setup(m => m.GetAll).Returns(new List<NewsItem>()
            {
                newsItem1, newsItem2, newsItem3
            });

            NewsService newsService = new NewsService(mock.Object);
            List<NewsItem> result = newsService.GetNewsItem(new FilterNewsItemParameters(){IsNewest = false}).ToList();

            Assert.IsTrue(result != null);
            Assert.IsTrue(result[0].LastChangeDate == lastChangeData20211101);
            Assert.IsTrue(result[1].LastChangeDate == lastChangeData20211102);
            Assert.IsTrue(result[2].LastChangeDate == lastChangeData20211103);
        }

        [Test]
        public void GetNewsItem_ManyItemWithLastChangeDate_ReturnAllItemsAndNewestFirst()
        {
            DateTime createData2019 = new DateTime(2019, 01, 01, 12, 00, 00);
            DateTime createData2020 = new DateTime(2020, 01, 01, 12, 00, 00);
            DateTime createData2021 = new DateTime(2021, 01, 01, 12, 00, 00);

            DateTime lastChangeData20211101 = new DateTime(2021, 11, 01, 12, 00, 00);
            DateTime lastChangeData20211102 = new DateTime(2021, 11, 02, 12, 00, 00);
            DateTime lastChangeData20211103 = new DateTime(2021, 11, 03, 12, 00, 00);

            NewsItem newsItem1 = new NewsItem()
            {
                Id = Guid.NewGuid().ToString(),
                Category = null,
                Description = "Description1",
                Files = null,
                Header = "Header1",
                CreatedDate = createData2021,
                LastChangeDate = lastChangeData20211101,
            };
            NewsItem newsItem2 = new NewsItem()
            {
                Id = Guid.NewGuid().ToString(),
                Category = null,
                Description = "Description2",
                Files = null,
                Header = "Header2",
                CreatedDate = createData2019,
                LastChangeDate = lastChangeData20211102,
            };
            NewsItem newsItem3 = new NewsItem()
            {
                Id = Guid.NewGuid().ToString(),
                Category = null,
                Description = "Description2",
                Files = null,
                Header = "Header2",
                CreatedDate = createData2020,
                LastChangeDate = lastChangeData20211103,
            };

            Mock<IRepository<NewsItem>> mock = new Mock<IRepository<NewsItem>>();
            mock.Setup(m => m.GetAll).Returns(new List<NewsItem>()
            {
                newsItem1, newsItem2, newsItem3
            });

            NewsService newsService = new NewsService(mock.Object);
            List<NewsItem> result = newsService.GetNewsItem(new FilterNewsItemParameters(){IsNewest = true}).ToList();

            Assert.IsTrue(result != null);
            Assert.IsTrue(result[0].LastChangeDate == lastChangeData20211103);
            Assert.IsTrue(result[1].LastChangeDate == lastChangeData20211102);
            Assert.IsTrue(result[2].LastChangeDate == lastChangeData20211101);
        }

        [Test]
        public void GetNewsItem_ManyItemPartWithLastChangeDate_ReturnAllItemsAndOldestFirst()
        {
            DateTime createData2019 = new DateTime(2019, 01, 01, 12, 00, 00);
            DateTime createData2020 = new DateTime(2020, 01, 01, 12, 00, 00);
            DateTime createData2021 = new DateTime(2021, 01, 01, 12, 00, 00);

            DateTime lastChangeData20211102 = new DateTime(2021, 11, 02, 12, 00, 00);
            DateTime lastChangeData20211103 = new DateTime(2021, 11, 03, 12, 00, 00);

            NewsItem newsItem1 = new NewsItem()
            {
                Id = Guid.NewGuid().ToString(),
                Category = null,
                Description = "Description1",
                Files = null,
                Header = "Header1",
                CreatedDate = createData2020,
                LastChangeDate = createData2020,
            };
            NewsItem newsItem2 = new NewsItem()
            {
                Id = Guid.NewGuid().ToString(),
                Category = null,
                Description = "Description2",
                Files = null,
                Header = "Header2",
                CreatedDate = createData2019,
                LastChangeDate = lastChangeData20211102,
            };
            NewsItem newsItem3 = new NewsItem()
            {
                Id = Guid.NewGuid().ToString(),
                Category = null,
                Description = "Description2",
                Files = null,
                Header = "Header2",
                CreatedDate = createData2020,
                LastChangeDate = lastChangeData20211103,
            };

            Mock<IRepository<NewsItem>> mock = new Mock<IRepository<NewsItem>>();
            mock.Setup(m => m.GetAll).Returns(new List<NewsItem>()
            {
                newsItem2, newsItem1, newsItem3
            });

            NewsService newsService = new NewsService(mock.Object);
            List<NewsItem> result = newsService.GetNewsItem(new FilterNewsItemParameters(){IsNewest = false}).ToList();

            Assert.IsTrue(result != null);
            Assert.IsTrue(result[0].LastChangeDate == createData2020);
            Assert.IsTrue(result[1].LastChangeDate == lastChangeData20211102);
            Assert.IsTrue(result[2].LastChangeDate == lastChangeData20211103);
        }

        [Test]
        public void GetNewsItem_ManyItemPartWithLastChangeDate_ReturnAllItemsAndNewestFirst()
        {
            DateTime createData2019 = new DateTime(2019, 01, 01, 12, 00, 00);
            DateTime createData2020 = new DateTime(2020, 01, 01, 12, 00, 00);
            DateTime createData2021 = new DateTime(2021, 01, 01, 12, 00, 00);

            DateTime lastChangeData20211102 = new DateTime(2021, 11, 02, 12, 00, 00);
            DateTime lastChangeData20211103 = new DateTime(2021, 11, 03, 12, 00, 00);

            NewsItem newsItem1 = new NewsItem()
            {
                Id = Guid.NewGuid().ToString(),
                Category = null,
                Description = "Description1",
                Files = null,
                Header = "Header1",
                CreatedDate = createData2021,
                LastChangeDate = createData2021,
            };
            NewsItem newsItem2 = new NewsItem()
            {
                Id = Guid.NewGuid().ToString(),
                Category = null,
                Description = "Description2",
                Files = null,
                Header = "Header2",
                CreatedDate = createData2019,
                LastChangeDate = lastChangeData20211102,
            };
            NewsItem newsItem3 = new NewsItem()
            {
                Id = Guid.NewGuid().ToString(),
                Category = null,
                Description = "Description2",
                Files = null,
                Header = "Header2",
                CreatedDate = createData2020,
                LastChangeDate = lastChangeData20211103,
            };


            Mock<IRepository<NewsItem>> mock = new Mock<IRepository<NewsItem>>();
            mock.Setup(m => m.GetAll).Returns(new List<NewsItem>()
            {
                newsItem1, newsItem2, newsItem3
            });

            NewsService newsService = new NewsService(mock.Object);
            List<NewsItem> result = newsService.GetNewsItem(new FilterNewsItemParameters(){IsNewest = true}).ToList();

            Assert.IsTrue(result != null);
            Assert.IsTrue(result[0].LastChangeDate == lastChangeData20211103);
            Assert.IsTrue(result[1].LastChangeDate == lastChangeData20211102);
            Assert.IsTrue(result[2].LastChangeDate == createData2021);
        }

        [Test]
        public void GetNewsItem_FilterByNotExistCategories_ReturnEmptyCollection()
        {
            DateTime data = new DateTime(2019, 01, 01, 12, 00, 00);

            NewsItem newsItem1 = new NewsItem()
            {
                Id = Guid.NewGuid().ToString(),
                Category = null,
                Description = "Description1",
                Files = null,
                Header = "Header1",
                CreatedDate = data,
                LastChangeDate = data
            };
            NewsItem newsItem2 = new NewsItem()
            {
                Id = Guid.NewGuid().ToString(),
                Category = null,
                Description = "Description2",
                Files = null,
                Header = "Header2",
                CreatedDate = data,
                LastChangeDate = data
            };
            NewsItem newsItem3 = new NewsItem()
            {
                Id = Guid.NewGuid().ToString(),
                Category = null,
                Description = "Description3",
                Files = null,
                Header = "Header3",
                CreatedDate = data,
                LastChangeDate = data
            };

            Mock<IRepository<NewsItem>> mock = new Mock<IRepository<NewsItem>>();
            mock.Setup(m => m.GetAll).Returns(new List<NewsItem>()
            {
                newsItem1, newsItem2, newsItem3
            });
            var filterParams = new FilterNewsItemParameters()
            {
                CategoryIds = new List<string>() { Guid.NewGuid().ToString() }
            };

            NewsService newsService = new NewsService(mock.Object);
            List<NewsItem> result = newsService.GetNewsItem(filterParams).ToList();

            Assert.IsTrue(result != null);
            Assert.IsTrue(result.Count == 0);
        }

        [Test]
        public void GetNewsItem_FilterByCategories_ReturnOneItem()
        {
            string guidCategory = Guid.NewGuid().ToString();

            DateTime data = new DateTime(2019, 01, 01, 12, 00, 00);

            NewsItem newsItem1 = new NewsItem()
            {
                Id = Guid.NewGuid().ToString(),
                Category = null,
                Description = "Description1",
                Files = null,
                Header = "Header1",
                CreatedDate = data,
                LastChangeDate = data
            };
            NewsItem newsItem2 = new NewsItem()
            {
                Id = Guid.NewGuid().ToString(),
                Category = guidCategory,
                Description = "Description2",
                Files = null,
                Header = "Header2",
                CreatedDate = data,
                LastChangeDate = data
            };
            NewsItem newsItem3 = new NewsItem()
            {
                Id = Guid.NewGuid().ToString(),
                Category = null,
                Description = "Description3",
                Files = null,
                Header = "Header3",
                CreatedDate = data,
                LastChangeDate = data
            };
            NewsItem newsItem4 = new NewsItem()
            {
                Id = Guid.NewGuid().ToString(),
                Category = null,
                Description = "Description4",
                Files = null,
                Header = "Header4",
                CreatedDate = data,
                LastChangeDate = data
            };

            Mock<IRepository<NewsItem>> mock = new Mock<IRepository<NewsItem>>();
            mock.Setup(m => m.GetAll).Returns(new List<NewsItem>()
            {
                newsItem1, newsItem2, newsItem3, newsItem4
            });
            var filterParams = new FilterNewsItemParameters()
            {
                CategoryIds = new List<string>() { guidCategory }
            };

            NewsService newsService = new NewsService(mock.Object);
            List<NewsItem> result = newsService.GetNewsItem(filterParams).ToList();

            Assert.IsTrue(result != null);
            Assert.IsTrue(result.Count == 1);

        }

        [Test]
        public void GetNewsItem_FilterManyItemsWithCategoriesByExistCategory_ReturnManyItems()
        {
            string guidCategory = Guid.NewGuid().ToString();

            DateTime data = new DateTime(2019, 01, 01, 12, 00, 00);

            NewsItem newsItem1 = new NewsItem()
            {
                Id = Guid.NewGuid().ToString(),
                Category = null,
                Description = "Description1",
                Files = null,
                Header = "Header1",
                CreatedDate = data,
                LastChangeDate = data
            };
            NewsItem newsItem2 = new NewsItem()
            {
                Id = Guid.NewGuid().ToString(),
                Category = guidCategory,
                Description = "Description2",
                Files = null,
                Header = "Header2",
                CreatedDate = data,
                LastChangeDate = data
            };
            NewsItem newsItem3 = new NewsItem()
            {
                Id = Guid.NewGuid().ToString(),
                Category = guidCategory,
                Description = "Description3",
                Files = null,
                Header = "Header3",
                CreatedDate = data,
                LastChangeDate = data
            };
            NewsItem newsItem4 = new NewsItem()
            {
                Id = Guid.NewGuid().ToString(),
                Category = null,
                Description = "Description4",
                Files = null,
                Header = "Header4",
                CreatedDate = data,
                LastChangeDate = data
            };

            Mock<IRepository<NewsItem>> mock = new Mock<IRepository<NewsItem>>();
            mock.Setup(m => m.GetAll).Returns(new List<NewsItem>()
            {
                newsItem1, newsItem2, newsItem3, newsItem4
            });
            var filterParams = new FilterNewsItemParameters()
            {
                CategoryIds = new List<string>() { guidCategory }
            };

            NewsService newsService = new NewsService(mock.Object);
            List<NewsItem> result = newsService.GetNewsItem(filterParams).ToList();

            Assert.IsTrue(result != null);
            Assert.IsTrue(result.Count == 2);
        }

        [Test]
        public void GetNewsItem_FilterManyItemFromSubCategory_ReturnManyItemsOnlySubcategory()
        {
            string guidSubCategory1 = Guid.NewGuid().ToString();
            string guidSubCategory2 = Guid.NewGuid().ToString();
            string guidSubCategory3 = Guid.NewGuid().ToString();

            DateTime data = new DateTime(2019, 01, 01, 12, 00, 00);

            NewsItem newsItem1 = new NewsItem()
            {
                Id = Guid.NewGuid().ToString(),
                Category = guidSubCategory1,
                Description = "Description1",
                Files = null,
                Header = "Header1",
                CreatedDate = data,
                LastChangeDate = data
            };
            NewsItem newsItem2 = new NewsItem()
            {
                Id = Guid.NewGuid().ToString(),
                Category = guidSubCategory1,
                Description = "Description2",
                Files = null,
                Header = "Header2",
                CreatedDate = data,
                LastChangeDate = data
            };
            NewsItem newsItem3 = new NewsItem()
            {
                Id = Guid.NewGuid().ToString(),
                Category = guidSubCategory2,
                Description = "Description3",
                Files = null,
                Header = "Header3",
                CreatedDate = data,
                LastChangeDate = data
            };
            NewsItem newsItem4 = new NewsItem()
            {
                Id = Guid.NewGuid().ToString(),
                Category = guidSubCategory3,
                Description = "Description4",
                Files = null,
                Header = "Header4",
                CreatedDate = data,
                LastChangeDate = data
            };

            Mock<IRepository<NewsItem>> mock = new Mock<IRepository<NewsItem>>();
            mock.Setup(m => m.GetAll).Returns(new List<NewsItem>()
            {
                newsItem1, newsItem2, newsItem3, newsItem4
            });

            var filterParams = new FilterNewsItemParameters()
            {
                CategoryIds = new List<string>() { guidSubCategory1, guidSubCategory2 }
            };

            NewsService newsService = new NewsService(mock.Object);
            List<NewsItem> result = newsService.GetNewsItem(filterParams).ToList();
            
            Assert.IsTrue(result != null);
            Assert.IsTrue(result.Count == 3);
        }

        [Test]
        public void GetNewsItem_ItemWithDifferentCategorySelectNullCategory_ReturnAllNewsItem()
        {
            string guidSubCategory1 = Guid.NewGuid().ToString();
            string guidSubCategory2 = Guid.NewGuid().ToString();
            string guidSubCategory3 = Guid.NewGuid().ToString();

            DateTime data = new DateTime(2019, 01, 01, 12, 00, 00);

            NewsItem newsItem1 = new NewsItem()
            {
                Id = Guid.NewGuid().ToString(),
                Category = guidSubCategory1,
                Description = "Description1",
                Files = null,
                Header = "Header1",
                CreatedDate = data,
                LastChangeDate = data
            };
            NewsItem newsItem2 = new NewsItem()
            {
                Id = Guid.NewGuid().ToString(),
                Category = guidSubCategory1,
                Description = "Description2",
                Files = null,
                Header = "Header2",
                CreatedDate = data,
                LastChangeDate = data
            };
            NewsItem newsItem3 = new NewsItem()
            {
                Id = Guid.NewGuid().ToString(),
                Category = guidSubCategory2,
                Description = "Description3",
                Files = null,
                Header = "Header3",
                CreatedDate = data,
                LastChangeDate = data
            };
            NewsItem newsItem4 = new NewsItem()
            {
                Id = Guid.NewGuid().ToString(),
                Category = guidSubCategory3,
                Description = "Description4",
                Files = null,
                Header = "Header4",
                CreatedDate = data,
                LastChangeDate = data
            };
            NewsItem newsItem5 = new NewsItem()
            {
                Id = Guid.NewGuid().ToString(),
                Category = null,
                Description = "Description4",
                Files = null,
                Header = "Header4",
                CreatedDate = data,
                LastChangeDate = data
            };

            Mock<IRepository<NewsItem>> mock = new Mock<IRepository<NewsItem>>();
            mock.Setup(m => m.GetAll).Returns(new List<NewsItem>()
            {
                newsItem1, newsItem2, newsItem3, newsItem4, newsItem5
            });

            var filterParams = new FilterNewsItemParameters()
            {
                CategoryIds = new List<string>()
            };

            NewsService newsService = new NewsService(mock.Object);
            List<NewsItem> result = newsService.GetNewsItem(filterParams).ToList();

            Assert.IsTrue(result != null);
            Assert.IsTrue(result.Count == 5);
        }
    }
}
