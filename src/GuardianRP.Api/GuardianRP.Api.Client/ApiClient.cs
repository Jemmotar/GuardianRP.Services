using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace GuardianRP.Api.Client {

    public class StateObject {
 
        public Socket Socket = null;

        public const int BufferSize = 256;

        public byte[] Buffer = new byte[BufferSize];

        public readonly StringBuilder Builder = new StringBuilder();

        public void Clear() {
            Array.Clear(Buffer, 0, BufferSize);
            Builder.Clear();
        }

    }

    public class ApiMessageEventArgs : EventArgs {

        public Socket Source    { private set; get; }
        public string Message   { private set; get; }

        public ApiMessageEventArgs(Socket src, string msg) {
            Source = src;
            Message = msg;
        }

    }

    public class ApiClient {

        public readonly Socket  Client = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
        public readonly string  Ip;
        public readonly int     Port;

        public event EventHandler<ApiMessageEventArgs> OnApiMessageReceived;

        public ApiClient(string ip, int port = 25567) {
            Ip = ip;
            Port = port;
        }

        public void Start() {
            Client.BeginConnect(Ip, Port, new AsyncCallback(OnConnected), this);
        }

        private void OnConnected(IAsyncResult result) {
            // At this potint, server connection was not sucessfull
            if(!Client.Connected)
                return;

            // End connection procedure
            Client.EndConnect(result);

            // Create a State object and begin listening for incomming messages
            StateObject state = new StateObject();
            state.Socket = Client;
            BeginReceive(state);
        }

        private void BeginReceive(StateObject state) {
            Client.BeginReceive(state.Buffer, 0, state.Buffer.Length, SocketFlags.None, new AsyncCallback(OnMessageReceived), state);
        }

        private void OnMessageReceived(IAsyncResult result) {
            // At this potint, server connection was lost
            if(!Client.Connected)
                return;

            StateObject state = (StateObject)result.AsyncState;
            int bytesRead = Client.EndReceive(result);

            // If the message is bigger then buffer, read it untill it is not
            while(bytesRead > state.Buffer.Length)
                state.Builder.Append(Encoding.UTF8.GetString(state.Buffer, 0, state.Buffer.Length));
            // Save the rest of the message
            state.Builder.Append(Encoding.UTF8.GetString(state.Buffer, 0, bytesRead % state.Buffer.Length));
            
            // Dispach message event
            OnApiMessageReceived(this, new ApiMessageEventArgs(state.Socket, state.Builder.ToString()));

            // Clear StateObject
            state.Clear();

            BeginReceive(state);
        }

    }

}
