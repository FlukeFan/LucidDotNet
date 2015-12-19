using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Web.Hosting;
using Demo.Domain.Utility;
using Demo.Infrastructure.NHibernate;
using Newtonsoft.Json;

namespace Demo.Web.Utility
{
    public class Startup
    {
        public virtual void InitRepository()
        {
            var connectionString = DevConnectionStringOverride() ?? WebConfigConnnectionString();

            // TODO verify NH not hitting DB for reserved words
            DemoNhRepository.Init(connectionString, typeof(DemoEntity));
        }

        private string WebConfigConnnectionString()
        {
            return ConfigurationManager.AppSettings["connectionString"];
        }

        private string DevConnectionStringOverride()
        {
            var webFolder = HostingEnvironment.MapPath("~");
            var configOverrideFile = Path.Combine(webFolder, @"..\BuildEnvironment.json");

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