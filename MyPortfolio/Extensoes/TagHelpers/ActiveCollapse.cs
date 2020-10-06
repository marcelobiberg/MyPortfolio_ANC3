using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Microsoft.AspNetCore.Razor.TagHelpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MyPortfolio.Extensoes.TagHelpers
{
    [HtmlTargetElement("ul", Attributes = "asp-ativar-collapse")]
    public class ActiveCollapse : TagHelper
    {
        [HtmlAttributeNotBound]
        [ViewContext]
        public ViewContext ViewContext { get; set; }

        [HtmlAttributeName("asp-ul-controller")]
        public string Controller { get; set; }

        public override void Process(TagHelperContext context, TagHelperOutput output)
        {
            string currentController = ViewContext.RouteData.Values["Controller"].ToString();

            if (Controller.ToLower() == currentController.ToLower())
            {
                var currentClass = context.AllAttributes["class"];
                string classValue = "show";

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
