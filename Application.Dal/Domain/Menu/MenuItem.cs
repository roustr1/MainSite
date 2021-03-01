using System.Collections.Generic;
using Application.Dal.Domain.Users;

namespace Application.Dal.Domain.Menu
{
    public class MenuItem : BaseEntity
    {
        public MenuItem()
        {
            UserRoles = new List<UserRoles>();
        }
        public string Name { get; set; }
        public string URL { get; set; }
        public string ActionName { get; set; }
        public string ParentId { get; set; }
        public bool IsActive { get; set; }
        public string ToolTip { get; set; }
        public virtual ICollection<UserRoles> UserRoles { get; set; }
    }
}