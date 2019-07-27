namespace MaterialUI.Html.Inputs
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using AspNetCore.Extensions;
    using MaterialUI.Html.Tags;
    using Microsoft.AspNetCore.Html;

    public class Pagination
    {
        public int Total { get; set; }
        public string ToHtml(int index, int size, int total)
        {
            Total = total;
            List<int> list;
            int last = (int)Math.Ceiling(total / (float)size);
            switch (index)
            {
                case 1:
                    switch (last)
                    {
                        case 1:
                        case 2:
                            list = new List<int> { };
                            break;
                        case 3:
                            list = new List<int> { 2 };
                            break;
                        case 4:
                            list = new List<int> { 2, 3 };
                            break;
                        case 5:
                            list = new List<int> { 2, 3, 4, };
                            break;
                        default:
                            list = new List<int> { 2, 3, 4, 5, };
                            break;
                    }

                    break;
                case 2:
                    switch (last)
                    {
                        case 2:
                            list = new List<int> { };
                            break;
                        case 3:
                            list = new List<int> { 2, };
                            break;
                        case 4:
                            list = new List<int> { 2, 3, };
                            break;
                        case 5:
                            list = new List<int> { 2, 3, 4, };
                            break;
                        default:
                            list = new List<int> { 2, 3, 4, 5, };
                            break;
                    }

                    break;
                case 3:
                    switch (last)
                    {
                        case 3:
                            list = new List<int> { 2, };
                            break;
                        case 4:
                            list = new List<int> { 2, 3, };
                            break;
                        case 5:
                            list = new List<int> { 2, 3, 4, };
                            break;
                        default:
                            list = new List<int> { 2, 3, 4, 5, };
                            break;
                    }

                    break;
                case 4:
                    switch (last)
                    {
                        case 4:
                            list = new List<int> { 2, 3, };
                            break;
                        case 5:
                            list = new List<int> { 2, 3, 4, };
                            break;
                        case 6:
                            list = new List<int> { 2, 3, 4, 5, };
                            break;
                        default:
                            list = new List<int> { 2, 3, 4, 5, 6, };
                            break;
                    }

                    break;
                case 5:
                    switch (last)
                    {
                        case 5:
                            list = new List<int> { 2, 3, 4, };
                            break;
                        case 6:
                            list = new List<int> { 2, 3, 4, 5, };
                            break;
                        case 7:
                            list = new List<int> { 2, 3, 4, 5, 6, };
                            break;
                        default:
                            list = new List<int> { 2, 3, 4, 5, 6, 7, };
                            break;
                    }

                    break;
                default:
                    int min = Math.Min(index, last - 2);
                    list = new List<int> { min - 1, min, min + 1 };
                    break;
            }

            return this.Generate(list, index, last);
        }

        private string Generate(List<int> list, int current, int last)
        {
            if (list.Any())
            {
                if (!list.Contains(current) && current != 1 && last != current)
                {
                    throw new Exception("current 必须在list中 1,last除外");
                }

                if (list.Contains(1))
                {
                    throw new Exception("1 不能在list中");
                }

                if (list.Contains(last))
                {
                    throw new Exception("last 不能在list中");
                }
            }

            var ul = TagHelper.Create(Tag.ul, new TagAttribute(Attr.Class, "pagination pagination-info"));
            ul.Content.AppendHtml(this.GetTag(1, current));
            if (list.Any() && list.First() - 1 > 1)
            {
                ul.Content.AppendHtml(TagHelper.Create(Tag.li, new AnchorHref("...").Html));
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
                ul.Content.AppendHtml(TagHelper.Create(Tag.li, new AnchorHref("...").Html));
            }

            if (last != 1)
            {
                ul.Content.AppendHtml(this.GetTag(last, current));
            }

            ul.Content.AppendHtml(TagHelper.Create(Tag.li, new AnchorHref($"共 {Total} 条").Html));
            return TagHelper.ToHtml(ul);
        }

        private IHtmlContent GetTag(int number, int index)
        {
            var tag = TagHelper.Create(Tag.li, new AnchorHref(number).Html);
            if (number == index)
            {
                tag.Attributes.Add(Attr.Class, "active");
            }

            return tag;
        }
    }
}
