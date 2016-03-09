using System;
using System.Collections.Generic;
using System.Linq;
using Lucid.Web.Testing.Http;

namespace Lucid.Web.Testing.Html
{
    public class ScrapedForm<T>
    {
        private string              _method;
        private string              _action;
        private ElementWrapper      _element;
        private IList<FormValue>    _formValues     = new List<FormValue>();
        private IList<SubmitValue>  _submitValues   = new List<SubmitValue>();

        public ScrapedForm(string method = "", string action = "")
        {
            SetMethod(method);
            SetAction(action);
        }

        public ScrapedForm(ElementWrapper element)
        {
            _element = element;
            SetMethod(_element.AttributeOrEmpty("method"));
            SetAction(_element.AttributeOrEmpty("action"));
            AddInputs();
        }

        public string                   Method          { get { return _method; } }
        public string                   Action          { get { return _action; } }
        public ElementWrapper           Element         { get { return _element; } }
        public IEnumerable<FormValue>   FormValues      { get { return _formValues; } }
        public IEnumerable<SubmitValue> SubmitValues    { get { return _submitValues; } }

        public ScrapedForm<T> SetMethod(string method)
        {
            _method = (method ?? "").ToLower() == "post" ? "post" : "get";
            return this;
        }

        public ScrapedForm<T> SetAction(string action)
        {
            _action = action ?? "";
            return this;
        }

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
            var formInputs = _element.FindAll("input, select, textarea");

            foreach (var formInput in formInputs)
                if (IsSubmit(formInput))
                    AddSubmit(formInput);
                else
                    AddInput(formInput);
        }

        private bool IsSubmit(ElementWrapper formInput)
        {
            return formInput.TagName.ToLower() == "input"
                && formInput.AttributeOrEmpty("type").ToLower() == "submit";
        }

        private void AddSubmit(ElementWrapper inputSubmit)
        {
            var submitValue = SubmitValue.FromElement(inputSubmit);
            _submitValues.Add(submitValue);
        }

        private void AddInput(ElementWrapper formInput)
        {
            if (!formInput.HasAttribute("name"))
                return;

            var formValue = FormValue.FromElement(formInput);
            _formValues.Add(formValue);
        }
    }
}
