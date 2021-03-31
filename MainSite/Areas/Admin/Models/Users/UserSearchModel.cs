namespace MainSite.Areas.Admin.Models.Users
{
    public class UserSearchModel : BaseAppEntityModel
    {
        public string[] SelectedRoleIds { get; set; }
        public string UserName { get; set; }
        public string IpAddress { get; set; }
        public int Pagesize { get; set; } = int.MaxValue;
        public int PageNum { get; set; }
    }
}