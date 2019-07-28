namespace Quartz.ViewConfiguration
{
    using AspNetCore.Extensions;
    using Microsoft.AspNetCore.Html;
    using Quartz.Javascript;

    public class ButtonColumn<TModel, TPostModel>
    {
        private readonly RenderDelegate render;
        private readonly IColumn<TModel, TPostModel> textBox;
        private readonly IColumn<TModel, TPostModel> left;
        private readonly IColumn<TModel, TPostModel> right;

        private delegate IHtmlContent RenderDelegate(TModel model);

        public ButtonColumn(IColumn<TModel, TPostModel> textBox)
        {
            this.textBox = textBox;
            this.render = this.Large;
        }

        public ButtonColumn(IColumn<TModel, TPostModel> left, IColumn<TModel, TPostModel> right)
        {
            this.left = left;
            this.right = right;
            this.render = this.Small;
        }

        public IHtmlContent Render(TModel model)
        {
            return this.render(model);
        }

        private IHtmlContent Small(TModel mode)
        {
            var div3 = TagHelper.Div("col-md-5 col-md-offset-1", this.left.Render(mode));
            var div6 = TagHelper.Div("col-md-5", this.right.Render(mode));

            return TagHelper.Content(div3, div6);
        }

        private IHtmlContent Large(TModel mode)
        {
            return TagHelper.Div("col-md-10 col-md-offset-1", this.textBox.Render(mode));
        }
    }
}