using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NUnit.Framework;
using MTCG_SWEN1.Server;
using System.Net.Sockets;
using System.Net;
using System.Net.NetworkInformation;

namespace MTCG_SWEN1.Test.Server
{
    [TestFixture]
    class TestHttpServer
    {
        // Arrange
        private IPGlobalProperties _properties = IPGlobalProperties.GetIPGlobalProperties();
        private IPAddress _localAddr = IPAddress.Parse("127.0.0.1");
        private int _port = 10001;

        [Test]
        public void TestHttpServer_ConnectExistingSocket()
        {
            IPEndPoint[] endPoints = _properties.GetActiveTcpListeners();

            foreach (IPEndPoint e in endPoints)
            {
                if(e.ToString() == $"{_localAddr}:{_port}")
                {
                    Assert.IsTrue(false, "port existing");
                }                
            }
            Assert.IsTrue(true, "port not existing");
        }
    }
}
