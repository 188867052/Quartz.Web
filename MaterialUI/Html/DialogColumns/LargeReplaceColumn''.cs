namespace MaterialUI.ViewConfiguration
{
    using MaterialUI.Javascript;
    using Microsoft.AspNetCore.Html;

    public class LargeReplaceColumn<TModel, TPostModel> : LargeColumn<TModel, TPostModel>
    {
        private delegate IHtmlContent RenderDelegate(TModel model);

        protected override string ColumnClassTemplate { get; } = "replace col-md-{0} col-md-offset-{1}";

        public LargeReplaceColumn(IColumn<TModel, TPostModel> textBox)
            : base(textBox)
        {
        }

        public LargeReplaceColumn(IColumn<TModel, TPostModel> left, IColumn<TModel, TPostModel> right)
              : base(left, right)
        {
        }

        public LargeReplaceColumn(IColumn<TModel, TPostModel> left, IColumn<TModel, TPostModel> middle, IColumn<TModel, TPostModel> right)
             : base(left, middle, right)
        {
        }
    }
}