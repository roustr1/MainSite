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
    }
}
