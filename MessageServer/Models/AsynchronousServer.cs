using System;
using System.Net;
using System.Net.Sockets;
using System.IO;
using System.Threading.Tasks;
using MessageCommonLib;
using System.Collections.Generic;
using System.ComponentModel;
using MessageCommonLib.Api;
using MessageServer.Models;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows.Threading;

namespace MessageServer.Model
{
    public class AsynchronousServer : INotifyPropertyChanged
    {
        #region Private properties

        private static NLog.Logger logger = NLog.LogManager.GetCurrentClassLogger();

        private const int ACTIVE_TIMEOUT_SEC = 60*5;

        private IPAddress ipAddress;
        private int port = 8080;

        private bool isRunnig = false;

        private TcpListener listener = null;

        private DispatcherTimer activeClientTimer = null;

        #endregion

        #region Constructor

        public AsynchronousServer()
        {
            ClientList = new ObservableCollection<ClientSocket>();

            activeClientTimer = new DispatcherTimer();
            activeClientTimer.Interval = new TimeSpan(0, 0, 2);
            activeClientTimer.Tick += ActiveClientTimer_Tick;
        }

        #endregion

        #region Properties

        public IPAddress IpAddress
        {
            get => ipAddress;
            set
            {
                if (ipAddress == value)
                    return;

                ipAddress = value;
                OnPropertyChanged("IpAddress");
            }
        }

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

        public bool IsRunning
        {
            get => isRunnig;
            private set
            {
                if (isRunnig == value)
                    return;

                isRunnig = value;
                OnPropertyChanged("IsRunning");
            }
        }

        public ObservableCollection<ClientSocket> ClientList { get; private set; }

