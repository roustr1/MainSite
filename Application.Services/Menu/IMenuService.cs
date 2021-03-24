using System.Collections.Generic;
using Application.Dal.Domain.Menu;

namespace Application.Services.Menu
{
    public interface IMenuService : IShowMenu
    {
        MenuItem Get(string id);
        void InsertItem(MenuItem mi);
        void DeleteItem(MenuItem mi);
        IEnumerable<MenuItem> GetAll();
        IEnumerable<MenuItem> GetRecursionAllChildren(string id);
    }
}