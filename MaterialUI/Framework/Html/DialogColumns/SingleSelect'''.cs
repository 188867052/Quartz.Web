namespace MaterialUI.ViewConfiguration
{
    using System;
    using System.Linq;
    using System.Linq.Expressions;

    public class SingleSelect<TModel, TPostModel, TEnum> : SingleSelect<TModel, TPostModel>
        where TEnum : Enum
    {
        public SingleSelect(
            string placeholder,
            Expression<Func<TPostModel, object>> expression,
            Expression<Func<TModel, object>> modelExpression,
            Expression<Func<TEnum, bool>> filter = null)
          : base(placeholder, expression, modelExpression)
        {
            Array values = Enum.GetValues(typeof(TEnum));
            var a = values.AsQueryable().GetEnumerator();

            foreach (var v in values)
            {
                if (filter == null)
                {
                    this.AddOption(v, v.ToString());
                }
                else
                {
                    Func<TEnum, bool> b = filter.Compile();
                    if (b.Invoke((TEnum)v))
                    {
                        this.AddOption(v, v.ToString());
                    }
                }
            }
        }
    }
}