using System;

namespace Lucid.Web.Testing.Hosting
{
    public class ConsoleWriter : MarshalByRefObject
    {
        public void Write(object o)
        {
            Console.Write(o);
        }

        public void WriteLine(object o)
        {
            Console.WriteLine(o);
        }
    }
}
