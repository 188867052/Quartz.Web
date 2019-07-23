namespace MaterialKit.Javascript
{
    using System.Collections.Generic;
    using System.Text;

    /// <summary>
    /// JavaScript used to initialize a view instance.
    /// </summary>
    public class JavaScriptInitialize
    {
        private readonly Dictionary<string, string> fields;
        private readonly string globalVariableName;
        private readonly string className;

        /// <summary>
        /// Initializes a new instance of the <see cref="JavaScriptInitialize"/> class.
        /// </summary>
        /// <param name="globalVariableName">The global variable name.</param>
        /// <param name="className">The class name.</param>
        public JavaScriptInitialize(string globalVariableName, string className)
        {
            this.globalVariableName = globalVariableName;
            this.className = className;
            this.fields = new Dictionary<string, string>();
        }

        public void AddStringInstance(string key, string value)
        {
            this.fields.Add(key, value);
        }

        public void AddUrlInstance(string key, string url)
        {
            this.AddStringInstance(key, url);
        }

        public void AddFrameWorkInstance(string key, Identifier identifier)
        {
            this.AddStringInstance(key, identifier.Value);
        }

        public string Render()
        {
            StringBuilder stringBuilder = new StringBuilder();
            foreach (var (key, value) in this.fields)
            {
                stringBuilder.Append($"{this.globalVariableName}._{key}=\"{value}\";");
            }

            string initializeCall = string.Concat(this.globalVariableName, ".initialize");
            string initializeScript = $"if({initializeCall} instanceof Function){{{initializeCall}();}}";

            return $"window.{this.globalVariableName} = new {this.className}();{stringBuilder}{initializeScript}";
        }
    }
}