using System.Collections.Generic;
using System.Collections.Specialized;
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
        private string              _query;
        private string              _verb;
        private HttpStatusCode?     _expectedResponse;
        private NameValueCollection _headers            = new NameValueCollection();
        private IList<NameValue>    _formValues         = new List<NameValue>();

        public Request(string url, string verb = "GET")
        {
            var urlParts = url.Split('?');
            _url = urlParts[0];
            _query = urlParts.Length > 1 ? urlParts[1] : null;

            _verb = verb.ToUpper();

            if (_verb == "POST")
                SetFormUrlEncoded();

            if (DefaultStatusCodes.ContainsKey(_verb))
                _expectedResponse = DefaultStatusCodes[_verb];
        }

        public string                   Url                 { get { return _url; } }
        public string                   Query               { get { return _query; } }
        public string                   Verb                { get { return _verb; } }
        public HttpStatusCode?          ExptectedResponse   { get { return _expectedResponse; } }
        public NameValueCollection      Headers             { get { return _headers; } }
        public IEnumerable<NameValue>   FormValues          { get { return _formValues; } }

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

        public Request AddQuery(string name, string value)
        {
            var prefix = string.IsNullOrEmpty(_query) ? "" : "&";
            _query += string.Format("{0}{1}={2}", prefix, name, value);

            return this;
        }
    }
}
