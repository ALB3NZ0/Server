using System.Windows;

namespace WPFServer.Views
{
    public partial class Server : Window
    {
        public Server()
        {
            InitializeComponent();
        }

        private void StartServer_Click(object sender, RoutedEventArgs e)
        {
            var viewModel = (ServerViewModel)DataContext;
            viewModel.StartServer();
        }
    }
}
