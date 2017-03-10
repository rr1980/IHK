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
                template += $"<select id='{Id}' class='form-control' data-bind='options: {Options}, selectedOptions: {Value}, optionsText: \"{OptionsText}\",optionsValue : \"{OptionsValue}\",  multiselect: {{numberDisplayed: 5}}' multiple='multiple'></select>";
            }
            else
            {
                template += $"<select id='{Id}' class='selectpicker form-control show-tick input-sm' data-style='btn btn-default btn-sm' data-bind='selectPicker: {Value},value: {Value} ,optionsText: \"{OptionsText}\", optionsValue : \"{OptionsValue}\",options: {Options}'></select>";
            }
            output.Content.SetHtmlContent(template);

        }
    }
}
