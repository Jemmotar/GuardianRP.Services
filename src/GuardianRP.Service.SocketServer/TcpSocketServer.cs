using Client = GuardianRP.Service.SocketClient.SocketClient;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;
using GuardianRP.Service.SocketClient.Event;

namespace GuardianRP.Service.SocketServer {

    public class TcpSocketServer : TcpListener {

        private readonly    List<Client>                        _clients = new List<Client>();

        public  readonly    int                                 Port;      
        public  readonly    IReadOnlyList<Client>               Clients;

        public  event       EventHandler<SocketClientEventArgs> OnClientConnected = delegate { };

        public TcpSocketServer(int port) : base(IPAddress.Any, port) {
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
            Client client = new Client(socket);
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
            foreach(Client client in Clients)
                client.Send(payload);
        }

    }

}
