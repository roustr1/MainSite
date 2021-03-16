using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Application.Dal;
using Application.Dal.Domain.Menu;
using Application.Services.Menu;
using Moq;
using NUnit.Framework;

namespace UnitTests
{
    [TestFixture]
    public class MenuServiceTests
    {
        [Test]
        public void GetManyById_ManyItemsAllItemsParentNull_GetAllItems()
        {
            var listObjects = new List<MenuItem>()
            {
                new MenuItem()
                {
                    Id = Guid.NewGuid().ToString(),
                    ParentId = null
                },
                new MenuItem()
                {
                    Id = Guid.NewGuid().ToString(),
                    ParentId = null
                },
                new MenuItem()
                {
                    Id = Guid.NewGuid().ToString(),
                    ParentId = null
                },
            };
            Mock<IRepository<MenuItem>> mock = new Mock<IRepository<MenuItem>>();
            mock.Setup(m => m.GetAll()).Returns(listObjects);

            MenuService menuService = new MenuService(mock.Object);
            IEnumerable<MenuItem> result = menuService.GetManyByParentId();

            Assert.IsTrue(result.Count() == 3);
        }

        [Test]
        public void GetAll_ManyItemsOneItemWithoutParentNull_GetItemsWithParentNull()
        {
            string id = Guid.NewGuid().ToString();
            var listObjects = new List<MenuItem>()
            {
                new MenuItem()
                {
                    Id = id,
                    ParentId = null
                },
                new MenuItem()
                {
                    Id = Guid.NewGuid().ToString(),
                    ParentId = id
                },
                new MenuItem()
                {
                    Id = Guid.NewGuid().ToString(),
                    ParentId = null
                },
            };
            Mock<IRepository<MenuItem>> mock = new Mock<IRepository<MenuItem>>();
            mock.Setup(m => m.GetAll()).Returns(listObjects);

            MenuService menuService = new MenuService(mock.Object);
            IEnumerable<MenuItem> result = menuService.GetAll();

            Assert.IsTrue(result.Count() == 3);
        }

        [Test]
        public void GetManyById_ManyItemsOneItemWithoutParentNullSelectParentIdNull_GetItemsWithParentNull()
        {
            string id = Guid.NewGuid().ToString();
            var listObjects = new List<MenuItem>()
            {
                new MenuItem()
                {
                    Id = id,
                    ParentId = null
                },
                new MenuItem()
                {
                    Id = Guid.NewGuid().ToString(),
                    ParentId = id
                },
                new MenuItem()
                {
                    Id = Guid.NewGuid().ToString(),
                    ParentId = null
                },
            };
            Mock<IRepository<MenuItem>> mock = new Mock<IRepository<MenuItem>>();
            mock.Setup(m => m.GetAll()).Returns(listObjects);

            MenuService menuService = new MenuService(mock.Object);
            IEnumerable<MenuItem> result = menuService.GetManyByParentId();
        }
    }
}
