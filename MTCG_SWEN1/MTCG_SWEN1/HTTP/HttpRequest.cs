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
        public string Method { get; set; }
        public string Path { get; set; }
        public string Version { get; set; }
        public string Body { get; set; }

        public Dictionary<string, string> Headers;
        public Dictionary<string, string> EndpointParameters;

        public HttpRequest(TcpClient socket)
        {
            _socket = socket;
            Headers = new();
            EndpointParameters = new();

            // Initialized with null to check at Receive() if first line processed or not.
            Path = null;
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
                    // Read and display lines from the file until the end of the file is reached.
                    while ((line = reader.ReadLine()) != null)
                    {
                        // Empty line announces content with next line.
                        if (line.Length == 0)
                        {
                            ParseBody(reader);
                            return;
                        }                        

                        if (Version == null)
                        {
                            ParseFirstLineRequest(line);
                            ParseParametersForQuery();
                        }
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

            Method = requestFirstLine[0];
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
            try
            {
                if (!Headers.ContainsKey("Content-Length"))
                    throw new KeyNotFoundException("Missing Header error => HttpRequest.cs, ParseBody().");

                while (reader.Peek() >= 0)
                {
                    Body += (char)reader.Read();
                }

            } 
            catch(KeyNotFoundException err)
            {
                Console.WriteLine(err.Message);                
                Body = "";
            } 
            catch (Exception)
            {
                Console.WriteLine($"Parsing content error => HttpRequest.cs, ParseBody().");
                Body = "";
            }
        }

        private void ParseParametersForQuery()
        {
            if (Path.Contains('?'))
            {
                var tempPath = Path.Split('?');
                Path = tempPath[0];
                tempPath = tempPath[1].Split('&');

                foreach (var item in tempPath)
                {
                    var keyValuePair = item.Split('=');
                    EndpointParameters.Add(keyValuePair[0], keyValuePair[1]);
                }
            }
            else
                return;            
        }

        
        public string GetValidEndpoint()
        {
            // Count number of '/' and return value after last '/' as path variable.
            // https://stackoverflow.com/questions/13961472/how-to-count-sets-of-specific-characters-in-a-string
            int slashCount = Path.ToCharArray().Count(symbol => symbol == '/');

            if (slashCount > 1)
                if (Path.Substring(0, Path.LastIndexOf("/")) == "/transactions")
                    //Console.WriteLine($"returned string instead: {a.Substring(a.LastIndexOf("/packages"))}");
                    // Return Path variable "/packages" to find Endpoint.
                    return Path.Substring(Path.LastIndexOf("/packages"));
                else
                    //Console.WriteLine($"returned string: {a.Substring(0, a.LastIndexOf("/"))}");
                    // Return Path variable, because 2nd part of Path equals token or username.
                    return Path.Substring(0, Path.LastIndexOf("/"));

            return Path;
        }

        
    }
}
