namespace MaterialUI.Javascript
{
    using MaterialUI.Shared;

    public class MethodCall
    {
        public MethodCall(string method)
        {
            Check.NotEmpty(method, nameof(method));

            this.Method = method;
            this.Id = new Identifier();
        }

        public string Method { get; }

        public Identifier Id { get; }
    }
}