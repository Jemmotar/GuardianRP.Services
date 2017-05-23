using GuardianRP.Api.Server.Networking;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GuardianRP.Api.Server {

    class Api {

        public static readonly TcpServer Server = new TcpServer(25567);

        static void Main(string[] args) {
            Server.Start();
            
            while(true) {
                Server.Broadcast(Console.ReadLine());
            }
        }

    }

}
