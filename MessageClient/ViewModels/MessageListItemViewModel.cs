using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MessageClient.ViewModels
{
    public class MessageListItemViewModel
    {
        public string SenderName { get; set; }

        public string Message { get; set; }

        public bool SentByMe { get; set; }

        public string MessageSentTime { get; set; }
    }
}
