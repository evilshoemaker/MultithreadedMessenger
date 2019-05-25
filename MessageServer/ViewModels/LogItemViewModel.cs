using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MessageServer.ViewModels
{
    public class LogItemViewModel
    {
        public LogItemViewModel(DateTime time, string level, string message)
        {
            Time = time;
            Level = level;
            Message = message;
        }

        public DateTime Time { get; private set; }

        public string Message { get; private set; }

        public string Level { get; private set; }
    }
}
