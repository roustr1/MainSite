using System;
using System.Collections.Generic;
using System.Linq;
using Application.Dal;
using Application.Dal.Domain.Menu;

namespace Application.Services.Menu
{
    public class MenuService : IShowMenu, IMenuService
    {
        private readonly ApplicationContext _context;

        public MenuService(ApplicationContext context)
        {
            _context = context;
        }

        public MenuItem GetItem(string id)
        {
            if (string.IsNullOrEmpty(id)) throw new ArgumentNullException("id is null");
            return _context.MenuItems.Find(id);
        }

        public void InsertItem(MenuItem mi)
        {
            if (mi.ActionName == null)
                mi.ActionName = new TranslitMethods.Translitter().Translit(mi.Name, TranslitMethods.TranslitType.Gost)
                    .Replace(' ', '_');

            _context.MenuItems.Add(mi);
            _context.SaveChanges();
        }

        public void DeleteItem(MenuItem mi)
        {
            if (mi == null) throw new NullReferenceException("menu item is null");
            //удаление дочерних элементов вместе с родительским
            //todo поискать рекурсивный обход дерева
            foreach (var elem in GetMenuItem(mi.Id))
            {
                _context.MenuItems.Remove(elem);
            }
            _context.MenuItems.Remove(mi);
            _context.SaveChanges();
        }

        public IEnumerable<MenuItem> GetMenuItem()
        {
            return _context.MenuItems.AsEnumerable();
        }

        public IEnumerable<MenuItem> GetMenuItem(string parentId = null)
        {
            return _context.MenuItems.Where(p => p.ParentId == parentId).AsEnumerable();
        }

        public IEnumerable<MenuItem> GetMenu(string userRole)
        {
            return _context.MenuItems.Where(m => m.UserRoles.Select(s => s.Id).Contains(userRole));
        }
    }
}