namespace MaterialKit.Html.Dialog
{
    using AspNetCore.Extensions;
    using MaterialKit.Html.Icons;
    using MaterialKit.Html.Tags;
    using MaterialKit.Routes;

    public class ConfirmDeleteDialog
    {
        private readonly string text;
        private readonly int id;

        public ConfirmDeleteDialog(string text, int id)
        {
            this.text = text;
            this.id = id;
        }

        public string ToHtml()
        {
            TagAttributeList attributes = new TagAttributeList()
            {
                new TagAttribute(Attr.Type, "button"),
                new TagAttribute(Attr.Class, "close"),
                new TagAttribute(Attr.DataDismiss, "modal"),
                new TagAttribute(Attr.AriaHidden, "true"),
            };
            var button = TagHelper.Create(Tag.button, attributes, new MaterialIcon("clear").Html);
            var header = TagHelper.Div("modal-header", button);

            var h5 = TagHelper.Create(Tag.h5, this.text);
            var body = TagHelper.Div("modal-body text-center", h5);

            var attributes1 = new TagAttributeList()
            {
                new TagAttribute(Attr.Type, "button"),
                new TagAttribute(Attr.Class, "btn btn-simple"),
                new TagAttribute(Attr.DataDismiss, "modal"),
            };
            var attributes2 = new TagAttributeList()
            {
                new TagAttribute(Attr.Type, "button"),
                new TagAttribute(Attr.Class, "btn btn-success btn-simple"),
                new TagAttribute(Attr.Id, this.id.ToString()),
                new TagAttribute(Attr.Action, ScheduleRoute.Delete),
            };
            var button1 = TagHelper.Create(Tag.button, attributes1, "取消");
            var button2 = TagHelper.Create(Tag.button, attributes2, "确定");
            var footer = TagHelper.Div("modal-footer text-center", button1, button2);

            var content = TagHelper.Div("modal-content", header, body, footer);
            var dialog = TagHelper.Div("modal-dialog modal-small", content);

            TagAttributeList attributes3 = new TagAttributeList()
            {
                new TagAttribute(Attr.Class, "modal fade"),
                new TagAttribute(Attr.Id, "smallAlertModal"),
                new TagAttribute(Attr.TabIndex, "-1"),
                new TagAttribute(Attr.Role, "dialog"),
                new TagAttribute(Attr.AriaLabelledBy, "myModalLabel"),
                new TagAttribute(Attr.AriaHidden, "true"),
            };

            var dialog1 = TagHelper.Create(Tag.div, attributes3, dialog);
            return TagHelper.ToHtml(dialog1);
        }
    }
}
