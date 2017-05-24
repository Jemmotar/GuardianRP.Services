using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Unosquare.Labs.EmbedIO;
using Unosquare.Labs.EmbedIO.Modules;

namespace GuardianRP.Services.Web {

    public class WebService : WebServer {

        public WebService(string path, int port = 80) : base(port) {
            RegisterModule(new StaticFilesModule(path));
            Module<StaticFilesModule>().UseRamCache = true;
            Module<StaticFilesModule>().DefaultExtension = ".html";
        }

        public void Start() {
            RunAsync();
        }

    }
}
