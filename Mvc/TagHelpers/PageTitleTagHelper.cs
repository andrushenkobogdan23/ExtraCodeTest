using Microsoft.AspNetCore.Razor.TagHelpers;

namespace MVC.TagHelpers
{

    [HtmlTargetElement(TagNames.SS_TITLE)]
    public class PageTitleTagHelper : TagHelper
    {
        [HtmlAttributeName(TagNames.SS_SUBTITLE)]
        public string SubTitle { get; set; } = string.Empty;

        public override void Process(TagHelperContext context, TagHelperOutput output)
        {
            output.TagName = "h2";

            if (!string.IsNullOrEmpty(SubTitle))
            {
                output.PostContent.SetHtmlContent($"<h5 class=\"text-muted\">{SubTitle}</h5>");
            }

            output.PostContent.AppendHtml("<hr class=\"mb-3\"/>");
        }
    }
}