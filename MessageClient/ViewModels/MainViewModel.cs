using MessageClient.Models;
using MessageCommonLib;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

namespace MessageClient.ViewModels
{
    public class MainViewModel : INotifyPropertyChanged
    {
        #region Private properties

        private ClientListItemViewModel currentClient;

        #endregion

        #region Constructor

        public MainViewModel()
        {
            ConnectCommand = new DelegateCommand(ConnectToHost);
            DisconnectCommand = new DelegateCommand(Disconnect);
            SendMessageCommand = new DelegateCommand(SendMessage);

            TcpClient = new Client();
            TcpClient.PropertyChanged += Client_PropertyChanged;
            TcpClient.UpdateClientList += TcpClient_UpdateClientList;
            TcpClient.NewMessage += TcpClient_NewMessage;
            TcpClient.ErrorOccurred += TcpClient_ErrorOccurred;

            ClientList = new ObservableCollection<ClientListItemViewModel>();
        }

        #endregion

        #region Commands

        public ICommand ConnectCommand { get; private set; }
        public ICommand DisconnectCommand { get; private set; }
        public ICommand SendMessageCommand { get; private set; }

        #endregion

        #region Properties

        public Client TcpClient { get; private set; }

        public ObservableCollection<ClientListItemViewModel> ClientList { get; private set; }

        public ClientListItemViewModel CurrentClient
        {
            get => currentClient;
            set
            {
                currentClient = value;
                OnPropertyChanged("CurrentClient");
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;
        public void OnPropertyChanged(string propertyName = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        #endregion

        #region Private methods

        private void Client_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            if (e.PropertyName == "Connected")
            {
                if (TcpClient.Connected)
                {
                    TcpClient.Login();
                }
            }
        }

        private void TcpClient_UpdateClientList(List<string> list)
        {
            IEnumerable<string> newItems = list.Where(x => !ClientList.Any(y => x == y.ClientName));

            foreach (string name in newItems)
            {
                ClientList.Add(new ClientListItemViewModel { ClientName = name});
            }

            IEnumerable<ClientListItemViewModel> removeItems = ClientList.Where(x => !list.Any(y => x.ClientName == y));

            foreach (ClientListItemViewModel item in removeItems)
            {
                ClientList.Remove(item);
            }
        }

        private void TcpClient_NewMessage(string senderName, string message)
        {
            ClientListItemViewModel client = ClientList.SingleOrDefault(x => x.ClientName == senderName);

            if (client == null)
                return;

            client.Messages.Add(new MessageListItemViewModel
            {
                Message = message,
                SentByMe = false,
                MessageSentTime = DateTime.Now.ToString("dd MMM HH:mm")
            });
        }

        private void TcpClient_ErrorOccurred(string errorMessage)
        {
            MessageBox.Show(errorMessage, "Errror");
        }

        private void ConnectToHost(object obj)
        {
            TcpClient.Connect();
        }

        private void Disconnect(object obj)
        {
            TcpClient.Disconnect();
        }

        private void SendMessage(object obj)
        {
            if (CurrentClient == null 
                || String.IsNullOrEmpty(CurrentClient.ClientName)
                || String.IsNullOrEmpty(CurrentClient.CurrentMessage)
                || !TcpClient.Connected)
            {
                return;
            }

            TcpClient.SendMessage(CurrentClient.ClientName, CurrentClient.CurrentMessage);

            CurrentClient.Messages.Add(new MessageListItemViewModel
            {
                Message = CurrentClient.CurrentMessage,
                SenderName = TcpClient.ClientName,
                SentByMe = true,
                MessageSentTime = DateTime.Now.ToString("dd MMM HH:mm")
            });

            CurrentClient.CurrentMessage = "";
        }

        #endregion
    }
}
