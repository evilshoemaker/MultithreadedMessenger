using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MessageCommonLib.Api
{
    public class LogoutRequest : LoginRequest
    {
        public LogoutRequest()
        {
            Method = "logout";
        }
    }
}
