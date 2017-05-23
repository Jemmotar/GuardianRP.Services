using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GuardianRP.Api.Client.Networking.Event {

    public class SocketClientEventArgs : EventArgs {

        public SocketClient Client { private set; get; }

        public SocketClientEventArgs(SocketClient client) {
            Client = client;
        }

    }

}
