using GuardianRP.Service.SocketClient.Event;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace GuardianRP.Service.SocketClient {

    public class SocketClient {

        public readonly     string  Ip;
        public readonly     int     Port;

        public bool Connected {
            get {
                return _connected;
            }
            set {
                if(_connected != value) {
                    _connected = value;
                    OnConnectionStateChanged(this, new SocketStateEventArgs(_socket, value));
                }
            }
        }

        private const       short   _BUFFER_SIZE    = 128;
        private readonly    byte[]  _buffer         = new byte[_BUFFER_SIZE];
        private readonly    Socket  _socket;
        private             bool    _connected      = false;
        private             bool    _receiving      = false;

        public  event       EventHandler<SocketStateEventArgs>      OnConnectionStateChanged = delegate { };
        public  event       EventHandler<SocketMessageEventArgs>    OnMessageReceived        = delegate { };

        public SocketClient(string ip, int port = 25567) {
            _socket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            _socket.SetSocketOption(SocketOptionLevel.Socket, SocketOptionName.KeepAlive, true);
            Ip = ip;
            Port = port;
        }

        public SocketClient(Socket socket) {
            _socket = socket;
            _socket.SetSocketOption(SocketOptionLevel.Socket, SocketOptionName.KeepAlive, true);

            IPEndPoint endpoint = _socket.RemoteEndPoint as IPEndPoint;
            Ip = endpoint.Address.ToString();
            Port = endpoint.Port;
            Connected = true;
        }

        public IAsyncResult Start() {
            if(!Connected)
                return _socket.BeginConnect(Ip, Port, new AsyncCallback(OnConnected), this);
            else if(!_receiving) {
                _receiving = true;
                BeginReceive();
            }

            return null;
        }

        public void Stop() {
            _socket.Close();
        }

        private void OnConnected(IAsyncResult result) {
            // At this potint, server connection was not sucessfull
            if(!_socket.Connected) {
                Connected = false;
                return;
            }

            // End connection procedure
            _socket.EndConnect(result);
            Connected = true;

            // Create a State object and begin listening for incomming messages
            BeginReceive();
        }

        private void BeginReceive() {
            _socket.BeginReceive(_buffer, 0, _buffer.Length, SocketFlags.None, new AsyncCallback(OnDataReceived), null);
        }

        private void OnDataReceived(IAsyncResult result) {
            int bytesRead = 0;

            try {
                // Try to read available message
                bytesRead = _socket.EndReceive(result);
            } catch (SocketException) {
                // At this point, server connection was lost
                Connected = false;
                return;
            }
 
            StringBuilder builder = new StringBuilder();

            // If there are data in the message
            if(bytesRead > 0) {

                // If the message is bigger then buffer, read it untill it is not
                while(bytesRead > _buffer.Length)
                    builder.Append(Encoding.UTF8.GetString(_buffer, 0, _buffer.Length));

                // Save the rest of the message
                builder.Append(Encoding.UTF8.GetString(_buffer, 0, bytesRead % _buffer.Length));

                // Dispach message event
                OnMessageReceived(this, new SocketMessageEventArgs(_socket, builder.ToString()));

            }

            // Start receiving again
            BeginReceive();
        }

        public void Send(byte[] payload) {
            SocketAsyncEventArgs e = new SocketAsyncEventArgs();
            e.SetBuffer(payload, 0, payload.Length);
            _socket.SendAsync(e);
        }

    }

}
