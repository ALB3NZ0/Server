using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Windows;
using WPFServer.Models;

namespace WPFServer
{
    public class ServerViewModel : INotifyPropertyChanged
    {
        private string _userName;
        private TcpServer _server;

        public ObservableCollection<MessageModel> Messages { get; set; }
        public ObservableCollection<UserModel> Users { get; set; }

        public string UserName
        {
            get => _userName;
            set
            {
                _userName = value;
                OnPropertyChanged();
            }
        }

        public ServerViewModel()
        {
            Messages = new ObservableCollection<MessageModel>();
            Users = new ObservableCollection<UserModel>();
        }

        public void StartServer()
        {
            if (string.IsNullOrEmpty(UserName))
            {
                MessageBox.Show("Пожалуйста, введите имя пользователя.");
                return;
            }

            _server = new TcpServer(this);
            _server.Start();
        }

        public void AddMessage(string userName, string text)
        {
            Application.Current.Dispatcher.Invoke(() => Messages.Add(new MessageModel { UserName = userName, Text = text }));
        }

        public void AddUser(string userName)
        {
            Application.Current.Dispatcher.Invoke(() => Users.Add(new UserModel { UserName = userName }));
        }

        public void RemoveUser(string userName)
        {
            Application.Current.Dispatcher.Invoke(() =>
            {
                var user = Users.FirstOrDefault(u => u.UserName == userName);
                if (user != null)
                {
                    Users.Remove(user);
                }
            });
        }

        public event PropertyChangedEventHandler PropertyChanged;

        protected void OnPropertyChanged([CallerMemberName] string name = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
        }
    }
}
