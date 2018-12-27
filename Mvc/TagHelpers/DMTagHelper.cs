using Microsoft.AspNetCore.Razor.TagHelpers;
using System;

namespace MVC.TagHelpers
{
    /// <summary>
    /// DateTime output format will be "dd.MM"
    /// </summary>
    [HtmlTargetElement(Attributes = TagNames.SS_DM)]
    public class DMTagHelper : TagHelper
    {
        /// <summary>
        /// DateTime output format will be "dd.MM"
        /// </summary>
        [HtmlAttributeName(TagNames.SS_DM)]
        public DateTime Date { get; set; }

        public override void Process(TagHelperContext context, TagHelperOutput output)
        {
            output.Content.SetContent( Date.ToString("dd.MM"));
        }
    }
}