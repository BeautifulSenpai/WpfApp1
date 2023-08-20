using SocketIOClient;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace WpfApp1
{
    public partial class MainWindow : Window
    {
        private SocketIO socket;

        public MainWindow()
        {
            InitializeComponent();
            ConnectToServer(true);
            InitializeSocket();
        }

        private async void ConnectToServer(bool connect)
        {
            using (HttpClient client = new HttpClient())
            {
                string action = connect ? "connect" : "disconnect";
                Uri url = new Uri($"http://62.217.182.138:3000?action={action}");
                await client.GetAsync(url);

                if (connect)
                {
                    Console.WriteLine("Connected from WPF application");
                }
                else
                {
                    Console.WriteLine("Disconnected from WPF application");
                }
            }
        }

        private void InitializeSocket()
        {
            socket = new SocketIO("http://62.217.182.138:3000");
            socket.On("text-received", (data) => {
                Dispatcher.Invoke(() => {
                    UpdateTextBlock(data.ToString());
                });
            });
            socket.ConnectAsync();
        }

        private void UpdateTextBlock(string text)
        {
            textBlock.Text = text;
        }

        protected override void OnClosed(EventArgs e)
        {
            ConnectToServer(false);
            base.OnClosed(e);
        }
    }
}


