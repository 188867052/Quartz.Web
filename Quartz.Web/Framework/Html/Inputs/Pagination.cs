namespace Quartz.Html.Inputs
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using AspNetCore.Extensions;
    using Microsoft.AspNetCore.Html;
    using Quartz.Html.Tags;

    public class Pagination
    {
        public int Total { get; private set; }

        public int Size { get; private set; }

        public IHtmlContent Render(int index, int size, int total)
        {
            this.Total = total;
            this.Size = size;
            int last = (int)Math.Ceiling(total / (float)size);
            List<int> list;
            if (index < 6)
            {
                list = this.Range(2, last - 1);
            }
            else
            {
                int min = Math.Min(index, last - 2);
                list = this.Range(min - 1, min + 1);
            }

            return this.Generate(list, index, last);
        }

        private List<int> Range(int left, int right)
        {
            var list = new List<int>();
            for (int i = left, j = 0; i <= right && j <= 4; i++, j++)
            {
                list.Add(i);
            }

            return list;
        }

        private IHtmlContent Generate(List<int> list, int current, int last)
        {
            var ul = TagHelper.Create(Tag.ul, new TagAttribute(Attr.Class, "pagination pagination-info"));
            ul.Content.AppendHtml(this.GetTag(1, current));
            if (list.Any() && list.First() - 1 > 1)
            {
                ul.Content.AppendHtml(this.GetTag("...", current));
            }

            foreach (var item in list)
            {
                var tag = TagHelper.Create(Tag.li, new AnchorHref(item).Html);
                if (item == current)
                {
                    tag.Attributes.Add(Attr.Class, "active");
                }

                ul.Content.AppendHtml(tag);
            }

            if (list.Any() && list.Last() + 1 < last)
            {
                ul.Content.AppendHtml(this.GetTag("...", current));
            }

            if (last != 1)
            {
                ul.Content.AppendHtml(this.GetTag(last, current));
            }

            ul.Content.AppendHtml(this.GetTag($"共 {this.Total} 条", current));
            ul.Content.AppendHtml(this.GetTag($"{this.Size} 条/页", current));
            return ul;
        }

        private IHtmlContent GetTag(object number, int index)
        {
            var tag = TagHelper.Create(Tag.li, new AnchorHref(number.ToString()).Html);
            if (number.ToString() == index.ToString())
            {
                tag.Attributes.Add(Attr.Class, "active");
            }

            return tag;
        }
    }
}
