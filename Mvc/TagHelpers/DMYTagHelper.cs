using Microsoft.AspNetCore.Razor.TagHelpers;
using System;

namespace MVC.TagHelpers
{
    /// <summary>
    /// DateTime output format will be "dd.MM.yyyy"
    /// </summary>
    [HtmlTargetElement(Attributes = TagNames.SS_DMY)]
    public class DMYTagHelper : TagHelper
    {
        /// <summary>
        /// DateTime output format will be "dd.MM.yyyy"
        /// </summary>
        [HtmlAttributeName(TagNames.SS_DMY)]
        public DateTime? Date { get; set; }

        public override void Process(TagHelperContext context, TagHelperOutput output)
        {
            if(Date.HasValue)
                output.Content.SetContent(Date.Value.ToString("dd.MM.yyyy"));
        }
    }
}