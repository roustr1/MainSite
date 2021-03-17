using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Application.Dal.Domain.Menu;
using Application.Services.Menu;
using MainSite.Components;
using MainSite.Controllers;
using MainSite.Models.UI.Menu;
using Microsoft.AspNetCore.Mvc.ViewComponents;
using Moq;
using NUnit.Framework;

namespace UnitTests
{
    [TestFixture]
    public class NavComponentTests
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
            mock.Setup(m => m.GetManyByParentId(null)).Returns(listObjects);
            mock.Setup(m => m.GetAll()).Returns(listObjects);
            NavViewComponent navViewComponent = new NavViewComponent(mock.Object);

            IEnumerable<MenuItemViewModel> result = (IEnumerable<MenuItemViewModel>)((ViewViewComponentResult)navViewComponent.Invoke()).ViewData.Model;

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
            mock.Setup(m => m.GetManyByParentId(null)).Returns(listObjects);
            mock.Setup(m => m.Get(guidSelectedMenuItem))
                .Returns(listObjects.FirstOrDefault(i => i.Id == guidSelectedMenuItem));
            mock.Setup(m => m.GetAll()).Returns(listObjects);
            NavViewComponent navViewComponent = new NavViewComponent(mock.Object);

            IEnumerable<MenuItemViewModel> result = (IEnumerable<MenuItemViewModel>)((ViewViewComponentResult)navViewComponent.Invoke(guidSelectedMenuItem)).ViewData.Model;

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
            mock.Setup(m => m.GetManyByParentId(null)).Returns(listObjects);
            mock.Setup(m => m.Get(guidSelectedMenuItem))
                .Returns(listObjects.FirstOrDefault(i => i.Id == guidSelectedMenuItem));
            mock.Setup(m => m.GetAll()).Returns(listObjects);
            NavViewComponent navViewComponent = new NavViewComponent(mock.Object);

            IEnumerable<MenuItemViewModel> result = (IEnumerable<MenuItemViewModel>)((ViewViewComponentResult)navViewComponent.Invoke(guidSelectedMenuItem)).ViewData.Model;

            Assert.IsTrue(result != null);
            Assert.IsTrue(result.Count() == listObjects.Count);

