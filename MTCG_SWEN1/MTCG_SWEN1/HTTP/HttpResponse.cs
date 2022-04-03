using MTCG_SWEN1.Server;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace MTCG_SWEN1.HTTP
{
    class HttpResponse
    {
        private bool BodyNotNull = false;

        public string StatusMessage { get; set; }
        public string Version { get; set; }
        public string Body { get; set; }
        public Dictionary<string, string> Headers { get; set; }
        //private Dictionary<HttpStatusCode, string> StatusCodeString;

        private TcpClient _socket;

        // Use for sending data as answer on clients request.

        public HttpResponse(TcpClient socket)
        {
            _socket = socket;
            Headers = new();
        }

        public void Send()
        {
            StreamWriter writer = new(_socket.GetStream()) { AutoFlush = true };
            SendServerAnswer();
            if (Version == null)
                Version = "1.1";


            WriteLine(writer, $"HTTP/{Version} {StatusMessage}");            
            WriteLine(writer, $"Datestamp: {DateTime.UtcNow.AddHours(1)}");
            WriteLine(writer, $"Server: {HttpServer.GetStaticServer._serverName}");
            if (Headers.Count != 0)
                foreach (var pair in Headers)
                    WriteLine(writer, $"{pair.Key}: {pair.Value}");
            /*if (BodyNotNull)
                WriteLine(writer, $"Content-Type: application/json; charset=UTF-8");
            WriteLine(writer, $"Content-Lenght: {Body.Length}");

            if (BodyNotNull)
                WriteLine(writer, Body);
            */
            if(!BodyNotNull)
            {
                WriteLine(writer, $"Content-Lenght: {Body.Length}");
                WriteLine(writer, $"Content-Type: application/json; charset=UTF-8");
                WriteLine(writer, "");
                WriteLine(writer, Body);

            }
            else
            {
                WriteLine(writer, $"Content-Lenght: {Body.Length}");
            }

            //writer.WriteLine();
            writer.Flush();
            writer.Close();
        }

        private void SendServerAnswer()
        {
            Console.WriteLine($"Response sent - Status: {StatusMessage}\n");
        }

        private void WriteLine(StreamWriter writer, string message)
        {
            writer.WriteLine(message);
        }
        
    }
}
