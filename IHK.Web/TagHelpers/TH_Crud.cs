using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Html;
using Microsoft.AspNetCore.Razor.TagHelpers;

namespace IHK.Web.TagHelpers
{
    public class TH_CrudContext
    {
        public IHtmlContent Left { get; set; }
        public IHtmlContent Right { get; set; }
    }

    [HtmlTargetElement("th_crud_left", ParentTag = "th_crud")]
    public class TH_Crud_Left : TagHelper
    {
        public override async Task ProcessAsync(TagHelperContext context, TagHelperOutput output)
        {
            var childContent = await output.GetChildContentAsync();
            var modalContext = (TH_CrudContext)context.Items[typeof(TH_Crud)];
            modalContext.Left = childContent;
            output.SuppressOutput();
        }
    }

    [HtmlTargetElement("th_crud_right", ParentTag = "th_crud")]
    public class TH_Crud_Right : TagHelper
    {
        public override async Task ProcessAsync(TagHelperContext context, TagHelperOutput output)
        {
            var childContent = await output.GetChildContentAsync();
            var modalContext = (TH_CrudContext)context.Items[typeof(TH_Crud)];
            modalContext.Right = childContent;
            output.SuppressOutput();
        }
    }

    [RestrictChildren("th_crud_left", "th_crud_right")]
    public class TH_Crud : TagHelper
    {
        [HtmlAttributeName("th-add-class")]
        public string AddClass { get; set; }
        [HtmlAttributeName("th-save")]
        public string Save { get; set; } = "onClickSave";

        [HtmlAttributeName("th-insert")]
        public string Insert { get; set; } = "onClickInsert";

        [HtmlAttributeName("th-edit")]
        public string Edit { get; set; } = "onClickEdit";

        [HtmlAttributeName("th-del")]
        public string Del { get; set; } = "onClickDelete";

        [HtmlAttributeName("th-bind-disable")]
        public string DelDisable { get; set; }

        [HtmlAttributeName("th-show-save")]
        public bool IsSave { get; set; } = true;

        [HtmlAttributeName("th-show-insert")]
        public bool IsInsert { get; set; } = true;

        [HtmlAttributeName("th-show-edit")]
        public bool IsEdit { get; set; } = true;

        [HtmlAttributeName("th-show-del")]
        public bool IsDel { get; set; } = true;

        [HtmlAttributeName("th-col")]
        public int Col { get; set; } = 12;

        [HtmlAttributeName("th-isFC")]
        public bool IsFormControl { get; set; } = true;

        [HtmlAttributeName("th-pull-right")]
        public bool PullRight { get; set; } = true;

        [HtmlAttributeName("th-id")]
        public string Id { get; set; }

        private string template = "";

        public override async Task ProcessAsync(TagHelperContext context, TagHelperOutput output)
        {
            var modalContext = new TH_CrudContext();
            context.Items.Add(typeof(TH_Crud), modalContext);
            await output.GetChildContentAsync();

            template = "<div class='form-group-sm col-md-12'>";
            template += $"<label class='control-label'>&nbsp;</label>";
            template += "</div>";

            output.Content.AppendHtml(template);


            if (IsFormControl)
            {
                if (PullRight)
                {
                    template = $"<div class='form-group-sm pull-right'>";
                }
                else
                {
                    template = $"<div class='form-group-sm'>";
                }
                output.Content.AppendHtml(template);
            }

            if (modalContext.Left != null)
            {
                output.Content.AppendHtml(modalContext.Left);
            }

            template = IsSave ? $"<button style='margin-left:10px;' class='btn btn-warning btn-sm' data-bind='click: {Save},disable: window.isLoading'>Speichern</button>" : "";
            template += IsInsert ? $"<button style='margin-left:10px;' class='btn btn-info btn-sm' data-bind='click: {Insert},disable: window.isLoading'>Einfügen</button>" : "";
            template += IsEdit ? $"<button style='margin-left:10px;' class='btn btn-warning btn-sm' data-bind='click: {Edit},disable: window.isLoading'>Bearbeiten</button>" : "";

            if (string.IsNullOrEmpty(DelDisable))
            {
                template += IsDel ? $"<button style='margin-left:10px;' class='btn btn-danger btn-sm' data-bind='click: {Del},disable: window.isLoading'>Löschen</button>" : "";
            }
            else
            {
                template += IsDel ? $"<button style='margin-left:10px;' class='btn btn-danger btn-sm' data-bind='click: {Del},disable: ({DelDisable} || window.isLoading())'>Löschen</button>" : "";
            }
            

            output.Content.AppendHtml(template);

            if (modalContext.Right != null)
            {
                output.Content.AppendHtml(modalContext.Right);
            }


            if (IsFormControl)
            {
                //output.TagName = "div";
                //output.Attributes.Add("class", $"form-group-sm col-md-{Col} " + AddClass);
                //output.TagMode = TagMode.StartTagAndEndTag;
                template = "</div>";
                output.Content.AppendHtml(template);
            }
            //template += "</div>";

            //output.Content.SetHtmlContent(template);

        }
    }
}
