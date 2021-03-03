using System.Collections.Generic;
using Application.Dal.Domain.Menu;

namespace Application.Services.Menu
{
    public interface IMenuService
    {
        MenuItem GetItem(string id);
        void InsertItem(MenuItem mi);
        void DeleteItem(MenuItem mi);
        IEnumerable<MenuItem> GetMenuItem(string parentId = null);
        IEnumerable<MenuItem> GetMenuItem();
    }
}