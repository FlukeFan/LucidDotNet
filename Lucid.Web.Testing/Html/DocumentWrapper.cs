﻿using AngleSharp.Dom.Html;

namespace Lucid.Web.Testing.Html
{
    public class DocumentWrapper : ParentNodeWrapper
    {
        private IHtmlDocument _document;

        public DocumentWrapper(IHtmlDocument document) : base(document)
        {
            _document = document;
        }

        public IHtmlDocument Document { get { return _document; } }

        public ScrapedForm<T>   Form<T>()                   { return FormHelper.Scrape<T>(this); }
        public ScrapedForm<T>   Form<T>(int index)          { return FormHelper.Scrape<T>(this, index); }
        public ScrapedForm<T>   Form<T>(string cssSelector) { return FormHelper.Scrape<T>(this, cssSelector); }
    }
}
