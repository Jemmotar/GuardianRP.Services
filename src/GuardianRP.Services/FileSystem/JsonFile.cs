using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GuardianRP.Services.FileSystem {

    [JsonObject(MemberSerialization.OptIn)]
    public class JsonFile {

        public bool Exists {
            get {
                return _file.Exists;
            }
        }

        public  readonly    string      Location;
        public  readonly    string      FileName;

        private readonly    FileInfo    _file;

        public JsonFile(string path) {
            Location = path;
            FileName = Path.GetFileName(Location);
            _file = new FileInfo(Location);
        }

        public void Load() {
            if(Exists)
                using(StreamReader reader = _file.OpenText())
                    JsonConvert.PopulateObject(reader.ReadToEnd(), this);
        }

        public void Save() {
            using(StreamWriter writer = _file.CreateText())
                writer.Write(JsonConvert.SerializeObject(this, Formatting.Indented));
        }

        public void Delete() {
            if(Exists)
                _file.Delete();
        }

    }

}
