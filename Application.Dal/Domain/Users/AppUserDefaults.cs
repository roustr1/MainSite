namespace Application.Dal.Domain.Users
{

    /// <summary>
    /// Represents default values related to users data
    /// </summary>
    public static partial class AppUserDefaults
    {
        #region System users roles

        /// <summary>
        /// Gets a system name of 'administrators' user role
        /// </summary>
        public static string AdministratorsRoleName => "Administrator";

        /// <summary>
        /// Gets a system name of 'moderators' user role
        /// </summary>
        public static string ModeratorsRoleName => "Moderator";

        /// <summary>
        /// Gets a system name of 'registered' user role
        /// </summary>
        public static string RegisteredRoleName => "Registered";

        #endregion
    }

}