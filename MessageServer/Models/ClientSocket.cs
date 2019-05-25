using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace MessageServer.Models
{
    public class ClientSocket : INotifyPropertyChanged
    {
        private DateTime lastActiveTime;

        public ClientSocket(TcpClient tcpClient)
        {
            UpdateLastActiveTime();

            TcpClient = tcpClient;
        }

        #region Properties

        public TcpClient TcpClient { get; private set; }

        public string ClientName { get; set; }

        public DateTime LastActiveTime
        {
            get => lastActiveTime;
            set
            {
                if (lastActiveTime == value)
                    return;

                lastActiveTime = value;
                OnPropertyChanged("LastActiveTime");
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;
        public void OnPropertyChanged(string propertyName = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        #endregion

        #region Public methods

        public void UpdateLastActiveTime()
        {
            LastActiveTime = DateTime.Now;
        }

        #endregion
    }
}
