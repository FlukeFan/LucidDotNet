using System;
using Lucid.Web.Testing.Http;

namespace Lucid.Web.Testing.Html
{
    public class FormValue
    {
        private string  _name;
        private string  _value;
        private bool    _readonly;
        private bool    _disabled;
        private bool    _sendEmpty;

        public static FormValue FromElement(ElementWrapper input)
        {
            var formValue = new FormValue(input.Attribute("name"));

            formValue
                .SetValue(input.HasAttribute("value") ? input.Attribute("value") : null)
                .SetReadonly(input.HasAttribute("readonly"))
                .SetDisabled(input.HasAttribute("disabled"))
                .SetSendEmpty(input.Attribute("type") != "checkbox");

            return formValue;
        }

        public FormValue(string name, string value = "", bool read_only = false, bool disabled = false, bool sendEmpty = true)
        {
            _name = name;
            _value = value;
            _readonly = read_only;
            _disabled = disabled;
            _sendEmpty = sendEmpty;
        }

        public string   Name        { get { return _name; } }
        public string   Value       { get { return _value; } }
        public bool     Readonly    { get { return _readonly; } }
        public bool     Disabled    { get { return _disabled; } }
        public bool     SendEmpty   { get { return _sendEmpty; } }

        public FormValue SetName(string name)
        {
            _name = name;
            return this;
        }

        public FormValue SetValue(string value)
        {
            if (_readonly && value != _value)
                throw new Exception(string.Format("Cannot change readonly input '{0}'", _name));

            _value = value;
            return this;
        }

        public FormValue SetReadonly(bool read_only)
        {
            _readonly = read_only;
            return this;
        }

        public FormValue SetDisabled(bool disabled)
        {
            _disabled = disabled;
            return this;
        }

        public FormValue SetSendEmpty(bool sendEmpty)
        {
            _sendEmpty = sendEmpty;
            return this;
        }

        public void SetFormValue(Request request)
        {
            if (_disabled)
                return;

            if (string.IsNullOrEmpty(_value) && !_sendEmpty)
                return;

            request.SetFormValue(_name, _value);
        }
    }
}