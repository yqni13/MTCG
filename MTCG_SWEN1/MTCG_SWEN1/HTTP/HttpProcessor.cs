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

    /// <summary>
    /// This version is the simple code to catch an request and display the header information.
    /// Instead of the HttpProcessor, the code will be expanded to an own class of Request and Response.
    /// Additionally, the ConnectionHandling class will take over part of HttpProcessor's work and 
    /// act as the central allocation unit of connection traffic.
    /// </summary>
    class HttpProcessor
    {
        private TcpClient _socket;
        private HttpServer _httpServer;
        private bool _wrongMethod = false;

        public EHttpMethods Method { get; private set; }
        public string Path { get; private set; }
        public string Version { get; private set; }

        public Dictionary<string, string> Headers { get; }

        public HttpProcessor(TcpClient s, HttpServer httpServer)
        {
            this._socket = s;
            this._httpServer = httpServer;

            //Method = null;
            Headers = new();
        }

        public void Process()
        {
            var writer = new StreamWriter(_socket.GetStream()) { AutoFlush = true };
            var reader = new StreamReader(_socket.GetStream());
            Console.WriteLine($"Connected to {_httpServer.GetHostName()}, {DateTime.UtcNow.AddHours(1)}\n");

            // read (and handle) the full HTTP-request
            string line = null;
            while ((line = reader.ReadLine()) != null)
            {
                Console.WriteLine(line);
                if (line.Length == 0)
                    break;  // empty line means next comes the content (which is currently skipped)

                // handle first line of HTTP
                if (Path == null)
                {
                    var parts = line.Split(' ');
                    Method = Enum.Parse<EHttpMethods>(parts[0].ToUpper());
                    Path = parts[1];
                    Version = parts[2];
                }
                // handle HTTP headers
                else
                {
                    var parts = line.Split(": ");
                    Headers.Add(parts[0], parts[1]);
                }
            }

            // write the full HTTP-response
            string content = $"<html><body><h1>test server</h1>" +
                $"Current Time: {DateTime.Now}" +
                $"<form method=\"GET\" action=\"/form\">" +
                $"<input type=\"text\" name=\"foo\" value=\"foovalue\">" +
                $"<input type=\"submit\" name=\"bar\" value=\"barvalue\">" +
                $"</form></html>";

            Console.WriteLine($"[Method extrahiert: {Method}");
            Console.WriteLine($"Path extrahiert: {Path}");
            Console.WriteLine($"Version extrahiert: {Version}]\n");

            WriteLine(writer, "HTTP/1.1 200 OK");
            WriteLine(writer, "Server: MTCG Server [yqni13]");
            WriteLine(writer, $"Current Time: {DateTime.Now}");
            WriteLine(writer, $"Content-Length: {content.Length}");
            WriteLine(writer, "Content-Type: text/html; charset=utf-8");
            WriteLine(writer, "");
            WriteLine(writer, content);

            
            writer.WriteLine();
            writer.Flush();
            writer.Close();

        }

        private void WriteLine(StreamWriter writer, string s)
        {
            Console.WriteLine(s);
            writer.WriteLine(s);
        }
    }
}
