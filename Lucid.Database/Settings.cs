using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Xml;

namespace Lucid.Database
{
    public static class Settings
    {
        private static IDictionary<string, string> _values;
        private static string _serialisedValues;

        public static void Init(string fileName, params object[] defaults)
        {
            LoadFromFile(fileName);

            foreach (var settings in defaults)
                UpdateValues(settings);

            SaveChanges(fileName);
        }

        private static void UpdateValues(object settings)
        {
            var type = settings.GetType();
            
            foreach (var field in type.GetFields(BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic))
            {
                var name = type.Name + "." + field.Name;

                if (!_values.ContainsKey(name))
                    _values.Add(name, (string)field.GetValue(settings));
                else
                    field.SetValue(settings, _values[name]);
            }
        }

        private static void LoadFromFile(string fileName)
        {
            _values = new Dictionary<string, string>();
            _serialisedValues = null;

            if (File.Exists(fileName))
            {
                var doc = new XmlDocument();
                doc.Load(fileName);

                foreach (XmlElement add in doc.SelectNodes("//add"))
                {
                    var key = add.Attributes["name"].Value;
                    var value = add.Attributes["value"].Value;
                    _values.Add(key, value);
                }

                _serialisedValues = doc.OuterXml;
            }
        }

        private static void SaveChanges(string fileName)
        {
            var doc = new XmlDocument();
            doc.LoadXml("<xml/>");
            var root = doc.DocumentElement;

            foreach (var key in _values.Keys.OrderBy(k => k))
            {
                var value = _values[key];
                var add = doc.CreateElement("add");
                add.SetAttribute("name", key);
                add.SetAttribute("value", value);
                root.AppendChild(add);
            }

            var serialisedValues = doc.OuterXml;

            if (_serialisedValues != serialisedValues)
            {
                doc.Save(fileName);
                _serialisedValues = serialisedValues;
            }
        }
    }
}
