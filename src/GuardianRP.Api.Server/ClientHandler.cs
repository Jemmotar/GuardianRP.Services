using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace GuardianRP.Api.Server {

    internal class ClientHandler {

        public readonly Socket Socket;

        public ClientHandler(Socket client) {
            Socket = client;
        }

    }

}
