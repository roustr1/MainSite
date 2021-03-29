using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Application.Dal.Domain.Users;

namespace Application.Dal.Domain.Menu
{
    public class MenuItem : BaseEntity
    {
        public MenuItem()
        {
            UserRoles = new List<UserRoles>();
        }
        /// <summary>
        /// отображаемый текст ссылки
        /// </summary>
        [Display(Name = "Название"), Required]
        public string Name { get; set; }

        /// <summary>
        /// относительный путь до иконки соответствующего пункта меню
        /// </summary>
        public string UrlIcone { get; set; }

        /// <summary>
        /// метод действия
        /// </summary>
        public string ActionName { get; set; }
        /// <summary>
        /// Родительский элемент
        /// </summary>
        [Display(Name = "Родительский элемент")]
        public string ParentId { get; set; }
        /// <summary>
        /// Оображается в списке
        /// </summary>
        [Display(Name = "Оображается в списке")]
        public bool IsActive { get; set; }
        /// <summary>
        /// Всплывающая подсказка при наведении
        /// </summary>
        [Display(Name = "Текст подсказки")]
        public string ToolTip { get; set; }
        /// <summary>
        /// Роли, которым доступен данный пункт меню
        /// </summary>
        public virtual ICollection<UserRoles> UserRoles { get; set; }

        public int Index { get; set; }
    }
}