using System;
using System.Collections.Generic;
using System.Linq;
using Lucid.Web.Testing.Http;

namespace Lucid.Web.Testing.Html
{
    public class ScrapedForm<T>
    {
        private ElementWrapper      _element;
        private IList<FormValue>    _formValues = new List<FormValue>();

        public ScrapedForm() { }

        public ScrapedForm(ElementWrapper element)
        {
            _element = element;
            AddInputs();
        }

        public ElementWrapper           Element     { get { return _element; } }
        public IEnumerable<FormValue>   FormValues  { get { return _formValues; } }

        public FormValue[] Get(string name)
        {
            return _formValues.Where(fv => fv.Name == name).ToArray();
        }

        public FormValue GetSingle(string name)
        {
            var formValues = Get(name);

            if (formValues.Length == 0)
                throw new Exception(string.Format("Could not find entry '{0}' in form values", name));

            if (formValues.Length > 1)
                throw new Exception(string.Format("Found multiple form values for '{0}'", name));

            return formValues.Single();
        }

        public ScrapedForm<T> SetFormValues(Request post)
        {
            foreach (var formValue in _formValues)
                formValue.SetFormValue(post);

            return this;
        }

        public object Post(SimulatedHttpClient client, string url)
        {
            return client.Post(url, r => SetFormValues(r));
        }

        private void AddInputs()
        {
            var inputs = _element.FindAll("input, select");

            foreach (var input in inputs)
            {
                if (!input.HasAttribute("name"))
                    continue;

                var formValue = FormValue.FromElement(input);
                _formValues.Add(formValue);
            }
        }
    }
}
