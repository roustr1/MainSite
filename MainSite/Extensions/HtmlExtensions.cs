using System.IO;
using System.Text;
using System.Text.Encodings.Web;
using MainSite.Models.Common;
using MainSite.Models.UI.Paging;
using Microsoft.AspNetCore.Html;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace MainSite.Extensions
{
    public static class HtmlExtensions
    {
        /// <summary>
        /// Convert IHtmlContent to string
        /// </summary>
        /// <param name="htmlContent">HTML content</param>
        /// <returns>Result</returns>
        public static string RenderHtmlContent(this IHtmlContent htmlContent)
        {
            using var writer = new StringWriter();
            htmlContent.WriteTo(writer, HtmlEncoder.Default);
            var htmlOutput = writer.ToString();
            return htmlOutput;
        }
        //we have two pagers:
        //The first one can have custom routes
        //The second one just adds query string parameter
        public static IHtmlContent Pager<TModel>(this IHtmlHelper<TModel> html, PagerModel model)
        {
            if (model.TotalRecords == 0)
                return new HtmlString("");



            var links = new StringBuilder();
            if (model.ShowTotalSummary && (model.TotalPages > 0))
            {
                links.Append("<li class=\"total-summary btn btn-defaultMainSite\">");
                links.Append(string.Format(model.CurrentPageText, model.PageIndex + 1, model.TotalPages, model.TotalRecords));
                links.Append("</li>");
            }
            if (model.ShowPagerItems && (model.TotalPages > 1))
            {
                if (model.ShowFirst)
                {
                    //first page
                    if ((model.PageIndex >= 3) && (model.TotalPages > model.IndividualPagesDisplayedCount))
                    {
                        model.RouteValues.page = 1;

                        links.Append("<li class=\"first-page btn btn-defaultMainSite\">");
                        if (model.UseRouteLinks)
                        {
                            var link = html.RouteLink(model.FirstButtonText, model.RouteActionName, model.RouteValues, new { title = "Первая страница" });
                            links.Append(link.RenderHtmlContent());
                        }
                        else
                        {
                            var link = html.ActionLink(model.FirstButtonText, model.RouteActionName, model.RouteValues, new { title = "Первая страница" });
                            links.Append(link.RenderHtmlContent());
                        }
                        links.Append("</li>");
                    }
                }
                if (model.ShowPrevious)
                {
                    //previous page
                    if (model.PageIndex > 0)
                    {
                        model.RouteValues.page = (model.PageIndex);

                        links.Append("<li class=\"previous-page btn btn-defaultMainSite\">");
                        if (model.UseRouteLinks)
                        {
                            var link = html.RouteLink(model.PreviousButtonText, model.RouteActionName, model.RouteValues, new { title = "Предыдущая страница" });
                            links.Append(link.RenderHtmlContent());
                        }
                        else
                        {
                            var link = html.ActionLink(model.PreviousButtonText, model.RouteActionName, model.RouteValues, new { title = "Предыдущая страница" });
                            links.Append(link.RenderHtmlContent());
                        }
                        links.Append("</li>");
                    }
                }
                if (model.ShowIndividualPages)
                {
                    //individual pages
                    var firstIndividualPageIndex = model.GetFirstIndividualPageIndex();
                    var lastIndividualPageIndex = model.GetLastIndividualPageIndex();
                    for (var i = firstIndividualPageIndex; i <= lastIndividualPageIndex; i++)
                    {
                        if (model.PageIndex == i)
                        {
                            links.AppendFormat("<li class=\"active current-page\"><a>{0}</a></li>", (i + 1));
                        }
                        else
                        {
                            model.RouteValues.page = (i + 1);

                            links.Append("<li class=\"individual-page waves-effect\">");
                            if (model.UseRouteLinks)
                            {
                                var link = html.RouteLink((i + 1).ToString(), model.RouteActionName, model.RouteValues, new { title = string.Format("Страница {0}", (i + 1)) });
                                links.Append(link.RenderHtmlContent());
                            }
                            else
                            {
                                var link = html.ActionLink((i + 1).ToString(), model.RouteActionName, model.RouteValues, new { title = string.Format("Страница {0}", (i + 1)) });
                                links.Append(link.RenderHtmlContent());
                            }
                            links.Append("</li>");
                        }
                    }
                }
                if (model.ShowNext)
                {
                    //next page
                    if ((model.PageIndex + 1) < model.TotalPages)
                    {
                        model.RouteValues.page = (model.PageIndex + 2);

                        links.Append("<li class=\"next-page btn btn-defaultMainSite\">");
                        if (model.UseRouteLinks)
                        {
                            var link = html.RouteLink(model.NextButtonText, model.RouteActionName, model.RouteValues, new { title = "Следующая страница" });
                            links.Append(link.RenderHtmlContent());
                        }
                        else
                        {
                            var link = html.ActionLink(model.NextButtonText, model.RouteActionName, model.RouteValues, new { title = "Следующая страница" });
                            links.Append(link.RenderHtmlContent());
                        }
                        links.Append("</li>");
                    }
                }
                if (model.ShowLast)
                {
                    //last page
                    if (((model.PageIndex + 3) < model.TotalPages) && (model.TotalPages > model.IndividualPagesDisplayedCount))
                    {
                        model.RouteValues.page = model.TotalPages;

                        links.Append("<li class=\"last-page btn btn-defaultMainSite\">");
                        if (model.UseRouteLinks)
                        {
                            var link = html.RouteLink(model.LastButtonText, model.RouteActionName, model.RouteValues, new { title = "Последняя страница" });
                            links.Append(link.RenderHtmlContent());
                        }
                        else
                        {
                            var link = html.ActionLink(model.LastButtonText, model.RouteActionName, model.RouteValues, new { title = "Последняя страница" });
                            links.Append(link.RenderHtmlContent());
                        }
                        links.Append("</li>");
                    }
                }
            }
            var result = links.ToString();
            if (!string.IsNullOrEmpty(result))
            {
                result = "<ul>" + result + "</ul>";
            }
            return new HtmlString(result);
        }


        public static Pager Pager(this IHtmlHelper helper, IPageableModel pagination)
        {
            return new Pager(pagination, helper.ViewContext);
        }

    }
}