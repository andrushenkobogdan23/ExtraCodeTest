using Microsoft.AspNetCore.Razor.TagHelpers;

namespace MVC.TagHelpers
{

    [HtmlTargetElement("pagefooter")]
    public class PageFooterTagHelper : TagHelper
    {

        public override void Process(TagHelperContext context, TagHelperOutput output)
        {
            output.TagName = "div";
            output.Attributes.Add("class", "mt-3");
        }
    }
}