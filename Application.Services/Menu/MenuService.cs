using System;
using System.Collections.Generic;
using System.Linq;
using Application.Dal;
using Application.Dal.Domain.Menu;
using Application.Services.Utils;

namespace Application.Services.Menu
{
    public class MenuService : IMenuService
    {
        private readonly IRepository<MenuItem> _repository;

        public MenuService(IRepository<MenuItem> repository)
        {
            _repository = repository;
        }

        public MenuItem Get(string id)
        {
            if (string.IsNullOrEmpty(id)) throw new ArgumentNullException("id is null");
            return _repository.Get(id);
        }

        public void InsertItem(MenuItem mi)
        {
            if (mi.ActionName == null)
                mi.ActionName = new TranslitMethods.Translitter().Translit(mi.Name, TranslitMethods.TranslitType.Gost)
                    .Replace(' ', '_');

            var maxIndexForCurrentDirectory = GetMaximumIndexForCategory(mi.ParentId);

            mi.Index = maxIndexForCurrentDirectory + 1;

            _repository.Add(mi);
        }
        
        public void DeleteItem(MenuItem mi)
        {
            if (mi == null) throw new NullReferenceException("menu item is null");
            //удаление дочерних элементов вместе с родительским
            //todo поискать рекурсивный обход дерева
            foreach (var elem in GetManyByParentId(mi.Id))
            {
                _repository.Delete(elem);
            }

            _repository.Delete(mi);
        }

        public void UpdateItem(MenuItem mi)
        {
            if (mi == null) throw new NullReferenceException("menu item is null");

            _repository.Update(mi);
        }

        public IEnumerable<MenuItem> GetAll()
        {
            return _repository.GetAll;
        }

        public IEnumerable<MenuItem> GetManyByParentId(string parentId = null)
        {
            return _repository.GetAll.Where(p => p.ParentId == parentId);
        }

        public IEnumerable<MenuItem> GetMenu(string userRole)
        {
            return _repository.GetAll.Where(m => m.UserRoles.Select(s => s.Id).Contains(userRole));
        }
        
        public IEnumerable<MenuItem> GetRecursionAllChildren(string id)
        {
            return AllChildrenCategories(id);
        }

        private IList<MenuItem> AllChildrenCategories(string categoryId)
        {
            var result = GetManyByParentId(categoryId).ToList();
            var tres = new List<MenuItem>();

            foreach (var menuItem in result)
            {
                tres.AddRange(GetManyByParentId(menuItem.Id));
            }

            result.AddRange(tres);

            return result;
        }

        private int GetMaximumIndexForCategory(string categoryId)
        {
            var result = 0;

            var menuItems = GetManyByParentId(categoryId).ToList();
            if (menuItems.Any())
            {
                var menuItem = menuItems.OrderBy(m => m.Index).LastOrDefault();
                if (menuItem != null)
                {
                    result = menuItem.Index;
                }
            }

            return result;
        }
    }
}