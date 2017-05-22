using GuardianRP.Api.Server;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace GuardianRP.Api {

    internal class TcpServer : TcpListener {

        private readonly List<ClientHandler>            _clients = new List<ClientHandler>();

        public  readonly int                            Port;      
        public  readonly IReadOnlyList<ClientHandler>   Clients;

        public TcpServer(int port) : base(IPAddress.Any, port) {
            Port = port;
            Clients = _clients.AsReadOnly();
        }

        public new void Start() {
            base.Start();
            BeginAcceptingClients();
            Console.WriteLine($"Started listening for incomming TCP connections on port {Port}.");
        }

        private void BeginAcceptingClients() {
            BeginAcceptSocket(new AsyncCallback(OnClientConnected), this);
        }

        private void OnClientConnected(IAsyncResult result) {
            Console.WriteLine("Incomming TCP connection detected...");
            Socket socket = EndAcceptSocket(result);
            Console.WriteLine($"Client from {((IPEndPoint)socket.RemoteEndPoint).Address.ToString()} was accepted.");
            _clients.Add(new ClientHandler(socket));
            BeginAcceptingClients();
        }

        public void Broadcast(string message) {
            // TODO: Do this in ClientHandler
            byte[] buffer = Encoding.UTF8.GetBytes(message);

            foreach(ClientHandler client in Clients) {
                SocketAsyncEventArgs e = new SocketAsyncEventArgs();
                e.SetBuffer(buffer, 0, buffer.Length);
                client.Socket.SendAsync(e);
            }
        }

    }

}
