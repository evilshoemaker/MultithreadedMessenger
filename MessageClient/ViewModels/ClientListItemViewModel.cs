using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MessageClient.ViewModels
{
    public class ClientListItemViewModel
    {
        public ClientListItemViewModel()
        {
            Messages = new ObservableCollection<MessageListItemViewModel>()
            {
                new MessageListItemViewModel()
                {
                    Message = "jdsfsdf;sdkf;sdnfknz kjlfn lksjfng kjfsng ",
                    SenderName = "test 1",
                    MessageSentTime = "12.16"
                },

                new MessageListItemViewModel()
                {
                    Message = "assdgdfgdgh",
                    SenderName = "test 1",
                    SentByMe = true,
                    MessageSentTime = "12.16"
                }
            };


        }

        #region Properties

        public string ClientName { get; set; }

        public ObservableCollection<MessageListItemViewModel> Messages { get; private set; }

        #endregion
    }
}
