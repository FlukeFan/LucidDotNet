using System;
using System.Linq.Expressions;
using Lucid.Web.Html;

namespace Lucid.Web.Testing.Html
{
    public static class TypedFormExtensions
    {
        public static string GetText<T>(this TypedForm<T> form, Expression<Func<T, string>> property)
        {
            var formName = InputHelpers.FormName(property);
            return form.GetSingle(formName).Value;
        }

        public static TypedForm<T> SetText<T>(this TypedForm<T> form, Expression<Func<T, string>> property, string value)
        {
            var formName = InputHelpers.FormName(property);
            var formValue = form.GetSingle(formName).SetValue(value);
            return form;
        }
    }
}
