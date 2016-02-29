using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;

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
            MemberExpression me = (MemberExpression)property.Body;
            var propertyName = me.Member.Name;
            return _formValues.Where(fv => fv.Name == propertyName).Single().Value;
        }

        private void AddInputs()
        {
            var inputs = _element.FindAll("input");

            foreach (var input in inputs)
                _formValues.Add(new FormValue
                {
                    Name = input.Attribute("name"), // TODO - what if 'name' doesn't exist?
                    Value = input.Attribute("value") // TODO = what if 'value' doesn't exist?
                });
        }
    }
}
