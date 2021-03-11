using System;
using System.Collections.Generic;
using System.Linq;
using Application.Dal;
using Application.Dal.Domain.Menu;
using Application.Services.Utils;

namespace Application.Services.Menu
{
    public class MenuService : IShowMenu, IMenuService
    {
        private readonly IRepository<MenuItem> _repository;

        public MenuService(IRepository<MenuItem> repository)
        {
            _repository = repository;
        }

        public MenuItem GetItem(string id)
        {
            if (string.IsNullOrEmpty(id)) throw new ArgumentNullException("id is null");
            return _repository.Get(id);
        }

        public void InsertItem(MenuItem mi)
        {
            if (mi.ActionName == null)
                mi.ActionName = new TranslitMethods.Translitter().Translit(mi.Name, TranslitMethods.TranslitType.Gost)
                    .Replace(' ', '_');

            _repository.Add(mi);
        }

        public void DeleteItem(MenuItem mi)
        {
            if (mi == null) throw new NullReferenceException("menu item is null");
            //удаление дочерних элементов вместе с родительским
            //todo поискать рекурсивный обход дерева
            foreach (var elem in GetMenuItem(mi.Id))
            {
                _repository.Delete(elem);
            }

            _repository.Delete(mi);
        }

        public IEnumerable<MenuItem> GetMenuItem()
        {
            return _repository.GetAll();
        }

        public IEnumerable<MenuItem> GetMenuItem(string parentId = null)
        {
            return _repository.GetAll().Where(p => p.ParentId == parentId).AsEnumerable().ToList();
        }

        public IEnumerable<MenuItem> GetMenu(string userRole)
        {
            return _repository.GetAll().Where(m => m.UserRoles.Select(s => s.Id).Contains(userRole));
        }
    }
}