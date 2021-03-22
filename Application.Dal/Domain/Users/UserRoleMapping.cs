namespace Application.Dal.Domain.Users
{
    /// <summary>
    /// Represents a User-user role mapping class
    /// </summary>
    public partial class UserUserRoleMapping : BaseEntity
    {
        /// <summary>
        /// Gets or sets the customer identifier
        /// </summary>
        public string UserName { get; set; }

        /// <summary>
        /// Gets or sets the customer role identifier
        /// </summary>
        public string UserRoleId { get; set; }
    }
}