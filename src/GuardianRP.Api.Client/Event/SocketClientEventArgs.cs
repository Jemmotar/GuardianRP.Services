using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GuardianRP.Service.SocketClient.Event {

    public class SocketClientEventArgs : EventArgs {

        public SocketClient Client { private set; get; }

        public SocketClientEventArgs(SocketClient client) {
            Client = client;
        }

    }

}
