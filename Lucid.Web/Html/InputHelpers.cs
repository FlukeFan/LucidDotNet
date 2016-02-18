using System.Web.Mvc;

namespace Lucid.Web.Html
{
    public static class InputHelpers
    {
        public static Form<T> Form<T>(this HtmlHelper<T> helper)
        {
            return new Form<T>(helper);
        }

        public static Form<U> Form<T, U>(this HtmlHelper<T> helper, U newModel)
        {
            var newHelper = helper.NewHelper(newModel);
            return new Form<U>(newHelper);
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
    }
}
