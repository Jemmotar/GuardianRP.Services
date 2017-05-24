using GuardianRP.Services.FileSystem;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GuardianRP.Services {

    internal class Configuration : JsonFile {

        internal static readonly Configuration Instance = new Configuration();

        [JsonProperty]
        public string WebDirectory { private set; get; } = @"C:\Users\GuardianRP\Desktop\build\web";

        [JsonProperty]
        public int WebPort { private set; get; } = 80;

        [JsonProperty]
        public int ScoketPort { private set; get; } = 25567;

        private Configuration(string filename = "config.json") : base(filename) {
            if(Exists) Load(); else Save();
        }

    }

}
