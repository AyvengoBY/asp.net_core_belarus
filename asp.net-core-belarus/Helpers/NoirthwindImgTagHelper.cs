using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Razor.TagHelpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace asp.net_core_belarus.Helpers
{
    [HtmlTargetElement("a", Attributes= "northwind-id")]
    public class NoirthwindImgTagHelper : TagHelper
    {
        public string NorthwindId { get; set; }
        public override void Process(TagHelperContext context, TagHelperOutput output)
        {
            var t = context.Items;

            var link = $"images//{NorthwindId}";

            output.Attributes.SetAttribute("href", link);
        }
    }
}
