using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;
using System.Net;
using System.Threading;
using System.Net.NetworkInformation;

namespace MTCG_SWEN1.Server
{
    class HttpServer
    {
        private int _port = 10001;
        private readonly TcpListener _socketListener;
        private readonly static HttpServer s_staticServer = new HttpServer();
        private IPGlobalProperties _properties = IPGlobalProperties.GetIPGlobalProperties();
        private IPAddress _localAddr;

        public static HttpServer GetServerStatic { get => s_staticServer; }

        public int Port { get => _port; set => _port = value; }
       
        private HttpServer()
        {            
            _socketListener = new TcpListener(IPAddress.Loopback, _port);
            _socketListener.Start();

        }
        
    }
}
