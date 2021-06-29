using System.Diagnostics.CodeAnalysis;
using Application.Dal.Domain.Menu;
using Application.Dal.Domain.Permissions;
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

        /// <summary>
        /// Return new example of permission record
        /// </summary>
        /// <param name="name">Permission name</param>
        /// <param name="systemName">System name</param>
        /// <param name="category">Category</param>
        /// <returns></returns>
        PermissionRecord CreatePermissionRecord([NotNull] string name, string systemName, string category);

        PermissionRecord CreatePermissionRecordForMenu(MenuItem mi);
    }
}