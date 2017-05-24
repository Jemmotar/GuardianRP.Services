using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GuardianRP.Service.SocketServer;

namespace GuardianRP.Services {

    class ServiceServer {

        public static readonly TcpSocketServer Server = new TcpSocketServer(25567);

        static void Main(string[] args) {
            Server.Start();
            
            while(true) {
                Server.Broadcast(Console.ReadLine());
            }
        }

    }

}
