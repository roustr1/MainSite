using System.Collections.Generic;

namespace MainSite.Areas.Admin.Models.Users
{
    /// <summary>
    /// Модель для отображения связи пользователь-список присвоеных ролей
    /// </summary>
    public   class UserModel:BaseAppEntityModel
    {
        public UserModel()
        {
            UserRoles = new List<string>();
        }
        public string UserName { get; set; }
        public string SystemName { get; set; }
        public string FullName { get; set; }
        public string IPAddress { get; set; }
        public IEnumerable<string> UserRoles { get; set; }

 
    }
}