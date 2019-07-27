namespace AspNetCore.Extensions
{
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;
    using System.Text;
    using System.Text.Encodings.Web;
    using System.Threading.Tasks;
    using MaterialUI.Shared;
    using Microsoft.AspNetCore.Html;
    using Microsoft.AspNetCore.Razor.TagHelpers;

    public static class TagHelper
    {
        public static string ToHtml(params IHtmlContent[] content)
        {
            Check.NotEmpty(content, nameof(content));

            StringBuilder sb = new StringBuilder();
            foreach (var item in content)
            {
                using (var writer = new StringWriter())
                {
                    item.WriteTo(writer, HtmlEncoder.Default);
                    sb.Append(writer.ToString());
                }
            }

            return sb.ToString();
        }

        public static string ToHtml(IEnumerable<IHtmlContent> content)
        {
            return ToHtml(content.ToArray());
        }

        public static TagHelperOutput Create(string tagName, TagAttributeList attributes)
        {
            Check.NotEmpty(tagName, nameof(tagName));
            Check.NotEmpty(attributes, nameof(attributes));

            return new TagHelperOutput(tagName,
                                       attributes,
                                       getChildContentAsync: (useCachedResult, encoder) => Task.FromResult<TagHelperContent>(new DefaultTagHelperContent()));
        }

        public static TagHelperOutput SelfClosingTag(string tagName, TagAttributeList attributes)
        {
            var tag = Create(tagName, attributes);
            tag.TagMode = TagMode.SelfClosing;
            return tag;
        }

        public static TagHelperOutput Create(string tagName)
        {
            Check.NotEmpty(tagName, nameof(tagName));

            return new TagHelperOutput(tagName,
                                       new TagHelperAttributeList(),
                                       getChildContentAsync: (useCachedResult, encoder) => Task.FromResult<TagHelperContent>(new DefaultTagHelperContent()));
        }

        public static TagHelperOutput Script(string script)
        {
            Check.NotEmpty(script, nameof(script));

            return Create("script", script);
        }

        public static TagHelperOutput StartOnlyTag(string tagName)
        {
            var tag = Create(tagName);
            tag.TagMode = TagMode.StartTagOnly;

            return tag;
        }

        public static IHtmlContent Empty => Create("div").Content;

        public static TagHelperOutput SelfClosingTag(string tagName)
        {
            var tag = Create(tagName);
            tag.TagMode = TagMode.SelfClosing;

            return tag;
        }

        public static TagHelperOutput Create(string tagName, TagAttributeList attributes, params string[] content)
        {
            Check.NotEmpty(tagName, nameof(tagName));
            Check.NotEmpty(attributes, nameof(attributes));

            TagHelperOutput tagHelperOutput = new TagHelperOutput(tagName,
                                                                  attributes,
                                                                  getChildContentAsync: (useCachedResult, encoder) => Task.FromResult<TagHelperContent>(new DefaultTagHelperContent()));
            return tagHelperOutput.AppendHtml(content);
        }

        public static TagHelperOutput Create(string tagName, TagAttributeList attributes, IEnumerable<IHtmlContent> content)
        {
            return Create(tagName, attributes, content.ToArray());
        }

        public static TagHelperOutput Create(string tagName, TagAttribute attribute, IList<IHtmlContent> content)
        {
            return Create(tagName, attribute, content.ToArray());
        }

        public static TagHelperOutput Create(string tagName, TagAttributeList attributes, IList<TagHelperOutput> content)
        {
            return Create(tagName, attributes, content.Select(o => (IHtmlContent)o));
        }

        public static TagHelperOutput Create(string tagName, TagAttribute attribute, string content)
        {
            Check.NotEmpty(tagName, nameof(tagName));
            Check.NotNull(attribute, nameof(attribute));

            TagHelperOutput tagHelperOutput = new TagHelperOutput(tagName,
                                                                  new TagHelperAttributeList { attribute },
                                                                  getChildContentAsync: (useCachedResult, encoder) => Task.FromResult<TagHelperContent>(new DefaultTagHelperContent()));
            return tagHelperOutput.AppendHtml(content);
        }

        public static TagHelperOutput Create(string tagName, string content)
        {
            Check.NotEmpty(tagName, nameof(tagName));
            Check.NotEmpty(content, nameof(content));

            TagHelperOutput tagHelperOutput = new TagHelperOutput(tagName,
                                                                  new TagHelperAttributeList(),
                                                                  getChildContentAsync: (useCachedResult, encoder) => Task.FromResult<TagHelperContent>(new DefaultTagHelperContent()));
            return tagHelperOutput.AppendHtml(content);
        }

        public static TagHelperOutput Create(string tagName, TagAttributeList attributes, params IHtmlContent[] htmlContent)
        {
            Check.NotEmpty(tagName, nameof(tagName));
            Check.NotEmpty(attributes, nameof(attributes));

            TagHelperOutput tagHelperOutput = new TagHelperOutput(tagName,
                                                                  attributes,
                                                                  getChildContentAsync: (useCachedResult, encoder) => Task.FromResult<TagHelperContent>(new DefaultTagHelperContent()));
            return tagHelperOutput.AppendHtml(htmlContent);
        }

        public static TagHelperOutput Create(string tagName, TagAttribute attribute, params IHtmlContent[] htmlContent)
        {
            Check.NotEmpty(tagName, nameof(tagName));
            Check.NotNull(attribute, nameof(attribute));

            TagHelperOutput tagHelperOutput = new TagHelperOutput(tagName,
                                                                  new TagHelperAttributeList { attribute },
                                                                  getChildContentAsync: (useCachedResult, encoder) => Task.FromResult<TagHelperContent>(new DefaultTagHelperContent()));
            return tagHelperOutput.AppendHtml(htmlContent);
        }

        public static TagHelperOutput Div(string className, params IHtmlContent[] htmlContent)
        {
            Check.NotNull(className, nameof(className));

            return Create("div", new TagAttribute("class", className), htmlContent);
        }

        public static TagHelperOutput Div(string className)
        {
            Check.NotNull(className, nameof(className));

            return Create("div", new TagAttribute("class", className));
        }

        public static TagHelperOutput Div(string className, params string[] htmlContent)
        {
            Check.NotNull(className, nameof(className));

            return Create("div", new TagAttributeList() { { "class", className } }, htmlContent);
        }

        public static TagHelperOutput Create(string tagName, params IHtmlContent[] htmlContent)
        {
            Check.NotEmpty(tagName, nameof(tagName));

            TagHelperOutput tagHelperOutput = new TagHelperOutput(tagName,
                                                                  new TagHelperAttributeList { },
                                                                  getChildContentAsync: (useCachedResult, encoder) => Task.FromResult<TagHelperContent>(new DefaultTagHelperContent()));
            return tagHelperOutput.AppendHtml(htmlContent);
        }

        public static IHtmlContent Content(params IHtmlContent[] htmlContent)
        {
            TagHelperOutput tagHelperOutput = new TagHelperOutput("div",
                                                                  new TagHelperAttributeList { },
                                                                  getChildContentAsync: (useCachedResult, encoder) => Task.FromResult<TagHelperContent>(new DefaultTagHelperContent()));

            foreach (var item in htmlContent)
            {
                tagHelperOutput.Content.AppendHtml(item);
            }

            return tagHelperOutput.Content;
        }

        public static IHtmlContent Combine(IEnumerable<IHtmlContent> htmlContent)
        {
            return Content(htmlContent.ToArray());
        }

        public static IHtmlContent Combine(params IHtmlContent[] content)
        {
            return Content(content);
        }

        public static IHtmlContent Combine(params string[] content)
        {
            TagHelperOutput tagHelperOutput = new TagHelperOutput("div",
                                                                  new TagHelperAttributeList { },
                                                                  getChildContentAsync: (useCachedResult, encoder) => Task.FromResult<TagHelperContent>(new DefaultTagHelperContent()));

            foreach (var item in content)
            {
                tagHelperOutput.Content.AppendHtml(item);
            }

            return tagHelperOutput.Content;
        }

        private static TagHelperOutput AppendHtml(this TagHelperOutput tagHelperOutput, params IHtmlContent[] htmlContent)
        {
            foreach (var item in htmlContent)
            {
                tagHelperOutput.Content.AppendHtml(item);
            }

            return tagHelperOutput;
        }

        private static TagHelperOutput AppendHtml(this TagHelperOutput tagHelperOutput, params string[] content)
        {
            Check.NotNull(tagHelperOutput, nameof(tagHelperOutput));

            foreach (var item in content)
            {
                tagHelperOutput.Content.AppendHtml(item);
            }

            return tagHelperOutput;
        }
    }
}