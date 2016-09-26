using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Web.Hosting;
using Lucid.Infrastructure;
using Lucid.Infrastructure.NHibernate;
using Newtonsoft.Json;
using SimpleFacade.Execution;
using SimpleFacade.Validation;

namespace Lucid.Web.Utility
{
    public class Startup
    {
        public virtual void Init()
        {
            InitExecutor();
            InitRepository();
        }

        private void InitExecutor()
        {
            PresentationRegistry.Executor =
                new CqExecutor(
                    new RepositoryExecutor(
                        new ValidatingExecutor(
                            new DemoExecutor()
                        )
                    )
                );
        }

        private void InitRepository()
        {
            var connectionString = DevConnectionStringOverride() ?? WebConfigConnnectionString();

            DemoStartup.Init(connectionString);
        }

        private string WebConfigConnnectionString()
        {
            return ConfigurationManager.AppSettings["SQLSERVER_CONNECTION_STRING"];
        }

        private string DevConnectionStringOverride()
        {
            var webFolder = HostingEnvironment.MapPath("~");
            var configOverrideFile = Path.Combine(webFolder, @"..\_items\BuildEnvironment.json");

            if (!File.Exists(configOverrideFile))
                return null;

            var json = File.ReadAllText(configOverrideFile);
            var values = JsonConvert.DeserializeObject<IDictionary<string, string>>(json);

            var key = "DemoConnection";

            if (!values.ContainsKey(key))
                return null;

            return values[key];
        }
    }
}