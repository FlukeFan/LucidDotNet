namespace Lucid.Web.Testing.Html
{
    public class FormScraper
    {
        protected ElementWrapper    _element;

        public FormScraper(ElementWrapper element)
        {
            _element = element;
        }

        public virtual TypedForm<T> Scrape<T>()
        {
            var method = _element.AttributeOrEmpty("method");
            var action = _element.AttributeOrEmpty("action");
            var form = new TypedForm<T>(method, action);
            AddInputs(form);
            return form;
        }

        protected virtual void AddInputs<T>(TypedForm<T> form)
        {
            var formInputs = _element.FindAll("input, select, textarea");

            foreach (var formInput in formInputs)
                if (IsSubmit(formInput))
                    AddSubmit(form, formInput);
                else
                    AddInput(form, formInput);
        }

        protected virtual bool IsSubmit(ElementWrapper formInput)
        {
            return formInput.TagName.ToLower() == "input"
                && formInput.AttributeOrEmpty("type").ToLower() == "submit";
        }

        protected virtual void AddSubmit<T>(TypedForm<T> form, ElementWrapper inputSubmit)
        {
            var submitValue = SubmitValue.FromElement(inputSubmit);
            form.AddSubmitValue(submitValue);
        }

        protected virtual void AddInput<T>(TypedForm<T> form, ElementWrapper formInput)
        {
            if (!formInput.HasAttribute("name"))
                return;

            var formValue = FormValueScraper.FromElement(formInput);
            form.AddFormValue(formValue);
        }
    }
}
