using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace GuardianRP.Service.SocketClient.Event {

    public class SocketStateEventArgs : EventArgs {

        public Socket   Source      { private set; get; }
        public bool     Connected   { private set; get; }

        public SocketStateEventArgs(Socket src, bool state) {
            Source = src;
            Connected = state;
        }

    }

}
