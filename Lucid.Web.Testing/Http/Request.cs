using System.Collections.Specialized;

namespace Lucid.Web.Testing.Http
{
    public class Request
    {
        private string              _url;
        private string              _query;
        private string              _verb;
        private NameValueCollection _headers    = new NameValueCollection();
        private NameValueCollection _formValues = new NameValueCollection();

        public Request(string url) : this(url, "GET") { }

        public Request(string url, string verb)
        {
            var urlParts = url.Split('?');
            _url = urlParts[0];
            _query = urlParts.Length > 1 ? urlParts[1] : null;

            _verb = verb;
        }

        public string               Url         { get { return _url; } }
        public string               Query       { get { return _query; } }
        public string               Verb        { get { return _verb; } }
        public NameValueCollection  Headers     { get { return _headers; } }
        public NameValueCollection  FormValues  { get { return _formValues; } }

        public Request SetFormUrlEncoded()
        {
            SetHeader("Content-Type", "application/x-www-form-urlencoded");
            return this;
        }

        public Request SetFormValue(string name, string value)
        {
            _formValues[name] = value;
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
