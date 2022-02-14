using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace MTCG_SWEN1.HTTP
{
    class HttpRequest
    {
        // Use for asking data as client demands.
        // Query stands for request of data from database

        private TcpClient _socket;
        public EHttpMethods Method { get; private set; }
        public string Path { get; private set; }
        public string Version { get; private set; }
        public string Body { get; private set; }

        public Dictionary<string, string> Headers;
        public Dictionary<string, string> EndpointParameters;

        public HttpRequest(TcpClient socket)
        {
            _socket = socket;

            // Initialized with null to check at Receive() if first line processed or not.
            Path = null;
            Headers = new();
            EndpointParameters = new();
        }

        // Send deserialiced data to respective service to do Business Logic.
        public void Send()
        {

        }

        // Receive incoming data request from client.
        public void Receive()
        {            
            try
            {
                // Create an instance of StreamReader to read from a file.
                StreamReader reader = new(_socket.GetStream());
                {
                    string line;
                    // Read and display lines from the file until the end of
                    // the file is reached.
                    while ((line = reader.ReadLine()) != null)
                    {
                        // Empty line announces content with next line.
                        if (line.Length == 0)
                            ParseBody(reader);

                        if (Version == null)
                            ParseFirstLineRequest(line);
                        else
                            ParseHeader(line);
                    }
                }
            }
            catch (Exception err)
            {
                // Let the user know what went wrong.
                Console.WriteLine("The Request could not be read:");
                Console.WriteLine(err.Message);
            }
        }
                
        private void ParseFirstLineRequest(string line)
        {
            var requestFirstLine = line.Split(' ');

            // Control usable methods via enum EHttpMethods.
            try
            {
                if (!Enum.IsDefined(typeof(EHttpMethods), requestFirstLine[0].ToUpper()))
                    throw new ArgumentException($"Using method ({requestFirstLine[0].ToUpper()}) is prohibited.");

                Method = Enum.Parse<EHttpMethods>(requestFirstLine[0].ToUpper());
            }
            catch (ArgumentException err)
            {               
                Console.WriteLine(err.Message);
                System.Environment.Exit(0);
            }            
            Path = requestFirstLine[1];
            Version = requestFirstLine[2];
        }

        private void ParseHeader(string line)
        {
            var requestHeader = line.Split(": ");
            Headers.Add(requestHeader[0], requestHeader[1]);
        }

        private void ParseBody(StreamReader reader)
        {            
            if (Headers.ContainsKey("Content-Length"))
            {
                var bodyBuffer = new char[int.Parse(Headers["Content-Length"])];
                Body = new string(bodyBuffer);
            }
            else
            {
                Body = "";
            }
        }
    }
}
