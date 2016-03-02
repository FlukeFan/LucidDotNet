using System;
using System.Linq.Expressions;

namespace Lucid.Web.Testing.Html
{
    public static class ScrapedFormExtensions
    {
        public static string GetText<T>(this ScrapedForm<T> form, Expression<Func<T, string>> property)
        {
            var formName = FormName(property);
            return form.Get(formName).Value;
        }

        public static ScrapedForm<T> SetText<T>(this ScrapedForm<T> form, Expression<Func<T, string>> property, string value)
        {
            var formName = FormName(property);
            var formValue = form.Get(formName).Value = value;
            return form;
        }

        private static string FormName(LambdaExpression expression)
        {
            MemberExpression me = (MemberExpression)expression.Body;
            var formName = me.Member.Name;
            return formName;
        }
    }
}
