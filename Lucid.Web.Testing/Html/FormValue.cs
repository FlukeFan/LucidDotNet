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
        private IList<string>   _confinedValues;
        private IList<string>   _texts;

        public static FormValue FromElement(ElementWrapper element)
        {
            var formValue = new FormValue(element.Attribute("name"));
            formValue.SetDisabled(element.HasAttribute("disabled"));

            switch (element.TagName.ToLower())
            {
                case "input":
                    return FromInput(element, formValue);

                case "textarea":
                    return FromTextArea(element, formValue);

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

        public static FormValue FromTextArea(ElementWrapper textArea, FormValue formValue)
        {
            formValue
                .SetValue(textArea.TextContent)
                .SetReadonly(textArea.HasAttribute("readonly"));

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

            formValue.SetConfinedValues(values, texts);

            return formValue;
        }

        public FormValue(string name, string value = "", bool read_only = false, bool disabled = false, bool send = true, IList<string> confinedValues = null, IList<string> texts = null)
        {
            _name = name;
            _value = value;
            _readonly = read_only;
            _disabled = disabled;
            _send = send;
            SetConfinedValues(confinedValues, texts);
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

            if (_confinedValues.Count > 0 && !_confinedValues.Contains(value))
                throw new Exception(string.Format("Value '{0}' cannot be set.  Must be one of '{1}'", value, string.Join(", ", _confinedValues)));

            return ForceValue(value);
        }

        public FormValue ForceValue(string value)
        {
            _value = value;
            return this;
        }

        public FormValue SelectText(string text)
        {
            var textIndex = _texts.IndexOf(text);

            if (textIndex < 0)
                throw new Exception(string.Format("Could not find '{0}' in '{1}'", text, string.Join(", ", _texts)));

            return SetValue(_confinedValues[textIndex]);
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

        public FormValue SetConfinedValues(IList<string> confinedValues, IList<string> texts = null)
        {
            _confinedValues = confinedValues ?? new List<string>();
            _texts = texts ?? new List<string>();

            if (_texts.Count > 0 && _texts.Count != _confinedValues.Count)
                throw new Exception(string.Format("Supplied texts '{0}' does not match length of supplied values '{1}'", string.Join(", ", _confinedValues), string.Join(", ", _texts)));

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