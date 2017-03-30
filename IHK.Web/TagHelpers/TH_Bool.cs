using Microsoft.AspNetCore.Razor.TagHelpers;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace IHK.Web.TagHelpers
{
    [HtmlTargetElement("th_bool", Attributes = "th-label, th-value")]
    public class TH_Bool : TagHelper
    {
        [HtmlAttributeName("th-label")]
        public string Label { get; set; }

        [HtmlAttributeName("th-value")]
        public string Value { get; set; }

        [HtmlAttributeName("th-id")]
        public string Id { get; set; }

        [HtmlAttributeName("th-col")]
        public int Col { get; set; } = 12;

        private string template = "";

        public override void Process(TagHelperContext context, TagHelperOutput output)
        {
            Value = TagHelperTools.FirstLetterToLower(Value);
            Id = Id ?? TagHelperTools.GetID();

            output.TagName = "div";
            output.Attributes.Add("class", $"form-group-sm col-md-{Col}");
            output.TagMode = TagMode.StartTagAndEndTag;

            template += $"<div class='checkbox'>";
            template += $"<input type='checkbox' data-bind='checked: {Value}' />";
            template += $"<label>{Label}</label>";
            template += "</div>";


            output.Content.SetHtmlContent(template);
        }
    }
}
