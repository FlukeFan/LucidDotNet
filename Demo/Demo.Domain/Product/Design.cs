using Demo.Domain.Utility;

namespace Demo.Domain.Product
{
    public class Design : DemoEntity
    {
        protected Design() { }

        public virtual string Name { get; protected set; }
    }
}
