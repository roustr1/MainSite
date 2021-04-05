using System.Collections.Generic;
using MainSite.Areas.Admin.Models.Users;

namespace MainSite.Areas.Admin.Factories
{
    public interface IUserModelFactory
    {


        IEnumerable<UserModel> PrepareUserModelList(UserSearchModel searchModel);

        UserAddUserRoleModel PrepareAddUserRoleModel(UserAddUserRoleModel model);
    }
}