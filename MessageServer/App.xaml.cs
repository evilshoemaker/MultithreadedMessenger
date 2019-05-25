using MessageCommonLib;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;

namespace MessageServer
{
    /// <summary>
    /// Логика взаимодействия для App.xaml
    /// </summary>
    public partial class App : Application
    {
        public static event LogMessageDelegate LogMessage;

        public static void LogMethod(string level, string message)
        {
            LogMessage?.Invoke(level, message);
        }
    }
}
