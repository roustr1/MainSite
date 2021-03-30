using MainSite.Areas.Admin.Models.Security;

namespace MainSite.Areas.Admin.Factories
{
    public interface ISecurityModelFactory
    {
        /// <summary>
        /// Prepare permission mapping model
        /// </summary>
        /// <param name="model">Permission mapping model</param>
        /// <returns>Permission mapping model</returns>
        PermissionMappingModel PreparePermissionMappingModel(PermissionMappingModel model);
    }
}