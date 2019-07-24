namespace MaterialUI.SearchFilterConfigurations
{
    using System.Collections.Generic;
    using MaterialUI.ViewConfiguration;

    public abstract class SearchFilterConfigurationBase<TModel, TPostModel>
    {
        protected abstract void CreateSearchFilter(IList<LargeColumn<TModel, TPostModel>> filters);

        public void Render(IList<LargeColumn<TModel, TPostModel>> filters)
        {
            this.CreateSearchFilter(filters);
        }
    }
}
