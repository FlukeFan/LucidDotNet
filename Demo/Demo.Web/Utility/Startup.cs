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
            var connectionString = ConfigurationManager.AppSettings["connectionString"];

            var webFolder = HostingEnvironment.MapPath("~");
            var configOverrideFile = Path.Combine(webFolder, @"..\BuildEnvironment.json");

            if (File.Exists(configOverrideFile))
            {
                var json = File.ReadAllText(configOverrideFile);
                var values = JsonConvert.DeserializeObject<IDictionary<string, string>>(json);

                var key = "DemoConnection";

                if (values.ContainsKey(key))
                    connectionString = values[key];
            }

            // TODO verify NH not hitting DB for reserved words
            DemoNhRepository.Init(connectionString, typeof(DemoEntity));
        }
    }
}