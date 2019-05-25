using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MessageCommonLib.Api
{
    public class SendMessageRequest : JsonRequest
    {
        public SendMessageRequest()
        {
            Method = "send";
        }

        public string ClientName { get; set; }
        public string Message { get; set; }
        public string SenderName { get; set; }
    }
}