            MenuItemViewModel selectedItem = result.FirstOrDefault(i => i.Id == guidSelectedMenuItem);
            Assert.IsTrue(selectedItem == null);
            Assert.IsTrue(result.Count(i => i.IsActive) == 0);
        }

        [Test]
        public void Menu_TestTwoRangMenu_GetTree()
        {
            string guidParentId1 = Guid.NewGuid().ToString();
            string guidParentId2 = Guid.NewGuid().ToString();

            List<MenuItem> listMenuObjects = new List<MenuItem>()
            {
                new MenuItem()
                {
                    Id = guidParentId1,
                    ParentId = null,
                },
                new MenuItem()
                {
                    Id = guidParentId2,
                    ParentId = null,
                },
                new MenuItem()
                {
                    Id = Guid.NewGuid().ToString(),
                    ParentId = guidParentId1,
                },
                new MenuItem()
                {
                    Id = Guid.NewGuid().ToString(),
                    ParentId = guidParentId2,
                },
                new MenuItem()
                {
                    Id = Guid.NewGuid().ToString(),
                    ParentId = guidParentId1,
                },
                new MenuItem()
                {
                    Id = Guid.NewGuid().ToString(),
                    ParentId = guidParentId2,
                },
            };
            Mock<IMenuService> mock = new Mock<IMenuService>();
            mock.Setup(m => m.GetManyByParentId(null)).Returns(listMenuObjects.Where(i => i.ParentId == null));
            mock.Setup(m => m.GetManyByParentId(guidParentId1)).Returns(listMenuObjects.Where(i => i.ParentId == guidParentId1));
            mock.Setup(m => m.GetManyByParentId(guidParentId2)).Returns(listMenuObjects.Where(i => i.ParentId == guidParentId2));
            mock.Setup(m => m.GetAll()).Returns(listMenuObjects);

            NavViewComponent navViewComponent = new NavViewComponent(mock.Object);

            IEnumerable<MenuItemViewModel> result = (IEnumerable<MenuItemViewModel>)((ViewViewComponentResult)navViewComponent.Invoke()).ViewData.Model;

            Assert.IsTrue(result != null);
            Assert.IsTrue(result.Count() == 2);
            MenuItemViewModel menuItemViewModel1 = result.ElementAt(0);
            MenuItemViewModel menuItemViewModel2 = result.ElementAt(0);
            Assert.IsTrue(menuItemViewModel1.Children.Count == 2);
            Assert.IsTrue(menuItemViewModel2.Children.Count == 2);
        }

        [Test]
        public void Menu_TwoRangeAndSelectedOnTwoLvl_GetTreeWithTwoSelected()
        {
            string guidParentId1 = Guid.NewGuid().ToString();
            string guidParentId2 = Guid.NewGuid().ToString();
            string guidSelectedItemFirstLvl = guidParentId2;
            string guidSelectedItemSecondLvl = Guid.NewGuid().ToString();

            List<MenuItem> listMenuObjects = new List<MenuItem>()
            {
                new MenuItem()
                {
                    Id = guidParentId1,
                    ParentId = null,
                },
                new MenuItem()
                {
                    Id = guidParentId2,
                    ParentId = null,
                },
                new MenuItem()
                {
                    Id = Guid.NewGuid().ToString(),
                    ParentId = guidParentId1,
                },
                new MenuItem()
                {
                    Id = Guid.NewGuid().ToString(),
                    ParentId = guidParentId2,
                },
                new MenuItem()
                {
                    Id = Guid.NewGuid().ToString(),
                    ParentId = guidParentId1,
                },
                new MenuItem()
                {
                    Id = guidSelectedItemSecondLvl,
                    ParentId = guidParentId2,
                },
            };

            Mock<IMenuService> mock = new Mock<IMenuService>();
            mock.Setup(m => m.GetManyByParentId(null)).Returns(listMenuObjects.Where(i => i.ParentId == null));
            mock.Setup(m => m.GetManyByParentId(guidParentId1)).Returns(listMenuObjects.Where(i => i.ParentId == guidParentId1));
            mock.Setup(m => m.GetManyByParentId(guidParentId2)).Returns(listMenuObjects.Where(i => i.ParentId == guidParentId2));
            mock.Setup(m => m.Get(guidSelectedItemFirstLvl)).Returns(listMenuObjects.FirstOrDefault(i => i.Id == guidSelectedItemFirstLvl));
            mock.Setup(m => m.Get(guidSelectedItemSecondLvl)).Returns(listMenuObjects.FirstOrDefault(i => i.Id == guidSelectedItemSecondLvl));
            mock.Setup(m => m.GetAll()).Returns(listMenuObjects);

            NavViewComponent navViewComponent = new NavViewComponent(mock.Object);

            IEnumerable<MenuItemViewModel> result = (IEnumerable<MenuItemViewModel>)((ViewViewComponentResult)navViewComponent.Invoke(guidSelectedItemSecondLvl)).ViewData.Model;

            Assert.IsTrue(result != null);
            Assert.IsTrue(result.Count() == 2);
            Assert.IsTrue(result.Count(i => i.IsActive) == 1);

            MenuItemViewModel selectedItemFirstLvl = result.FirstOrDefault(i => i.IsActive);
            Assert.IsTrue(selectedItemFirstLvl != null);
            Assert.IsTrue(selectedItemFirstLvl.Id == guidSelectedItemFirstLvl);
            Assert.IsTrue(selectedItemFirstLvl.Children.Count(i => i.IsActive) == 1);

            MenuItemViewModel selectedItemSecondLvl = selectedItemFirstLvl.Children.FirstOrDefault(i => i.IsActive);
            Assert.IsTrue(selectedItemSecondLvl != null);
            Assert.IsTrue(selectedItemSecondLvl.Id == guidSelectedItemSecondLvl);
        }
    }
}
