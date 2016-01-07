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
                if (!client.HadExpectedError())
                    client.ConsoleWriter.WriteLine("Last response:\n\n" + SimulatedHttpClient.LastResponseText);

                throw;
            }

            if (client.HadExpectedError())
                throw new Exception("Expected error from HTTP client, but didn't get one");
        }

        public override object InitializeLifetimeService()
        {
            return null; // Tells .NET not to expire this remoting object
        }
    }
}
