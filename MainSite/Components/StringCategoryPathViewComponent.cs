using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Application.Dal.Domain.Menu;
using Application.Services.Menu;
using MainSite.Utils;
using Microsoft.AspNetCore.Html;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ViewComponents;

namespace MainSite.Components
{
    [ViewComponent(Name = "CategoryPathComponent")]
    public class StringCategoryPathViewComponent
    {
        private IMenuService _service;
        public StringCategoryPathViewComponent(IMenuService service)
        {
            _service = service;
        }

        public IViewComponentResult Invoke(string categoryId = null)
        {
            const string breadcrumb = "breadcrumb";
            var stringBuilder = new StringBuilder();

            var listNames = new List<string>();

            var id = categoryId;
            while (id != null)
            {
                var item = _service.Get(id);

                listNames.Add(item.Name);

                id = item.ParentId;
            }

            listNames.Reverse();

            stringBuilder.Append("<div class=\"nav-wrapper\"><div class=\"col s12\">");

            if (string.IsNullOrWhiteSpace(categoryId))
            {
                stringBuilder.AppendHtmlA("Не выбран раздел", breadcrumb);
            }
            else
            {
                foreach (var name in listNames)
                {
                    stringBuilder.AppendHtmlA(name, breadcrumb);
                }
            }

            stringBuilder.Append("</div></div>");

            var result = stringBuilder.ToString();

            return new HtmlContentViewComponentResult(new HtmlString(result));
        }
    }
}
