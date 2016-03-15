using System.Collections.Generic;
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
        private HttpStatusCode?     _expectedResponse;
        private NameValueCollection _headers            = new NameValueCollection();
        private IList<NameValue>    _queryValues        = new List<NameValue>();
        private IList<NameValue>    _formValues         = new List<NameValue>();

        public Request(string url, string verb = "GET")
        {
            var indexOfQuery = url.IndexOf('?');

            if (indexOfQuery < 0)
            {
                _url = url;
            }
            else
            {
                _url = url.Substring(0, indexOfQuery);
                _queryValues = SplitQuery(url.Substring(indexOfQuery + 1));
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
        public IEnumerable<NameValue>   QueryValues         { get { return _queryValues; } }
        public IEnumerable<NameValue>   FormValues          { get { return _formValues; } }

        public string Query()
        {
            var queryValues = _queryValues.Select(nv => nv.QueryValue());
            return string.Join("&", queryValues);
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
            _formValues.Add(nameValue);
            return this;
        }

        public Request SetHeader(string name, string value)
        {
            _headers[name] = value;
            return this;
        }

        public Request AddQueryValue(string name, string value)
        {
            return AddQueryValue(new NameValue(name, value));
        }

        public Request AddQueryValue(NameValue nameValue)
        {
            _queryValues.Add(nameValue);
            return this;
        }

        private static IList<NameValue> SplitQuery(string query)
        {
            var queryValues = new List<NameValue>();

            var values = query.Split('&');
            foreach (var value in values)
            {
                if (string.IsNullOrEmpty(value))
                    continue;

                var parts = value.Split('=');
                var namePart = parts[0];
                var valuePart = parts.Length > 1 ? parts[1] : "";
                var nameValue = new NameValue(namePart, valuePart);
                queryValues.Add(nameValue);
            }

            return queryValues;
        }
    }
}
