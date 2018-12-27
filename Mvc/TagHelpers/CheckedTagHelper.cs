using Microsoft.AspNetCore.Razor.TagHelpers;
using System;

namespace MVC.TagHelpers
{
    /// <summary>
    /// DateTime output format will be "dd.MM"
    /// </summary>
    [HtmlTargetElement("input", Attributes = TagNames.SS_CHECKED)]
    public class CheckedTagHelper : TagHelper
    {
        [HtmlAttributeName(TagNames.SS_CHECKED)]
        public bool Checked { get; set; }

        public override void Process(TagHelperContext context, TagHelperOutput output)
        {
            const string CHECKED = "checked";
            if (Checked)
            {
                output.Attributes.Add(CHECKED, CHECKED);
            }
            else
            {
                output.Attributes.RemoveAll(CHECKED);
            }
        }
    }
}