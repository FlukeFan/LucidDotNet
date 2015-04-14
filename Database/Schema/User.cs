
namespace LucidDotNet.Database.Schema
{
    public static partial class Db
    {
        public static TableUser Table_User = new TableUser();

        public class TableUser
        {
            public readonly string Name         = "User";

            public readonly string Column_Id    = "Id";
            public readonly string Column_Email = "Email";
        }
    }
}
