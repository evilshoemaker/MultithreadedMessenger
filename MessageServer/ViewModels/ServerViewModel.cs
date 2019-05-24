using MessageServer.Model;
//using MessageCommonLib;
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

        public ServerViewModel()
        {
            StartServerCommand = new DelegateCommand(StartServer);
            StopServerCommand = new DelegateCommand(StopServer);

            Server = new AsynchronousServer();
        }

        #region Commands

        public ICommand StartServerCommand { get; private set; }
        public ICommand StopServerCommand { get; private set; }

        #endregion

        #region Properties

        public ObservableCollection<IPAddress> NetworkInterfaces { get; private set; }

        public AsynchronousServer Server { get; private set; }

        #endregion

        #region Private methods

        private void StartServer(object obj)
        {
            //Server.IsRunning = true;

            MessageCommonLib.WindowService.Show(typeof(ConnectSettingsWindow), this, false, b => { });
        }

        private void StopServer(object obj)
        {
            Server.IsRunning = false;
        }

        #endregion
    }
}
