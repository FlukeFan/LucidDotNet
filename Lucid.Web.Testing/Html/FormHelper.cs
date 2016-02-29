﻿using System;
using System.Collections.Generic;

namespace Lucid.Web.Testing.Html
{
    public static class FormHelper
    {
        public static Form<T> Scrape<T>(DocumentWrapper doc)
        {
            return Scrape<T>(doc, "form");
        }

        public static Form<T> Scrape<T>(DocumentWrapper doc, int index)
        {
            var formElements = FindForms(doc, "form");

            if (index > formElements.Count - 1)
                throw new Exception(string.Format("Index '{0}' is too large for collection with '{1}' forms: {2}", index, formElements.Count, ElementWrapper.FormatTags(formElements)));

            return new Form<T>(formElements[index]);
        }

        public static Form<T> Scrape<T>(DocumentWrapper doc, string cssSelector)
        {
            var formElements = FindForms(doc, cssSelector);

            if (formElements.Count > 1)
                throw new Exception("Multiple form elements found in document: " + ElementWrapper.FormatTags(formElements));

            return new Form<T>(formElements[0]);
        }

        private static List<ElementWrapper> FindForms(DocumentWrapper doc, string cssSelector)
        {
            var formElements = doc.FindAll(cssSelector);

            if (formElements.Count == 0)
                throw new Exception(string.Format("CSS selector '{0}' did not match any elements in the document", cssSelector));

            return formElements;
        }
    }
}
