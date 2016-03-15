namespace Lucid.Web.Testing.Http
{
    public class NameValue
    {
        private string  _name;
        private string  _value;

        public NameValue(string name, string value)
        {
            _name = name;
            _value = value;
        }

        public string   Name    { get { return _name; } }
        public string   Value   { get { return _value; } }

        public string QueryValue()
        {
            return string.Format("{0}={1}", Name, Value);
        }
    }
}
