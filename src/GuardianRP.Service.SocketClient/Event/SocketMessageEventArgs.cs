using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace GuardianRP.Service.SocketClient.Event {

    public class SocketMessageEventArgs : EventArgs {

        public Socket   Source  { private set; get; }
        public string   Message { private set; get; }

        public SocketMessageEventArgs(Socket src, string msg) {
            Source = src;
            Message = msg;
        }

    }

}
