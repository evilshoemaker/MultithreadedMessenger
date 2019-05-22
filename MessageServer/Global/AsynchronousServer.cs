using System;
using System.Net;
using System.Net.Sockets;
using System.IO;
using System.Threading.Tasks;

namespace MessageServer
{
    public class AsynchronousServer
    {
        private IPAddress ipAddress;
        private int port;

        public AsynchronousServer()
        {

        }

        #region Properties

        public IPAddress IpAddress
        {
            get => ipAddress;
            set => ipAddress = value;
        }

        public int Port
        {
            get => port;
            set => port = value;
        }

        #endregion

        public async void Start()
        {
            TcpListener listener = new TcpListener(this.ipAddress, this.port);
            listener.Start();
            

            while (true)
            {
                try
                { 
                    TcpClient tcpClient = await listener.AcceptTcpClientAsync();
                    //Task t = Process(tcpClient);
                    //await t;
                }
                catch (Exception ex)
                {
                    //Console.WriteLine(ex.Message);
                }
            }
        }
    }
}
