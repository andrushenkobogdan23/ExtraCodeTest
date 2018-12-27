using Microsoft.AspNetCore.Razor.TagHelpers;
using System;

namespace MVC.TagHelpers
{
    /// <summary>
    /// add attribute "selected"
    /// </summary>
    [HtmlTargetElement("option", Attributes = TagNames.SS_SELECTED)]
    public class SelectedTagHelper : TagHelper
    {
        [HtmlAttributeName(TagNames.SS_SELECTED)]
        public bool Selected { get; set; }

        public override void Process(TagHelperContext context, TagHelperOutput output)
        {
            const string SELECTED = "selected";
            if (Selected)
            {
                output.Attributes.Add(SELECTED, SELECTED);
            }
            else
            {
                output.Attributes.RemoveAll(SELECTED);
            }
        }
    }
}