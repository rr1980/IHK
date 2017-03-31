using Microsoft.AspNetCore.Razor.TagHelpers;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace IHK.Web.TagHelpers
{
    [HtmlTargetElement("th_numUpDown", Attributes = "th-label, th-value,th-up,th-down")]
    public class TH_NumericUpDown : TagHelper
    {
        [HtmlAttributeName("th-label")]
        public string Label { get; set; }

        [HtmlAttributeName("th-value")]
        public string Value { get; set; }

        [HtmlAttributeName("th-up")]
        public string Up { get; set; }

        [HtmlAttributeName("th-down")]
        public string Down { get; set; }

        [HtmlAttributeName("th-min")]
        public int Min { get; set; } = 1;

        [HtmlAttributeName("th-max")]
        public int Max { get; set; } = 9;

        [HtmlAttributeName("th-step")]
        public int Step { get; set; } = 1;

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

            template += $"<label class='control-label'>{Label}</label>";
            template += $"<div class='spinbox' data-min='{Min}' data-max='{Max}' data-step='{Step}'>";
            template += $"<input type='text' class='form-control spinbox-input' data-bind='value: {Value}' />";
            template += $"<div class='spinbox-buttons'>";
            template += $"<button data-bind='click: {Up}' class='spinbox-up btn btn-default btn-xs' type='button'>+</button>";
            template += $"<button data-bind='click: {Down}' class='spinbox-down btn btn-default btn-xs' type='button'>-</button>";
            template += "</div>";
            template += "</div>";

            output.Content.SetHtmlContent(template);
        }
    }
}
