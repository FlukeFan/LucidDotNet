using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using Lucid.Web.Testing.Http;

namespace Lucid.Web.Testing.Html
{
    public class ScrapedForm<T>
    {
        private ElementWrapper      _element;
        private IList<FormValue>    _formValues = new List<FormValue>();

        public ScrapedForm(ElementWrapper element)
        {
            _element = element;
            AddInputs();
        }

        public ElementWrapper           Element     { get { return _element; } }
        public IEnumerable<FormValue>   FormValues  { get { return _formValues; } }

        public string GetText(Expression<Func<T, string>> property)
        {
            var formName = FormName(property);
            return _formValues.Where(fv => fv.Name == formName).Single().Value;
        }

        public ScrapedForm<T> SetText(Expression<Func<T, string>> property, string value)
        {
            var formName = FormName(property);
            _formValues.Where(fv => fv.Name == formName).Single().Value = value;
            return this;
        }

        public ScrapedForm<T> SetFormValues(Request post)
        {
            foreach (var formValue in _formValues)
                post.SetFormValue(formValue.Name, formValue.Value);

            return this;
        }

        public object Post(SimulatedHttpClient client, string url)
        {
            return client.Post(url, r => SetFormValues(r));
        }

        private string FormName(LambdaExpression expression)
        {
            MemberExpression me = (MemberExpression)expression.Body;
            var formName = me.Member.Name;
            return formName;
        }

        private void AddInputs()
        {
            var inputs = _element.FindAll("input");

            foreach (var input in inputs)
            {
                if (!input.HasAttribute("name"))
                    continue;

                var formValue = new FormValue { Name = input.Attribute("name") };

                if (input.HasAttribute("value"))
                    formValue.Value = input.Attribute("value");

                _formValues.Add(formValue);
            }
        }
    }
}
