
namespace Lucid.Database.Schema
{
    public static partial class Db
    {
        public static TableDesign Table_Design = new TableDesign();

        public class TableDesign
        {
            public readonly string Name = "Design";

            public readonly string Column_Id = "Id";
            public readonly string Column_Name = "Name";
        }
    }
}
