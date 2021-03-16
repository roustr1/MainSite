using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Application.Dal.Domain.Menu;
using Application.Services.Menu;
using MainSite.Controllers;
using MainSite.Models.UI.Menu;
using Moq;
using NUnit.Framework;

namespace UnitTests
{
    [TestFixture]
    public class NavControllerTests
    {
        [Test]
        public void Menu_TestOneRangeMenu_GetFlatMenu()
        {
            var listObjects = new List<MenuItem>()
            {
                new MenuItem()
                {
                    Id = Guid.NewGuid().ToString(),
                    ParentId = null,
                },
                new MenuItem()
                {
                    Id = Guid.NewGuid().ToString(),
                    ParentId = null,
                },
                new MenuItem()
                {
                    Id = Guid.NewGuid().ToString(),
                    ParentId = null,
                },
            };
            Mock<IMenuService> mock = new Mock<IMenuService>();
            mock.Setup(m => m.GetMenuItem()).Returns(listObjects);
            NavController navController = new NavController(mock.Object);

            IEnumerable<MenuItemViewModel> result = (IEnumerable<MenuItemViewModel>) navController.Menu().Model;

            Assert.IsTrue(result.Count() == listObjects.Count);
        }

        [Test]
        public void Menu_TestOneRangeMenuWithSelectedElementMenuAndExistId_GetFlatMenuWithSelectedElementMenu()
        {
            var guidSelectedMenuItem = Guid.NewGuid().ToString();
            var listObjects = new List<MenuItem>()
            {
                new MenuItem()
                {
                    Id = Guid.NewGuid().ToString(),
                    ParentId = null,
                },
                new MenuItem()
                {
                    Id = guidSelectedMenuItem,
                    ParentId = null,
                },
                new MenuItem()
                {
                    Id = Guid.NewGuid().ToString(),
                    ParentId = null,
                },
            };
            Mock<IMenuService> mock = new Mock<IMenuService>();
            mock.Setup(m => m.GetMenuItem()).Returns(listObjects);
            NavController navController = new NavController(mock.Object);

            IEnumerable<MenuItemViewModel> result = (IEnumerable<MenuItemViewModel>)navController.Menu(guidSelectedMenuItem).Model;
            
            Assert.IsTrue(result != null);
            Assert.IsTrue(result.Count() == listObjects.Count);

            MenuItemViewModel selectedItem = result.FirstOrDefault(i => i.Id == guidSelectedMenuItem);
            Assert.IsTrue(selectedItem != null);
            Assert.IsTrue(selectedItem.IsActive);
            Assert.IsTrue(result.Count(i => i.IsActive) == 1);
        }

        [Test]
        public void Menu_TestOneRangeMenuWithNoExistSelectedId_GetFlatMenuWithoutSelectedElementMenu()
        {
            var guidSelectedMenuItem = Guid.NewGuid().ToString();
            var listObjects = new List<MenuItem>()
            {
                new MenuItem()
                {
                    Id = Guid.NewGuid().ToString(),
                    ParentId = null,
                },
                new MenuItem()
                {
                    Id = Guid.NewGuid().ToString(),
                    ParentId = null,
                },
                new MenuItem()
                {
                    Id = Guid.NewGuid().ToString(),
                    ParentId = null,
                },
            };
            Mock<IMenuService> mock = new Mock<IMenuService>();
            mock.Setup(m => m.GetMenuItem()).Returns(listObjects);
            NavController navController = new NavController(mock.Object);

            IEnumerable<MenuItemViewModel> result = (IEnumerable<MenuItemViewModel>)navController.Menu(guidSelectedMenuItem).Model;

            Assert.IsTrue(result != null);
            Assert.IsTrue(result.Count() == listObjects.Count);

            MenuItemViewModel selectedItem = result.FirstOrDefault(i => i.Id == guidSelectedMenuItem);
            Assert.IsTrue(selectedItem == null);
            Assert.IsTrue(result.Count(i => i.IsActive) == 0);
        }
    }
}
