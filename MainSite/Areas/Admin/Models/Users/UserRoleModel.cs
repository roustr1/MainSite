using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace MainSite.Areas.Admin.Models.Users
{
    /// <summary>
    /// Represents a user role model
    /// </summary>
    public partial class UserRoleModel : BaseAppEntityModel
    {
        #region Properties

        [Display (Name="User.Name")]
        public string Name { get; set; }
 

        [Display (Name="User.IsActive")]
        public bool Active { get; set; }

        [Display (Name="User.IsSystemRole")]
        public bool IsSystemRole { get; set; }

        [Display (Name="User.SystemName")]
        public string SystemName { get; set; }
         
        #endregion
    }
}