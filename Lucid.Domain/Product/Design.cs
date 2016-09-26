using Lucid.Domain.Contract.Product.Commands;
using Lucid.Domain.Utility;

namespace Lucid.Domain.Product
{
    public class Design : LucidEntity
    {
        protected Design() { }

        public virtual string Name { get; protected set; }

        public static Design Start(StartDesign cmd)
        {
            VerifyNoDesignWithName(cmd.Name);
            var design = new Design { Name = cmd.Name };
            return Repository.Save(design);
        }

        private static void VerifyNoDesignWithName(string name)
        {
            var designsWithName = Repository.Query<Design>()
                .Filter(d => d.Name == name)
                .List();

            if (designsWithName.Count > 0)
                throw new LucidException("Please specify a design name that is not already in use.");
        }
    }
}
