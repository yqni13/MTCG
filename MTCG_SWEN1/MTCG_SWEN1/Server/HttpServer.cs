using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MTCG_SWEN1.Server
{
    class HttpServer
    {
        private int _port = 10001;

        private readonly static HttpServer s_staticServer = new HttpServer();

        public static HttpServer GetServerStatic
        {
            get
            {
                return s_staticServer;
            }
        }

        public void SayHello()
        {
            Console.WriteLine("static use successful + port = ", _port);
        }
    }
}
