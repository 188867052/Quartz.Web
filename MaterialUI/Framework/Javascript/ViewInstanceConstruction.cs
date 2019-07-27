namespace Quartz.Javascript
{
    using Quartz.Shared;

    public abstract class ViewInstanceConstruction
    {
        protected abstract string InstanceClassName { get; }

        /// <summary>
        /// View instance.
        /// </summary>
        /// <returns>A JavaScriptInitialize.</returns>
        public JavaScriptInitialize ViewInstance()
        {
            Check.NotEmpty(this.InstanceClassName, nameof(this.InstanceClassName));

            char firstChar = this.InstanceClassName[0];
            string globalVariableName = char.ToLower(firstChar) + this.InstanceClassName.Substring(1);
            JavaScriptInitialize javaScriptInitialize = new JavaScriptInitialize(globalVariableName, this.InstanceClassName);
            this.InitializeViewInstance(javaScriptInitialize);
            return javaScriptInitialize;
        }

        public abstract void InitializeViewInstance(JavaScriptInitialize initialize);
    }
}