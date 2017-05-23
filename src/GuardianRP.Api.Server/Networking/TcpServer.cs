using GuardianRP.Api.Client.Networking;
using GuardianRP.Api.Client.Networking.Event;
using GuardianRP.Api.Server;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace GuardianRP.Api.Server.Networking {

    internal class TcpServer : TcpListener {

        private readonly    List<SocketClient>                  _clients = new List<SocketClient>();

        public  readonly    int                                 Port;      
        public  readonly    IReadOnlyList<SocketClient>         Clients;

        public  event       EventHandler<SocketClientEventArgs> OnClientConnected = delegate { };

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
            BeginAcceptSocket(new AsyncCallback(OnClientAccepted), this);
        }

        private void OnClientAccepted(IAsyncResult result) {
            Socket socket = EndAcceptSocket(result);
            SocketClient client = new SocketClient(socket);
            client.Start();
            _clients.Add(client);

            client.OnConnectionStateChanged += (sender, args) => {
                if(args.Connected)
                    return;

                Console.WriteLine($"Client from {client.Ip} has disconnected.");
                client.Stop();
                _clients.Remove(client);
            };

            client.OnMessageReceived += (sender, args) => {
                Console.WriteLine($"[{client.Ip}] {args.Message}");
            };

            Console.WriteLine($"Client from {client.Ip} was accepted.");
            OnClientConnected(this, new SocketClientEventArgs(client));

            BeginAcceptingClients();
        }

        public void Broadcast(string message) {
            byte[] payload = Encoding.UTF8.GetBytes(message);
            foreach(SocketClient client in Clients)
                client.Send(payload);
        }

    }

}
