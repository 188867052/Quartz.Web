namespace MaterialKit.Html.Filters
{
    using System.Collections.Generic;
    using AspNetCore.Extensions;
    using Javascript;
    using MaterialKit.ViewConfiguration;
    using Microsoft.AspNetCore.Html;

    public abstract class FilterBase<TModel, TPostModel> : IDialogConfiguration
    {
        public IHtmlContent Render()
        {
            IList<IHtmlContent> contents = new List<IHtmlContent>();
            var columns = new List<LargeColumn<TModel, TPostModel>>();
            this.CreateColumns(columns);
            TModel model = default;
            foreach (var item in columns)
            {
                contents.Add(item.Render(model));
            }

            return TagHelper.Combine(contents);
        }

        protected abstract void CreateColumns(IList<LargeColumn<TModel, TPostModel>> columns);
    }
}