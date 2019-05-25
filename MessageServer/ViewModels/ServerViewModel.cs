using MessageCommonLib;
using MessageServer.Model;
using MessageServer.View;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace MessageServer.ViewModels
{
    public class ServerViewModel
    {
        private static NLog.Logger logger = NLog.LogManager.GetCurrentClassLogger();

        public ServerViewModel()
        {
            StartServerCommand = new DelegateCommand(StartServer);
            StopServerCommand = new DelegateCommand(StopServer);

            Server = new AsynchronousServer();

            LogList = new ObservableCollection<string>();
            NetworkInterfaces = new ObservableCollection<IPAddress>(AsynchronousServer.AvailableNetworkInterfaces());

            App.LogMessage += App_LogMessage;
        }

        #region Commands

        public ICommand StartServerCommand { get; private set; }
        public ICommand StopServerCommand { get; private set; }

        #endregion

        #region Properties

        public ObservableCollection<string> LogList { get; private set; }

        public ObservableCollection<IPAddress> NetworkInterfaces { get; private set; }

        public AsynchronousServer Server { get; private set; }

        #endregion

        #region Private methods

        private void StartServer(object obj)
        {
            Server.Start();
        }

        private void StopServer(object obj)
        {
            Server.Stop();
        }

        private void App_LogMessage(string level, string message)
        {
            LogList.Add(String.Format("{0} {1}", level, message));
        }

        #endregion
    }
}
