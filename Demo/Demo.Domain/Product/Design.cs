using Demo.Domain.Utility;

namespace Demo.Domain.Product
{
    public class Design : DemoEntity
    {
        protected Design() { }

        public string Name { get; protected set; }
    }
}
