using System.Web;

namespace Lucid.Web.Utility.Html
{
    public class Component<T> : IHtmlString
    {
        public T Content { get; protected set; }

        public Component(T content)
        {
            Content = content;
        }

        public string ToHtmlString()
        {
            return Content.ToString();
        }
    }

    public static class ComponentExtensions
    {
        public static Component<T> ToComponent<T>(this T content) { return new Component<T>(content); }
    }
}
