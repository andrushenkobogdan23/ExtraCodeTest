using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Razor.TagHelpers;
using System;
using System.Collections.Generic;

namespace MVC.TagHelpers
{
    /// <summary>
    /// fill select html control with given items
    /// </summary>
    [HtmlTargetElement("select", Attributes = TagNames.SS_ITEMS)]
    public class SelectTagHelper : TagHelper
    {
        [HtmlAttributeName(TagNames.SS_ITEMS)]
        public IEnumerable<SelectListItem> Items { get; set; }

        public override void Process(TagHelperContext context, TagHelperOutput output)
        {
            foreach (var item in Items)
            {
                output.Content.AppendFormat("<option value=\"{0}\"{1}>{2}</option>", item.Value, item.Selected?" selected":string.Empty, item.Text);
            }
        }
    }
}