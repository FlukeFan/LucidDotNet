using System;
using System.Linq.Expressions;
using System.Web.Mvc;
using HtmlTags;

namespace Demo.Web.Utility.Html
{
    public static class InputHelpers
    {
        public static Form<T> Form<T>(this HtmlHelper<T> helper, Action<FormTag> mutator = null)
        {
            return new Form<T>(helper, mutator);
        }

        public static Form<U> Form<T, U>(this HtmlHelper<T> helper, U newModel, Action<FormTag> mutator = null)
        {
            var newHelper = helper.NewHelper(newModel);
            return new Form<U>(newHelper, mutator);
        }

        public static HtmlHelper<U> NewHelper<T, U>(this HtmlHelper<T> helper, U newModel)
        {
            var tmpViewData = new ViewDataDictionary(helper.ViewData);
            tmpViewData.Model = newModel;
            var viewData = new ViewDataDictionary<U>(tmpViewData);
            var data = new ViewDataContainer { ViewData = viewData };
            var newHelper = new HtmlHelper<U>(helper.ViewContext, data);
            return newHelper;
        }

        private class ViewDataContainer : IViewDataContainer
        {
            public ViewDataDictionary ViewData { get; set; }
        }

        public static string FormName(LambdaExpression property)
        {
            return ExpressionHelper.GetExpressionText(property);
        }
    }
}
