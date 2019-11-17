using asp.net_core_belarus.Data;
using Microsoft.AspNetCore.Html;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using static System.Net.WebRequestMethods;

namespace asp.net_core_belarus.Helpers
{
    public static class HtmlHelperExtensions
    {
        public static HtmlString NorthwindImageLink<TModel, TResult>(this IHtmlHelper<TModel> htmlHelper,
            Expression<Func<TModel, TResult>> expression, int imgId, string linkText)
        {
            return new HtmlString(String.Format("<a href='Categories/GetImage/{0}'>{1}</a>", imgId, linkText));
        }
    }
}
