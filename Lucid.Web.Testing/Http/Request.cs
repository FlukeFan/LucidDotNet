using System.Collections.Specialized;

namespace Lucid.Web.Testing.Http
{
    public class Request
    {
        public string               Url;
        public string               Query;
        public string               Verb;
        public NameValueCollection  Headers     = new NameValueCollection();
        public NameValueCollection  FormValues  = new NameValueCollection();

        public Request SetFormUrlEncoded()
        {
            Headers["Content-Type"] = "application/x-www-form-urlencoded";
            return this;
        }

        public Request SetFormValue(string name, string value)
        {
            FormValues[name] = value;
            return this;
        }

        public Request SetHeader(string name, string value)
        {
            Headers[name] = value;
            return this;
        }

        public Request AddQuery(string name, string value)
        {
            var prefix = string.IsNullOrEmpty(Query) ? "" : "&";
            Query += string.Format("{0}{1}={2}", prefix, name, value);

            return this;
        }
    }
}
