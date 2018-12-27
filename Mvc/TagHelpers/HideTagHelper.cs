using Microsoft.AspNetCore.Razor.TagHelpers;

namespace MVC.TagHelpers
{
    [HtmlTargetElement(Attributes = TagNames.SS_HIDE)]
    public class HideTagHelper : TagHelper
    {
        [HtmlAttributeName(TagNames.SS_HIDE)]
        public bool Condition { get; set; }

        public override void Process(TagHelperContext context, TagHelperOutput output)
        {
            if (Condition)
            {
                output.SuppressOutput();
            }
        }
    }
}