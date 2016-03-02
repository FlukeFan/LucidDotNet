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

        public static FormValue FromElement(ElementWrapper input)
        {
            var formValue = new FormValue(input.Attribute("name"));

            formValue
                .SetValue(input.HasAttribute("value") ? input.Attribute("value") : null)
                .SetReadonly(input.HasAttribute("readonly"))
                .SetDisabled(input.HasAttribute("disabled"));

            return formValue;
        }

        public FormValue(string name, string value = "", bool read_only = false, bool disabled = false)
        {
            _name = name;
            _value = value;
            _readonly = read_only;
            _disabled = disabled;
        }

        public string   Name        { get { return _name; } }
        public string   Value       { get { return _value; } }
        public bool     Readonly    { get { return _readonly; } }
        public bool     Disabled    { get { return _disabled; } }

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

        public void SetFormValue(Request post)
        {
            if (!_disabled)
                post.SetFormValue(_name, _value);
        }
    }
}