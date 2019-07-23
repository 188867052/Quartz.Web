namespace MaterialKit.ViewConfiguration
{
    using AspNetCore.Extensions;
    using MaterialKit.Javascript;
    using Microsoft.AspNetCore.Html;

    public class LargeColumn<TModel, TPostModel>
    {
        private readonly RenderDelegate render;
        private readonly IColumn<TModel, TPostModel> textBox;
        private readonly IColumn<TModel, TPostModel> left;
        private readonly IColumn<TModel, TPostModel> middle;
        private readonly IColumn<TModel, TPostModel> right;

        private delegate IHtmlContent RenderDelegate(TModel model);

        public LargeColumn(IColumn<TModel, TPostModel> textBox)
        {
            this.textBox = textBox;
            this.render = this.Large;
        }

        public LargeColumn(IColumn<TModel, TPostModel> left, IColumn<TModel, TPostModel> right)
        {
            this.left = left;
            this.right = right;
            this.render = this.Middle;
        }

        public LargeColumn(IColumn<TModel, TPostModel> left, IColumn<TModel, TPostModel> middle, IColumn<TModel, TPostModel> right)
        {
            this.left = left;
            this.middle = middle;
            this.right = right;
            this.render = this.Small;
        }

        protected virtual string ColumnClassTemplate { get; } = "col-md-{0} col-md-offset-{1}";

        public IHtmlContent Render(TModel model)
        {
            return this.render(model);
        }

        public bool IsFilter { get; set; }

        private IHtmlContent Small(TModel mode)
        {
            // 5 , 2, 3
            var div1 = TagHelper.Div("card-content", this.left.Render(mode));
            var div2 = TagHelper.Div(this.GetClass(5, 1), div1);

            var div3 = TagHelper.Div("card-content", this.middle.Render(mode));
            var div4 = TagHelper.Div(this.GetClass(2, 0), div3);

            var div5 = TagHelper.Div("card-content", this.right.Render(mode));
            var div6 = TagHelper.Div(this.GetClass(3, 0), div5);

            return TagHelper.Content(div2, div4, div6);
        }

        private IHtmlContent Middle(TModel mode)
        {
            var div1 = TagHelper.Div("card-content", this.left.Render(mode));
            int width = (int)this.left.Width;
            var div2 = TagHelper.Div(this.GetClass(width, 1), div1);

            var div3 = TagHelper.Div("card-content", this.right.Render(mode));
            var div4 = TagHelper.Div(this.GetClass((int)this.right.Width, 5 - width), div3);

            return TagHelper.Content(div2, div4);
        }

        private IHtmlContent Large(TModel mode)
        {
            var div1 = TagHelper.Div("card-content", this.textBox.Render(mode));
            var div2 = TagHelper.Div(this.IsFilter ? this.GetClass(2, 0) : this.GetClass(10, 1), div1);

            return div2;
        }

        private string GetClass(int left, int right)
        {
            return string.Format(this.ColumnClassTemplate, left, right);
        }
    }
}