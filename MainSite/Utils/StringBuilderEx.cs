using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MainSite.Utils
{
    public static class StringBuilderEx
    {
        public static StringBuilder AppendHtmlLi(this StringBuilder stringBuilder, string value)
        {
            stringBuilder.Append("<li>");
            stringBuilder.Append(value);
            stringBuilder.Append("</li>");

            return stringBuilder;
        }

        public static StringBuilder AppendHtmlA(this StringBuilder stringBuilder, string value, string cssClass)
        {
            stringBuilder.Append("<a");
            stringBuilder.AppendHtmlCssClass(cssClass);
            stringBuilder.Append(">");
            stringBuilder.Append(value);
            stringBuilder.Append("</a>");

            return stringBuilder;
        }

        public static StringBuilder AppendHtmlCssClass(this StringBuilder stringBuilder, string value)
        {
            stringBuilder.Append(" class=\"");
            stringBuilder.Append(value);
            stringBuilder.Append("\"");

            return stringBuilder;
        }
    }
}
