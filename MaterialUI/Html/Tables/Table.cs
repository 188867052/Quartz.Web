namespace MaterialKit.Html.Tables
{
    using AspNetCore.Extensions;
    using MaterialKit.Html.Icons;
    using MaterialKit.Html.Tags;

    public class Table
    {
        public string ToHtml()
        {
            var th1 = TagHelper.Create(Tag.th, new TagAttribute(Attr.Class, "text-center"), "#");
            var th2 = TagHelper.Create(Tag.th, "Name");
            var th3 = TagHelper.Create(Tag.th, "Job Position");
            var th4 = TagHelper.Create(Tag.th, "Since");
            var th5 = TagHelper.Create(Tag.th, new TagAttribute(Attr.Class, "text-right"), "Salary");
            var th6 = TagHelper.Create(Tag.th, new TagAttribute(Attr.Class, "text-right"), "Actions");

            var tr1 = TagHelper.Create(Tag.tr, th1, th2, th3, th4, th5, th6);
            var td1 = TagHelper.Create(Tag.td, new TagAttribute(Attr.Class, "text-center"), "1");
            var td2 = TagHelper.Create(Tag.td, "Andrew Mike");
            var td3 = TagHelper.Create(Tag.td, "Develop");
            var td4 = TagHelper.Create(Tag.td, "2013");
            var td5 = TagHelper.Create(Tag.td, new TagAttribute(Attr.Class, "text-right"), "&euro; 99,225");

            TagAttributeList attributes1 = new TagAttributeList()
            {
                new TagAttribute(Attr.Type, "button"),
                new TagAttribute(Attr.Rel, "tooltip"),
                new TagAttribute(Attr.Class, "btn btn-info btn-round"),
            };

            TagAttributeList attributes2 = new TagAttributeList()
            {
                new TagAttribute(Attr.Type, "button"),
                new TagAttribute(Attr.Rel, "tooltip"),
                new TagAttribute(Attr.Class, "btn btn-success btn-simple"),
            };

            TagAttributeList attributes3 = new TagAttributeList()
            {
                new TagAttribute(Attr.Type, "button"),
                new TagAttribute(Attr.Rel, "tooltip"),
                new TagAttribute(Attr.Class, "btn btn-danger"),
            };

            var button1 = TagHelper.Create(Tag.button, attributes1, new MaterialIcon("person").Html);
            button1.PostElement.Append(" ");
            var button2 = TagHelper.Create(Tag.button, attributes2, new MaterialIcon("edit").Html);
            button2.PostElement.Append(" ");
            var button3 = TagHelper.Create(Tag.button, attributes3, new MaterialIcon("close").Html);
            var td6 = TagHelper.Create(Tag.td, new TagAttribute(Attr.Class, "td-actions text-right"), button1, button2, button3);

            var tr2 = TagHelper.Create(Tag.tr, td1, td2, td3, td4, td5, td6);

            var thead = TagHelper.Create(Tag.thead, tr1);
            var tbody = TagHelper.Create(Tag.tbody, tr2);
            var table = TagHelper.Create(Tag.table, new TagAttribute(Attr.Class, "table"), thead, tbody);
            var div = TagHelper.Create(Tag.div, new TagAttribute(Attr.Class, "table-responsive"), table);

            return TagHelper.ToHtml(div);
        }
    }
}
