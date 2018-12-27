using Microsoft.AspNetCore.Razor.TagHelpers;

namespace MVC.TagHelpers
{
    [HtmlTargetElement(Attributes = TagNames.SS_MONEY)]
    public class MoneyTagHelper : TagHelper
    {
        [HtmlAttributeName(TagNames.SS_MONEY)]
        public decimal? Money { get; set; }

        public override void Process(TagHelperContext context, TagHelperOutput output)
        {
            if(Money.HasValue)
            {
                output.Content.SetContent(Money.Value.ToString("0.00 грн."));
                output.Attributes.Add("class", "text-nowrap");
            }
            //output.PreContent.SetHtmlContent("<h2>");
            //output.PostContent.SetHtmlContent("</h2>");
        }
    }
}