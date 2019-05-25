using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MessageCommonLib.Api
{
    public class ClientListBroadcast : JsonRequest
    {
        public ClientListBroadcast()
        {
            Method = "clientListBroadcast";
        }

        public List<string> ClientList { get; set; }
    }
}
