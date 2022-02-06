using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;
using System.Net;
using System.Threading;
using System.Net.NetworkInformation;
using MTCG_SWEN1.HTTP;

namespace MTCG_SWEN1.Server
{
    class HttpServer
    {
        private int _port = 10001;
        private readonly string _serverName = "MTCG_SWEN1";
        private readonly string _creator = "Lukas Varga [if20b167]";
        private volatile bool _serverIsActive = false;
        
        private TcpListener _socketListener;
        
        private readonly static HttpServer s_staticServer = new HttpServer();

        private Thread t_serverThread;

        public static HttpServer GetServerStatic { get => s_staticServer; }

        public int Port { get => _port; set => _port = value; }
       
        private HttpServer()
        {
            _socketListener = new TcpListener(IPAddress.Loopback, _port);
            _serverIsActive = false;
        }        

        public void StartServerThread()
        {
            if (_serverIsActive)
                return;

            t_serverThread = new Thread(RunServer);
            t_serverThread.Start();

            while (!_serverIsActive)
                Thread.Sleep(1);
        }

        private void RunServer()
        {            
            _socketListener.Start(5);
            Console.WriteLine($"Server {_serverName} started by {_creator}, {DateTime.UtcNow.AddHours(1)}\nWaiting for connection...");
            _serverIsActive = true;

            while (_serverIsActive)
            {
                TcpClient socket = _socketListener.AcceptTcpClient();
                HttpProcessor processor = new HttpProcessor(socket, this);
                new Thread(processor.Process).Start();
                Thread.Sleep(1);
            }
        }
    }
}
