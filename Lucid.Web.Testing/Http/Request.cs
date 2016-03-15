﻿using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Net;

namespace Lucid.Web.Testing.Http
{
    public class Request
    {
        public static readonly IDictionary<string, HttpStatusCode> DefaultStatusCodes = new Dictionary<string, HttpStatusCode>
        {
            { "GET",    HttpStatusCode.OK },
            { "POST",   HttpStatusCode.Redirect },
        };

        private string              _url;
        private string              _verb;
        private string              _query;
        private HttpStatusCode?     _expectedResponse;
        private NameValueCollection _headers            = new NameValueCollection();
        private IList<NameValue>    _formValues;

        public Request(string url, string verb = "GET")
        {
            var indexOfQuery = url.IndexOf('?');

            if (indexOfQuery < 0)
            {
                _url = url;
                _query = null;
            }
            else
            {
                _url = url.Substring(0, indexOfQuery);
                _query = url.Substring(indexOfQuery + 1);
            }

            _verb = verb.ToUpper();

            if (_verb == "POST")
                SetFormUrlEncoded();

            if (DefaultStatusCodes.ContainsKey(_verb))
                _expectedResponse = DefaultStatusCodes[_verb];
        }

        public string                   Url                 { get { return _url; } }
        public string                   Verb                { get { return _verb; } }
        public HttpStatusCode?          ExptectedResponse   { get { return _expectedResponse; } }
        public NameValueCollection      Headers             { get { return _headers; } }
        public IEnumerable<NameValue>   FormValues          { get { return _formValues; } }

        public string Query()
        {
            if (_verb == "POST" || _formValues == null)
                return _query;

            var queryValues = _formValues.Select(nv => nv.UrlQueryValue());
            return string.Join("&", queryValues);
        }

        public Request StartForm()
        {
            _formValues = new List<NameValue>();
            return this;
        }

        public Request SetExpectedResponse(HttpStatusCode? expectedResponseStatusCode)
        {
            _expectedResponse = expectedResponseStatusCode;
            return this;
        }

        public Request SetFormUrlEncoded()
        {
            SetHeader("Content-Type", "application/x-www-form-urlencoded");
            return this;
        }

        public Request AddFormValue(string name, string value)
        {
            return AddFormValue(new NameValue(name, value));
        }

        public Request AddFormValue(NameValue nameValue)
        {
            _formValues = _formValues ?? new List<NameValue>();
            _formValues.Add(nameValue);
            return this;
        }

        public Request SetHeader(string name, string value)
        {
            _headers[name] = value;
            return this;
        }
    }
}
