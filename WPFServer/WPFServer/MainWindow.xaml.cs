using System;
using System.Collections.Generic;
using System.Net.Sockets;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using WPFServer.Views;

namespace WPFServer
{
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();

        }

        private void OpenServerWindow(object sender, RoutedEventArgs e)
        {
            Server serverWindow = new Server();
            serverWindow.Show();
        }

        private void OpenClientWindow(object sender, RoutedEventArgs e)
        {
            string userName = NameTextBox.Text;
            Client clientWindow = new Client(userName);
            clientWindow.Show();
        }

    }
}
