using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.ServiceModel;
using Client.ServiceChat;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Client
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window, IServiceChatCallback
    {

        ServiceChatClient client;
        bool isConnected = false;
        int id;
        public MainWindow()
        {
            InitializeComponent();
        }

        private void Connect_Click(object sender, RoutedEventArgs e)
        {
            if (textBoxUserName.Text != String.Empty)
            {
                if (isConnected)
                {
                    DisconnectUser();
                }
                else
                {
                    ConnectUser();
                }
            }
            else
            {
                MessageBox.Show("Введите имя пользователя, чтобы присоединиться к чату.");
            }
        }

        void ConnectUser()
        {
            if (!isConnected)
            {
                client = new ServiceChat.ServiceChatClient(new InstanceContext(this));
                id = client.Connect(textBoxUserName.Text);
                buttonConnect.Content = "Leave the chat";
                isConnected = true;
                textBoxUserName.IsEnabled = false;
            }
        }
        void DisconnectUser()
        {
            if (isConnected)
            {
                client.Disconnect(id);
                client = null;
                buttonConnect.Content = "Join the chat";
                isConnected = false;
                textBoxUserName.IsEnabled = true;
            }
        }

        public void CallBackMessage(string message)
        {
            listBoxChat.Items.Add(message);
            listBoxChat.ScrollIntoView(listBoxChat.Items[listBoxChat.Items.Count - 1]);
        }

        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            DisconnectUser();
        }

        private void Message_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter) 
            {
                if (client != null)
                {
                    if (textBoxMessage.Text != String.Empty)
                    {
                        client.SendMessage(textBoxMessage.Text, id);
                        textBoxMessage.Text = String.Empty;
                    }
                    else
                    {
                        MessageBox.Show("Введите сообщение, чтобы его отправить.");
                    }
                }
                else
                {
                    MessageBox.Show("Вы не подключились к чату, чтобы отправлять сообщения.");
                }
            }
        }
    }
}
