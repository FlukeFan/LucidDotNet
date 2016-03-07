using System;
using System.Collections.Generic;
using Lucid.Web.Testing.Http;

namespace Lucid.Web.Testing.Html
{
    public class FormValue
    {
        private string          _name;
        private string          _value;
        private bool            _readonly;
        private bool            _disabled;
        private bool            _send;
        private IList<string>   _confinedValues = new List<string>();
        private IList<string>   _texts          = new List<string>();

        public static FormValue FromElement(ElementWrapper element)
        {
            var formValue = new FormValue(element.Attribute("name"));
            formValue.SetDisabled(element.HasAttribute("disabled"));

            switch (element.TagName.ToLower())
            {
                case "input":
                    return FromInput(element, formValue);

                case "select":
                    return FromSelect(element, formValue);

                default:
                    throw new Exception("Unhandled tag: " + element.TagName);
            }
        }

        public static FormValue FromInput(ElementWrapper input, FormValue formValue)
        {
            var type = input.HasAttribute("type") ? input.Attribute("type").ToLower() : "text";

            formValue
                .SetValue(input.HasAttribute("value") ? input.Attribute("value") : null)
                .SetReadonly(input.HasAttribute("readonly"));

            if (type == "checkbox" || type == "radio")
            {
                if (string.IsNullOrEmpty(formValue.Value))
                    formValue.SetValue("on");

                if (!input.HasAttribute("checked"))
                    formValue.SetSend(false);
            }

            return formValue;
        }

        public static FormValue FromSelect(ElementWrapper select, FormValue formValue)
        {
            var options = select.FindAll("option");
            var texts = new List<string>();
            var values = new List<string>();

            foreach (var option in options)
            {
                var text = option.TextContent;

                var value = option.HasAttribute("value")
                    ? option.Attribute("value")
                    : text;

                texts.Add(text);
                values.Add(value);

                if (option.HasAttribute("selected"))
                    formValue.SetValue(value);
            }

            formValue
                .SetTexts(texts)
                .SetConfinedValues(values);

            return formValue;
        }

        public FormValue(string name, string value = "", bool read_only = false, bool disabled = false, bool send = true)
        {
            _name = name;
            _value = value;
            _readonly = read_only;
            _disabled = disabled;
            _send = send;
        }

        public string           Name            { get { return _name; } }
        public string           Value           { get { return _value; } }
        public bool             Readonly        { get { return _readonly; } }
        public bool             Disabled        { get { return _disabled; } }
        public bool             Send            { get { return _send; } }
        public IList<string>    ConfinedValues  { get { return _confinedValues; } }
        public IList<string>    Texts           { get { return _texts; } }

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

        public FormValue SetSend(bool send)
        {
            _send = send;
            return this;
        }

        public FormValue SetConfinedValues(IList<string> confinedValues)
        {
            _confinedValues = confinedValues;
            return this;
        }

        public FormValue SetTexts(IList<string> texts)
        {
            _texts = texts;
            return this;
        }

        public void SetFormValue(Request request)
        {
            if (_disabled || !_send)
                return;

            request.SetFormValue(_name, _value);
        }
    }
}