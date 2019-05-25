using MessageCommonLib.Api;
using System;
using System.ComponentModel;
using System.IO;
using System.Net;
using System.Net.Sockets;
using System.Threading.Tasks;

namespace MessageClient.Models
{
    public class Client : INotifyPropertyChanged
    {
        #region Private properties

        private string hostAddress;
        private int port;
        private string clientName;

        private bool connected = false;

        private TcpClient tcpClient;

        #endregion

        #region Constructor

        public Client()
        {
            tcpClient = new TcpClient();
        }

        #endregion

        #region Properties

        public int Port
        {
            get => port;
            set
            {
                if (port == value)
                    return;

                port = value;
                OnPropertyChanged("Port");
            }
        }

        public string HostAddress
        {
            get => hostAddress;
            set
            {
                if (hostAddress == value)
                    return;

                hostAddress = value;
                OnPropertyChanged("HostAddress");
            }
        }

        public string ClientName
        {
            get => clientName;
            set
            {
                if (clientName == value)
                    return;

                clientName = value;
                OnPropertyChanged("ClientName");
            }
        }

        public bool Connected
        {
            get => connected;
            private set
            {
                if (connected == value)
                    return;

                connected = value;
                OnPropertyChanged("Connected");
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;
        public void OnPropertyChanged(string propertyName = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
        #endregion

        #region Public methods

        public async void Connect()
        {
            IPAddress ipAddress = null;
            IPHostEntry ipHostInfo = Dns.GetHostEntry(hostAddress);

            for (int i = 0; i < ipHostInfo.AddressList.Length; ++i)
            {
                if (ipHostInfo.AddressList[i].AddressFamily == AddressFamily.InterNetwork)
                {
                    ipAddress = ipHostInfo.AddressList[i];
                    break;
                }
            }

            if (ipAddress == null)
            {
                throw new Exception("Unable to find an IPv4 address for server");
            }

            await tcpClient.ConnectAsync(ipAddress, port);

            Connected = true;

            try
            {
                Task processTask = Process(tcpClient);
                await processTask;

                //logger.Info("Server started");
                //IsRunning = true;
            }
            catch (Exception ex)
            {
                //logger.Error(ex);
            }
        }

        public async void Disconnect()
        {
            if (tcpClient.Connected)
            {
                try
                {
                    NetworkStream networkStream = tcpClient.GetStream();
                    StreamWriter writer = new StreamWriter(networkStream);

                    writer.AutoFlush = true;

                    LogoutRequest logoutRequest = new LogoutRequest();
                    await writer.WriteLineAsync(logoutRequest.ToJson());
                }
                catch
                {

                }

                tcpClient.Close();
            }
                
            Connected = false;
        }

        public async void Authorization()
        {
            if (!tcpClient.Connected)
            {
                Connected = false;
                return;
            }

            try
            {
                NetworkStream networkStream = tcpClient.GetStream();
                StreamWriter writer = new StreamWriter(networkStream);

                writer.AutoFlush = true;

                LoginRequest authorizationRequest = new LoginRequest(clientName);

                await writer.WriteLineAsync(authorizationRequest.ToJson());
            }
            catch
            {
                Connected = false;
            }
        }

        #endregion

        #region Private methods

        private async Task Process(TcpClient tcpClient)
        {
            try
            {
                NetworkStream networkStream = tcpClient.GetStream();
                StreamReader reader = new StreamReader(networkStream);
                StreamWriter writer = new StreamWriter(networkStream);
                writer.AutoFlush = true;

                while (true)
                {
                    string request = await reader.ReadLineAsync();
                    if (request != null)
                    {
                        /*Console.WriteLine("Received service request: " + request);
                        string response = Response(request);
                        Console.WriteLine("Computed response is: " + response + "\n");
                        await writer.WriteLineAsync(response);*/
                    }
                    else
                    {
                        break;
                    }
                }

                tcpClient.Close();
            }
            catch (Exception ex)
            {
                //logger.Error(ex);

                if (tcpClient.Connected)
                {
                    tcpClient.Close();
                    Connected = false;
                }
            }
        }

        #endregion
    }
}
