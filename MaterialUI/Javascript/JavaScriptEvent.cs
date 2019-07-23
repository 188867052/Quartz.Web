namespace MaterialUI.Javascript
{
    using AspNetCore.Extensions;
    using MaterialUI.Html.Tags;
    using Microsoft.AspNetCore.Html;

    /// <summary>
    /// JavaScript used to initialize a view instance.
    /// </summary>
    public class JavaScriptEvent
    {
        private readonly JavaScriptEventEnum eventType;
        private readonly string selector;
        private readonly string func;

        public JavaScriptEvent(string func, string @class, JavaScriptEventEnum eventType = default)
            : this(eventType)
        {
            this.func = func;
            this.selector = $".{@class}";
        }

        public JavaScriptEvent(string func, Identifier id, JavaScriptEventEnum eventType = default)
            : this(eventType)
        {
            this.func = func;
            this.selector = $"#{id.Value}";
            this.Id = id;
        }

        private JavaScriptEvent(JavaScriptEventEnum eventType)
        {
            this.eventType = eventType;
        }

        public Identifier Id { get; set; }

        public string Render()
        {
            string js = $"$(\"{this.selector}\").on('{EnumMapping.ToString(this.eventType)}',function(){{{this.func}();}});";
            return $"<script>{js}</script>";
        }

        public IHtmlContent Script
        {
            get
            {
                string js = $"$(\"{this.selector}\").on('{EnumMapping.ToString(this.eventType)}',function(){{{this.func}();}});";
                return TagHelper.Create(Tag.script, js);
            }
        }
    }
}