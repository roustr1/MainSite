using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using Application.Dal;
using Application.Dal.Domain.News;
using Application.Services.News;
using Moq;
using NUnit.Framework;

namespace UnitTests
{
    [TestFixture]
    public class PinNewsServiceTests
    {
        [Test]
        public void GetAllPinnedNewsByCategory_PinnedNewsIsNotInCategory_EmptyList()
        {
            var categoryId1 = Guid.NewGuid().ToString();
            var categoryId2 = Guid.NewGuid().ToString();
            var categoryId3 = Guid.NewGuid().ToString();
            var listObjects = new List<PinNews>
            {
                new PinNews() {CategoryId = categoryId1, NewsItemId = Guid.NewGuid().ToString()},
                new PinNews() {CategoryId = categoryId2, NewsItemId = Guid.NewGuid().ToString()}
            };

            Mock<IRepository<PinNews>> mock = new Mock<IRepository<PinNews>>();
            mock.Setup(m => m.GetMany(pn => pn.CategoryId == categoryId1))
                .Returns(listObjects.Where(pn => pn.CategoryId == categoryId1));
            mock.Setup(m => m.GetMany(pn => pn.CategoryId == categoryId2))
                .Returns(listObjects.Where(pn => pn.CategoryId == categoryId2));
            mock.Setup(m => m.GetMany(pn => pn.CategoryId == categoryId3))
                .Returns(listObjects.Where(pn => pn.CategoryId == categoryId3));

            PinNewsService service = new PinNewsService(mock.Object);

            var result = service.GetAllPinnedNewsByCategory(categoryId3);

            Assert.IsTrue(result != null);
            Assert.IsTrue(!result.Any());
        }

        [Test]
        public void GetAllPinnedNewsByCategory_PinnedNewsIsInCategory_NotEmptyList()
        {
            var categoryId1 = Guid.NewGuid().ToString();
            var categoryId2 = Guid.NewGuid().ToString();
            var categoryId3 = Guid.NewGuid().ToString();
            var listObjects = new List<PinNews>
            {
                new PinNews() {CategoryId = categoryId1, NewsItemId = Guid.NewGuid().ToString()},
                new PinNews() {CategoryId = categoryId2, NewsItemId = Guid.NewGuid().ToString()}
            };

            Mock<IRepository<PinNews>> mock = new Mock<IRepository<PinNews>>();
            mock.Setup(m => m.GetMany(pn => pn.CategoryId == categoryId1))
                .Returns(listObjects.Where(pn => pn.CategoryId == categoryId1));
            mock.Setup(m => m.GetMany(pn => pn.CategoryId == categoryId2))
                .Returns(listObjects.Where(pn => pn.CategoryId == categoryId2));
            mock.Setup(m => m.GetMany(pn => pn.CategoryId == categoryId3))
                .Returns(listObjects.Where(pn => pn.CategoryId == categoryId3));

            PinNewsService service = new PinNewsService(mock.Object);

            var result = service.GetAllPinnedNewsByCategory(categoryId1);

            Assert.IsTrue(result != null);
            Assert.IsTrue(result.Any());
            Assert.IsTrue(result.Count() == 1);
        }

    }
}
