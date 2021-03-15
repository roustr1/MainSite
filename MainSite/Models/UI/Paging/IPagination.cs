namespace MainSite.Models.UI.Paging
{
    /// <summary>
    /// Generic form of <see cref="IPageableModel"/>
    /// </summary>
    /// <typeparam name="T">Type of object being paged</typeparam>
    public interface IPagination<T> : IPageableModel
    {

    }
}