        public event PropertyChangedEventHandler PropertyChanged;
        public void OnPropertyChanged(string propertyName = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        #endregion

        #region Static methods

        public static List<IPAddress> AvailableNetworkInterfaces()
        {
            string hostName = Dns.GetHostName();
            IPHostEntry ipHostInfo = Dns.GetHostEntry(hostName);

            List<IPAddress> ipAddressesList = new List<IPAddress>();

            ipAddressesList.Add(IPAddress.Any);

            for (int i = 0; i < ipHostInfo.AddressList.Length; ++i)
            {
                if (ipHostInfo.AddressList[i].AddressFamily == AddressFamily.InterNetwork)
                {
                    ipAddressesList.Add(ipHostInfo.AddressList[i]);
                }
            }

            return ipAddressesList;
        }

        #endregion

        #region Public methods

        public async void Start()
        {
            try
            {
                listener = new TcpListener(ipAddress, port);
                listener.Start();
                activeClientTimer.Start();

                logger.Info(String.Format("Server running on {0} port {1}", ipAddress, port));
                IsRunning = true;
            }
            catch (Exception ex)
            {
                logger.Error(ex.Message);
                return;
            }

            while (true)
            {
                try
                {
                    TcpClient tcpClient = await listener.AcceptTcpClientAsync();

                    Task processTask = Process(tcpClient);
                    //await processTask;
                }
                catch (Exception ex)
                {
                    //logger.Error(ex.Message);
                    break;
                }
            }

            listener = null;
            logger.Info("Server stopped");
            IsRunning = false;
        }

        public void Stop()
        {
            listener.Stop();
            activeClientTimer.Stop();
        }

        #endregion

        #region Private methods

        private void ActiveClientTimer_Tick(object sender, EventArgs e)
        {
            IEnumerable<ClientSocket> notActiveClients = ClientList
                .Where(x => (DateTime.Now - x.LastActiveTime).TotalSeconds > ACTIVE_TIMEOUT_SEC).ToList();

            foreach (ClientSocket client in notActiveClients)
            {
                RemoveClient(client.TcpClient);
            }
        }

        private void RemoveClient(TcpClient tcpClient)
        {
            ClientSocket client = ClientList.SingleOrDefault(x => x.TcpClient == tcpClient);

            logger.Info(String.Format("Logout client [{0}]", client.ClientName));

            ClientList.Remove(client);
            //ClientList.RemoveAll(x => x.TcpClient == tcpClient);
            tcpClient.Close();

            SendAllClientList();
        }

        private void LoginClient(TcpClient tcpClient, string clientName)
        {
            ClientSocket client = ClientList.SingleOrDefault(x => x.TcpClient == tcpClient && x.ClientName == clientName);
            if (client != null)
            {
                client.UpdateLastActiveTime();
            }

            client = ClientList.SingleOrDefault(x => x.TcpClient != tcpClient && x.ClientName == clientName);
            if (client == null)
            {
                client = new ClientSocket(tcpClient);
                client.ClientName = clientName;

                ClientList.Add(client);
                SendAllClientList();

                logger.Info(String.Format("Login new client {0} [{1}]", 
                    ((IPEndPoint)tcpClient.Client.RemoteEndPoint).Address.ToString(),
                    clientName));
            }
            else
            {
                throw new Exception("Client with this name is already registered");
            }
        }

        private void LogoutClient(string clientName)
        {
            ClientSocket client = ClientList.SingleOrDefault(x => x.ClientName != clientName);
            if (client != null)
            {
                RemoveClient(client.TcpClient);
                SendAllClientList();

                logger.Info(String.Format("Logout client {0} [{1}]",
                    ((IPEndPoint)client.TcpClient.Client.RemoteEndPoint).Address.ToString(),
                    clientName));
            }
        }

        private void SendAllClientList()
        {
            ClientListBroadcast broadcast = new ClientListBroadcast();
            broadcast.ClientList = ClientList.Select(x => x.ClientName).ToList();

            foreach(ClientSocket client in ClientList)
            {
                try
                {
                    NetworkStream networkStream = client.TcpClient.GetStream();
                    StreamWriter writer = new StreamWriter(networkStream);
                    writer.AutoFlush = true;

                    writer.WriteLineAsync(broadcast.ToJson());
                }
                catch (Exception ex)
                {
                    logger.Error(ex.Message);
                }
            }
        }

        private async Task SendMessage(SendMessageRequest request)
        {
            logger.Info(String.Format("[{0}] => [{1}] {2}",
                    request.SenderName, request.ClientName, request.Message));

            ClientSocket client = ClientList.SingleOrDefault(x => x.ClientName == request.ClientName);
            if (client != null)
            {
                client.UpdateLastActiveTime();

                try
                {
                    NetworkStream networkStream = client.TcpClient.GetStream();
                    StreamWriter writer = new StreamWriter(networkStream);
                    writer.AutoFlush = true;

                    await writer.WriteLineAsync(request.ToJson());
                }
                catch
                {
                    throw new Exception("Error while sending message");
                }
            }
            else
            {
                throw new Exception("Client not found or already registered");
            }
        }

        private async Task Process(TcpClient tcpClient)
        {
            string clientEndPoint = tcpClient.Client.RemoteEndPoint.ToString();

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
                        object obj = JsonRequest.FromJson(request);
                        if (obj.GetType() == typeof(LoginRequest))
                        {
                            LoginRequest loginRequest = (LoginRequest)obj;

                            try
                            {
                                LoginClient(tcpClient, loginRequest.ClientName);

                                JsonResponse response = new JsonResponse(JsonResponse.ResponseResults.Success, "", loginRequest.RequestId);
                                await writer.WriteLineAsync(response.ToJson());
                            }
                            catch (Exception ex)
                            {
                                JsonResponse response = new JsonResponse(JsonResponse.ResponseResults.Error, ex.Message, loginRequest.RequestId);
                                await writer.WriteLineAsync(response.ToJson());
                            }
                            
                        }
                        else if (obj.GetType() == typeof(LogoutRequest))
                        {
                            LogoutRequest logoutRequest = (LogoutRequest)obj;

                            LogoutClient(logoutRequest.ClientName);

                            JsonResponse response = new JsonResponse(JsonResponse.ResponseResults.Success, "", logoutRequest.RequestId);
                            await writer.WriteLineAsync(response.ToJson());

                            break;
                        }
                        else if (obj.GetType() == typeof(SendMessageRequest))
                        {
                            SendMessageRequest messageRequest = (SendMessageRequest)obj;

                            try
                            {
                                await SendMessage(messageRequest);

                                JsonResponse response = new JsonResponse(JsonResponse.ResponseResults.Success, "", messageRequest.RequestId);
                                await writer.WriteLineAsync(response.ToJson());
                            }
                            catch
                            {
                                JsonResponse response = new JsonResponse(JsonResponse.ResponseResults.Error, "", messageRequest.RequestId);
                                await writer.WriteLineAsync(response.ToJson());
                            }
                        }
                        else
                        {
                            JsonResponse response = new JsonResponse(JsonResponse.ResponseResults.Error,
                                "Incorrect request");

                            await writer.WriteLineAsync(response.ToJson());

                            RemoveClient(tcpClient);
                            break;
                        }
                    }
                    else
                    {
                        RemoveClient(tcpClient);
                        break;
                    }
                }
            }
            catch (Exception ex)
            {
                //logger.Error(ex.Message);

                RemoveClient(tcpClient);

                if (tcpClient.Connected)
                    tcpClient.Close();
            }
        }

        private string Response(string request)
        {
            return "";
        }

        #endregion
    }
}
