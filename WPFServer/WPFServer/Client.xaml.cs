using System;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace WPFServer
{
    public partial class Client : Window
    {
        private string userName;
        private Socket server;

        public Client(string userName)
        {
            InitializeComponent();
            this.userName = userName;

            server = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            server.Connect("26.6.250.111", 8888);
            ReceiveMessage();
        }

        private async void ReceiveMessage()
        {
            while (true)
            {
                byte[] bytes = new byte[1024];
                await server.ReceiveAsync(new ArraySegment<byte>(bytes), SocketFlags.None);
                string message = Encoding.UTF8.GetString(bytes).Trim('\0');

                Application.Current.Dispatcher.Invoke(() =>
                {
                    MessagesLbx.Items.Add(message);
                });
            }
        }

        private async void SendMessage(string message)
        {
            string formattedMessage = $"{DateTime.Now:HH:mm:ss} {userName}: {message}";
            byte[] bytes = Encoding.UTF8.GetBytes(formattedMessage);
            await server.SendAsync(new ArraySegment<byte>(bytes), SocketFlags.None);
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            SendMessage(MessageTxt.Text);
            
        }
    }
}
