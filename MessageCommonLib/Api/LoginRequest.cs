using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MessageCommonLib.Api
{
    public class LoginRequest : JsonRequest
    {
        public LoginRequest()
        {
            Method = "login";
        }

        public LoginRequest(string clientName) : this()
        {
            ClientName = clientName;
        }

        public string ClientName { get; set; }
    }
}
