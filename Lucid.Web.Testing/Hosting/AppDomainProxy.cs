using System;
using Lucid.Web.Testing.Http;

namespace Lucid.Web.Testing.Hosting
{
    public class AppDomainProxy : MarshalByRefObject
    {
        public AppDomainProxy()
        {
            WebHost.IsRunningInTestHost = true;
        }

        public void RunCodeInAppDomain(Action codeToRun)
        {
            codeToRun();
        }

        public void RunCodeInAppDomain(SerializableDelegate<Action<SimulatedHttpClient>> codeToRun, SimulatedHttpClient client)
        {
            try
            {
                codeToRun.Delegate(client);
            }
            catch
            {
                client.ConsoleWriter.WriteLine("Last response:\n\n" + SimulatedHttpClient.LastResponseText);
                throw;
            }
        }

        public override object InitializeLifetimeService()
        {
            return null; // Tells .NET not to expire this remoting object
        }
    }
}
