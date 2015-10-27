using Demo.Domain.Product.Commands;
using Demo.Domain.Utility;

namespace Demo.Domain.Product
{
    public class Design : DemoEntity
    {
        protected Design() { }

        public virtual string Name { get; protected set; }

        public static Design Start(StartDesign cmd)
        {
            var design = new Design { Name = cmd.Name };
            return Registry.Repository.Save(design);
        }
    }
}
