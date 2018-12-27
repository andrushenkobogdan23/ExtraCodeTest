using Microsoft.AspNetCore.Razor.TagHelpers;
using System;

namespace MVC.TagHelpers
{
    /// <summary>
    /// DateTime output format will be "dd.MM"
    /// </summary>
    [HtmlTargetElement(Attributes = TagNames.SS_DM_FROM_TO)]
    public class DayFromToTagHelper : TagHelper
    {
        /// <summary>
        /// DateTime output format will be "dd.MM"
        /// </summary>
        [HtmlAttributeName(TagNames.SS_DM_FROM)]
        public DateTime From { get; set; }

        [HtmlAttributeName(TagNames.SS_DM_TO)]
        public DateTime To { get; set; }

        public override void Process(TagHelperContext context, TagHelperOutput output)
        {
            output.Content.SetContent(string.Format("{0:dd.MM} - {1:dd.MM}", From, To));
        }
    }
}