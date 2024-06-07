using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;
using WPFServer.Models;

namespace WPFServer
{
    public class TcpServer
    {
        private readonly ServerViewModel _viewModel;
        private List<(UserModel user, Socket socket)> clients = new List<(UserModel, Socket)>();
        private Socket socket;

        public TcpServer(ServerViewModel viewModel)
        {
            _viewModel = viewModel;
        }

        public void Start()
        {
            IPEndPoint ipPoint = new IPEndPoint(IPAddress.Any, 8888);
            socket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            socket.Bind(ipPoint);
            socket.Listen(100);
            ListenToClients();
        }

        private async void ListenToClients()
        {
            while (true)
            {
                var client = await socket.AcceptAsync();
                string userName = await ReceiveUserName(client);
                var user = new UserModel { UserName = userName };
                clients.Add((user, client));
                _viewModel.AddUser(userName);
                _viewModel.AddMessage("Система", $"{userName} присоединился к чату.");
                ReceiveMessage(client);
            }
        }

        private async Task<string> ReceiveUserName(Socket client)
        {
            byte[] bytes = new byte[1024];
            await client.ReceiveAsync(new ArraySegment<byte>(bytes), SocketFlags.None);
            string userName = Encoding.UTF8.GetString(bytes).Trim('\0');
            return userName;
        }

        private async void ReceiveMessage(Socket client)
        {
            while (true)
            {
                byte[] bytes = new byte[1024];
                try
                {
                    await client.ReceiveAsync(new ArraySegment<byte>(bytes), SocketFlags.None);
                }
                catch
                {
                    var disconnectedUser = clients.Find(x => x.socket == client).user.UserName;
                    clients.Remove(clients.Find(x => x.socket == client));
                    _viewModel.RemoveUser(disconnectedUser);
                    _viewModel.AddMessage("Система", $"{disconnectedUser} покинул чат.");
                    client.Dispose();
                    break;
                }
                string message = Encoding.UTF8.GetString(bytes).Trim('\0');
                var userName = clients.Find(x => x.socket == client).user.UserName;
                _viewModel.AddMessage(userName, message);

                foreach (var item in clients)
                {
                    SendMessage(item.socket, $"[{userName}]: {message}");
                }
            }
        }

        private async void SendMessage(Socket client, string message)
        {
            byte[] bytes = Encoding.UTF8.GetBytes(message);
            await client.SendAsync(new ArraySegment<byte>(bytes), SocketFlags.None);
        }
    }
}
