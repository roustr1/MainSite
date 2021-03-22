using System;
using System.Security;

namespace Application.Dal.Domain.Users
{
    public class User:BaseEntity
    {
        /// <summary>
        /// Get or set user name
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Get or set user name in AD system
        /// </summary>
        public string Fio { get; set; }

        /// <summary>
        /// Get or set user name in AD system
        /// </summary>
        public string SystemName { get; set; }

        /// <summary>
        /// Gets or sets the last IP address
        /// </summary>
        public string LastIpAddress { get; set; }
        /// <summary>
        /// Gets or sets the date and time of last activity
        /// </summary>
        public DateTime LastActivityDate { get; set; }
        /// <summary>
        /// Gets or sets a value indicating whether the customer is active
        /// </summary>
        public bool Active { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether the customer has been deleted
        /// </summary>
        public bool Deleted { get; set; }
    }
}