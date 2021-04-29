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
            mock.Setup(m => m.GetAll).Returns(listObjects);

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
            mock.Setup(m => m.GetAll).Returns(listObjects);

            MenuService menuService = new MenuService(mock.Object);
            IEnumerable<MenuItem> result = menuService.GetAll();

            Assert.IsTrue(result.Count() == 3);
        }

        [Test]
        public void GetManyByParentId_ManyItemsOneItemWithoutParentNullSelectParentIdNull_GetItemsWithParentNullAndChildren()
        {
            string guidRootMenuWithChildren = Guid.NewGuid().ToString();
            MenuItem menuItem1 = new MenuItem() { Name = "1", Id = guidRootMenuWithChildren, IsActive = true };
            MenuItem menuItem2 = new MenuItem() { Name = "2", Id = Guid.NewGuid().ToString(), IsActive = true };
            MenuItem menuItem3 = new MenuItem() { Name = "1.1", Id = Guid.NewGuid().ToString(), ParentId = menuItem1.Id, IsActive = true };

            var listObjects = new List<MenuItem>()
            {
                menuItem1, menuItem2, menuItem3
            };
            Mock<IRepository<MenuItem>> mock = new Mock<IRepository<MenuItem>>();
            mock.Setup(m => m.GetAll).Returns(listObjects);

            MenuService menuService = new MenuService(mock.Object);
            IEnumerable<MenuItem> result = menuService.GetManyByParentId();

            Assert.IsTrue(result != null);
            Assert.IsTrue(result.Count() == 2);
        }

        [Test]
        public void GetRecursionAllChildren_ManyItemsThreeLvlSelectParentIdNotNull_GetItemsAllItems()
        {
            string guidRootMenuWithChildren = Guid.NewGuid().ToString();
            MenuItem menuItem1 = new MenuItem() { Name = "1", Id = guidRootMenuWithChildren, IsActive = true };
            MenuItem menuItem2 = new MenuItem() { Name = "2", Id = Guid.NewGuid().ToString(), IsActive = true };
            MenuItem menuItem3 = new MenuItem() { Name = "3", Id = Guid.NewGuid().ToString(), IsActive = true };
            MenuItem menuItem4 = new MenuItem() { Name = "1.1", Id = Guid.NewGuid().ToString(), ParentId = menuItem1.Id, IsActive = true };
            MenuItem menuItem5 = new MenuItem() { Name = "1.2", Id = Guid.NewGuid().ToString(), ParentId = menuItem1.Id, IsActive = true };
            MenuItem menuItem6 = new MenuItem() { Name = "1.2.1", Id = Guid.NewGuid().ToString(), ParentId = menuItem5.Id, IsActive = true };
            MenuItem menuItem7 = new MenuItem() { Name = "2.1", Id = Guid.NewGuid().ToString(), ParentId = menuItem2.Id, IsActive = true };
            MenuItem menuItem8 = new MenuItem() { Name = "3.1", Id = Guid.NewGuid().ToString(), ParentId = menuItem3.Id, IsActive = true };

            var listObjects = new List<MenuItem>()
            {
                menuItem1, menuItem2, menuItem3, menuItem4,
                menuItem5, menuItem6, menuItem7, menuItem8,
            };
            Mock<IRepository<MenuItem>> mock = new Mock<IRepository<MenuItem>>();
            mock.Setup(m => m.GetAll).Returns(listObjects);

            MenuService menuService = new MenuService(mock.Object);
            IEnumerable<MenuItem> result = menuService.GetRecursionAllChildren(guidRootMenuWithChildren);

            Assert.IsTrue(result != null);
            Assert.IsTrue(result.Count() == 3);
        }
    }
}
