using System;
using System.Collections.Generic;
using System.Linq;
using Lucid.Web.Testing.Http;

namespace Lucid.Web.Testing.Html
{
    public class TypedForm<T>
    {
        protected   string              _method;
        protected   string              _action;
        protected   IList<FormValue>    _formValues     = new List<FormValue>();
        protected   IList<SubmitValue>  _submitValues   = new List<SubmitValue>();

        public TypedForm(string method = "", string action = "")
        {
            SetMethod(method);
            SetAction(action);
        }

        public string                   Method          { get { return _method; } }
        public string                   Action          { get { return _action; } }
        public IEnumerable<FormValue>   FormValues      { get { return _formValues; } }
        public IEnumerable<SubmitValue> SubmitValues    { get { return _submitValues; } }

        public TypedForm<T> SetMethod(string method)
        {
            _method = (method ?? "").ToLower() == "post" ? "post" : "get";
            return this;
        }

        public TypedForm<T> SetAction(string action)
        {
            _action = action ?? "";
            return this;
        }

        public TypedForm<T> AddFormValue(FormValue formValue)
        {
            _formValues.Add(formValue);
            return this;
        }

        public TypedForm<T> AddSubmitValue(SubmitValue submitValue)
        {
            _submitValues.Add(submitValue);
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

        public TypedForm<T> SetFormValues(Request request)
        {
            foreach (var formValue in _formValues)
                formValue.SetFormValue(request);

            return this;
        }

        public Response Submit(ISimulatedHttpClient client, Action<Request> modifier = null)
        {
            if (_submitValues.Count == 0)
                throw new Exception("No submit inputs found");

            if (_submitValues.Count > 1)
                throw new Exception("Found multiple submit inputs: " + string.Join(", ", _submitValues.Select(sv => string.Format("({0}={1})", sv.Name, sv.Value))));

            var submit = _submitValues[0];
            return Submit(submit, client, modifier);
        }

        public Response Submit(SubmitValue submit, ISimulatedHttpClient client, Action<Request> modifier = null)
        {
            var request = new Request(Action, Method);
            SetFormValues(request);

            if (submit != null)
                submit.SetFormValue(request);

            return client.Process(request, modifier);
        }
    }
}
