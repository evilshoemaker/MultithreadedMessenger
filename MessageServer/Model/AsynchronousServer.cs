using System;
using System.Net;
using System.Net.Sockets;
using System.IO;
using System.Threading.Tasks;
using MessageCommonLib;
using System.Collections.Generic;
using System.ComponentModel;

namespace MessageServer.Model
{
    public class AsynchronousServer : INotifyPropertyChanged
    {
        private static NLog.Logger logger = NLog.LogManager.GetCurrentClassLogger();

        private IPAddress ipAddress;
        private int port;

        private bool isRunnig = false;

        #region Constructor

        public AsynchronousServer()
        {

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
            set
            {
                if (isRunnig == value)
                    return;

                isRunnig = value;
                OnPropertyChanged("IsRunning");
            }
        }

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
            TcpListener listener = new TcpListener(this.ipAddress, this.port);
            listener.Start();  

            while (true)
            {
                try
                { 
                    TcpClient tcpClient = await listener.AcceptTcpClientAsync();

                    Task processTask = Process(tcpClient);
                    await processTask;

                    logger.Info("Server started");
                }
                catch (Exception ex)
                {
                    logger.Error(ex);
                }
            }
        }

        #endregion

        #region Private methods

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
                logger.Error(ex);

                if (tcpClient.Connected)
                    tcpClient.Close();
            }
        }

        private string Response(string request)
        {
            // assumes request has form like method=average&data=1.1 2.2 3.3&eor
            // eor stands for end-of-request
            // dummy delay bssed on the first numeric value
            /*string[] pairs = request.Split('&');
            string methodName = pairs[0].Split('=')[1];
            string valueString = pairs[1].Split('=')[1];

            string[] values = valueString.Split(' ');
            double[] vals = new double[values.Length];
            for (int i = 0; i < values.Length; ++i)
                vals[i] = double.Parse(values[i]);

            string response = "";
            if (methodName == "average") response += Average(vals);
            else if (methodName == "minimum") response += Minimum(vals);
            else response += "BAD methodName: " + methodName;

            int delay = ((int)vals[0]) * 1000; // dummy delay
            System.Threading.Thread.Sleep(delay);

            return response;*/

            return "";
        }

        #endregion
    }
}
