using MessageClient.Models;
using MessageCommonLib;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace MessageClient.ViewModels
{
    public class ClientViewModel : INotifyPropertyChanged
    {
        #region Constructor

        public ClientViewModel()
        {
            ConnectCommand = new DelegateCommand(ConnectToHost);
            DisconnectCommand = new DelegateCommand(Disconnect);

            Client = new Client();
            Client.PropertyChanged += Client_PropertyChanged;
        }

        #endregion

        #region Commands

        public ICommand ConnectCommand { get; private set; }
        public ICommand DisconnectCommand { get; private set; }

        #endregion

        #region Properties

        public Client Client { get; private set; }

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
                if (Client.Connected)
                {
                    Client.Authorization();
                }
            }
        }

        private void ConnectToHost(object obj)
        {
            Client.Connect();
        }

        private void Disconnect(object obj)
        {
            Client.Disconnect();
        }

        #endregion
    }
}
