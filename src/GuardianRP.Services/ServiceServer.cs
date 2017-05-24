using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GuardianRP.Services.Tcp;
using GuardianRP.Services.Web;

namespace GuardianRP.Services {

    class ServiceServer {

        public static Configuration Config = Configuration.Instance;

        public static readonly TcpSocketService TcpService = new TcpSocketService(Config.ScoketPort);
        public static readonly WebService       WebService = new WebService(Config.WebDirectory, Config.WebPort);

        static void Main(string[] args) {
            TcpService.Start();
            WebService.Start();

            while(true) {
                TcpService.Broadcast(Console.ReadLine());
            }
        }

    }

}
