using Microsoft.AspNetCore.Razor.TagHelpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace IHK.Web.TagHelpers
{
    [HtmlTargetElement("th_select", Attributes = "th-label, th-value, th-options, th-ovalue, th-otext")]
    public class TH_Select : TagHelper
    {
        [HtmlAttributeName("th-label")]
        public string Label { get; set; }

        [HtmlAttributeName("th-value")]
        public string Value { get; set; }

        [HtmlAttributeName("th-options")]
        public string Options { get; set; }

        [HtmlAttributeName("th-id")]
        public string Id { get; set; }

        [HtmlAttributeName("th-col")]
        public int Col { get; set; } = 12;

        [HtmlAttributeName("th-ovalue")]
        public string OptionsValue { get; set; }

        [HtmlAttributeName("th-otext")]
        public string OptionsText { get; set; }

        [HtmlAttributeName("th-isFC")]
        public bool IsFormControl { get; set; } = true;

        [HtmlAttributeName("th-withLabel")]
        public bool WithLabel { get; set; } = true;

        [HtmlAttributeName("th-multiple")]
        public bool Multiple { get; set; } = false;

        private string template = "";

        public override void Process(TagHelperContext context, TagHelperOutput output)
        {
            Value = TagHelperTools.FirstLetterToLower(Value);
            OptionsValue = TagHelperTools.FirstLetterToLower(OptionsValue);
            Options = TagHelperTools.FirstLetterToLower(Options);
            OptionsText = TagHelperTools.FirstLetterToLower(OptionsText);

            Id = Id ?? TagHelperTools.GetID();

            if (IsFormControl)
            {
                output.TagName = "div";
                output.Attributes.Add("class", $"form-group-sm col-md-{Col}");
                output.TagMode = TagMode.StartTagAndEndTag;
            }

            if (WithLabel)
            {
                template += $"<label class='control-label'>{Label}</label>";
            }

            if (Multiple)
            {
                template += $"<select multiple='true' data-bind='selectedOptions: {Value},options: {Options} ,optionsText: \"{OptionsText}\", optionsValue : \"{OptionsValue}\", select2: {{minimumResultsForSearch: Infinity}}'></select>";
            }
            else
            {
                //template += $"<select data-bind='select2:{{value: {Value},options: {Options} ,optionsText: \"{OptionsText}\", optionsValue : \"{OptionsValue}\"}}'></select>";
                template += $"<select data-bind='value:{Value}, optionsText: \"{OptionsText}\", optionsValue : \"{OptionsValue}\", options: {Options},select2: {{minimumResultsForSearch: Infinity}}'></select>";
            }
            output.Content.SetHtmlContent(template);

        }
    }
}
