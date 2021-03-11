using System.Collections.Generic;
using Application.Dal.Domain.Menu;

namespace Application.Services.Menu
{
    public interface IShowMenu
    {
        IEnumerable<MenuItem> GetMenuItem(string parentId = null);
    }
}