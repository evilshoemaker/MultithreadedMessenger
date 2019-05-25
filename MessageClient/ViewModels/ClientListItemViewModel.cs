using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MessageClient.ViewModels
{
    public class ClientListItemViewModel : INotifyPropertyChanged
    {
        private string currentMessage;

        public ClientListItemViewModel()
        {
            Messages = new ObservableCollection<MessageListItemViewModel>();
        }

        #region Properties

        public string ClientName { get; set; }

        public ObservableCollection<MessageListItemViewModel> Messages { get; private set; }

        public string CurrentMessage
        {
            get => currentMessage;
            set
            {
                if (currentMessage == value)
                    return;

                currentMessage = value;
                OnPropertyChanged("CurrentMessage");
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;
        public void OnPropertyChanged(string propertyName = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        #endregion
    }
}
