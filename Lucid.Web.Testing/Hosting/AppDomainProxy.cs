using System;
using Lucid.Web.Testing.Http;

namespace Lucid.Web.Testing.Hosting
{
    public class AppDomainProxy : MarshalByRefObject
    {
        public void RunCodeInAppDomain(Action codeToRun)
        {
            codeToRun();
        }

        public void RunCodeInAppDomain(SerializableDelegate<Action<SimulatedHttpClient>> codeToRun, SimulatedHttpClient client)
        {
            codeToRun.Delegate(client);
        }

        public override object InitializeLifetimeService()
        {
            return null; // Tells .NET not to expire this remoting object
        }
    }
}
