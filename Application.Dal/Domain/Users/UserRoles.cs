using System.Collections.Generic;
using Application.Dal.Domain.Permissions;

namespace Application.Dal.Domain.Users
{

    /// <summary>
    /// Represents a user role
    /// </summary>
    public partial class UserRole : BaseEntity
    {
        /// <summary>
        /// Gets or sets the user role name
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether the user role is active
        /// </summary>
        public bool Active { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether the user role is system
        /// </summary>
        public bool IsSystemRole { get; set; }

        /// <summary>
        /// Gets or sets the user role system name
        /// </summary>
        public string SystemName { get; set; }

    }

}