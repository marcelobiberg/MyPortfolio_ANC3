using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Microsoft.AspNetCore.Razor.TagHelpers;

namespace MyPortfolio.Extensoes.TagHelpers
{
    [HtmlTargetElement("li", Attributes = "asp-ativar-link")]
    public class ActiveRouteTagHelper : TagHelper
    {
        [HtmlAttributeNotBound]
        [ViewContext]
        public ViewContext ViewContext { get; set; }

        [HtmlAttributeName("asp-li-action")]
        public string Action { get; set; }

        [HtmlAttributeName("asp-li-controller")]
        public string Controller { get; set; }

        public override void Process(TagHelperContext context, TagHelperOutput output)
        {
            string currentController = ViewContext.RouteData.Values["Controller"].ToString();
            string currentAction = ViewContext.RouteData.Values["Action"].ToString();

            if (Action.ToLower() == currentAction.ToLower() && Controller.ToLower() == currentController.ToLower())
            {
                var currentClass = context.AllAttributes["class"];
                string classValue = "active";

                if (currentClass != null)
                {
                    classValue += " " + currentClass.Value;
                }

                TagHelperAttribute classAttribute = new TagHelperAttribute("class", classValue);
                output.Attributes.SetAttribute(classAttribute);
            }
        }
    }
}
