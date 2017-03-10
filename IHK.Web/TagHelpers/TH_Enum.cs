using Microsoft.AspNetCore.Razor.TagHelpers;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace IHK.Web.TagHelpers
{
    [HtmlTargetElement("th_enum", Attributes = "th-label, th-value, th-type")]
    public class TH_Enum : TagHelper
    {
        [HtmlAttributeName("th-label")]
        public string Label { get; set; }

        [HtmlAttributeName("th-value")]
        public string Value { get; set; }

        [HtmlAttributeName("th-type")]
        public Type EnumType { get; set; }

        [HtmlAttributeName("th-id")]
        public string Id { get; set; }

        [HtmlAttributeName("th-col")]
        public int Col { get; set; } = 12;

        [HtmlAttributeName("th-isFC")]
        public bool IsFormControl { get; set; } = true;

        [HtmlAttributeName("th-multiple")]
        public bool Multiple { get; set; } = false;

        [HtmlAttributeName("th-withLabel")]
        public bool WithLabel { get; set; } = true;

        private string template = "";

        public override void Process(TagHelperContext context, TagHelperOutput output)
        {
            Value = TagHelperTools.FirstLetterToLower(Value);
            Id = Id ?? TagHelperTools.GetID();

            //Array OptionsArray = System.Enum.GetValues(EnumType);


            var enumVals = new List<object>();
            foreach (var item in Enum.GetValues(EnumType))
            {

                enumVals.Add(new
                {
                    id = (int)item,
                    name = item.ToString()
                });
            }

            string Options = JsonConvert.SerializeObject(enumVals);

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
                template += $"<select id='{Id}' class='form-control' data-bind='options: {Options}, selectedOptions: {Value}, optionsText: \"name\",optionsValue : \"id\",  multiselect: {{numberDisplayed: 5}}' multiple='multiple'></select>";
            }
            else
            {
                template += $"<select id='{Id}' class='selectpicker form-control show-tick input-sm' data-style='btn btn-default btn-sm'  data-bind='selectPicker: {Value},value:{Value}, optionsText: \"name\", optionsValue : \"id\", options: {Options}'></select>";
            }

            output.Content.SetHtmlContent(template);
        }
    }
}
