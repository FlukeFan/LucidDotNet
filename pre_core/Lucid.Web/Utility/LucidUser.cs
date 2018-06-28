using System.Security.Principal;
using Lucid.Domain.Contract.Account.Responses;

namespace Lucid.Web.Utility
{
    public class LucidUser : CookieAuthentication.IUser, IPrincipal
    {
        public int          Id          { get; protected set; }
        public string       Name        { get; protected set; }
        public IIdentity    Identity    { get; protected set; }

        public LucidUser(UserDto userDto) : this(userDto.UserId, userDto.Name)
        {
        }

        public LucidUser(int id, string name)
        {
            Id = id;
            Name = name;
            Identity = new GenericIdentity(Name);
        }

        public string CookieValue()
        {
            return string.Format("{0}|{1}", Id, Name);
        }

        public static LucidUser CreateFromCookieValue(string cookieValue)
        {
            var values = cookieValue.Split('|');

            if (values.Length != 2)
                return null;

            var id = int.Parse(values[0]);
            var name = values[1];

            return new LucidUser(id, name);
        }

        public bool IsInRole(string role)
        {
            return false;
        }
    }
}