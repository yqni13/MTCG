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
        public string StatusMessage { get; set; }
        public string Version { get; private set; }
        public string BodyType { get; set; }
        public string BodyContent { get; set; } // only post
        public Dictionary<string, string> Headers { get; set; }
        //private Dictionary<HttpStatusCode, string> StatusCodeString;

        private TcpClient _socket;

        // Use for sending data as answer on clients request.

        public HttpResponse(TcpClient socket)
        {
            _socket = socket;
            Headers = new();
        }


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
                            //ParseBody(reader);
                            return;
                        }

                        if (Version == null)
                        {
                            ParseVersionAndStatus(line);
                            //ParseParametersForQuery();
                        }
                        else                            
                            //ParseHeader(line);
                    }
                }
            }
            catch (Exception err)
            {
                // Let the user know what went wrong.
                Console.WriteLine("The Request could not be read:");
                Console.WriteLine(err);
            }
        }

        public void ParseVersionAndStatus(string line)
        {
            var responseFirstLine = line.Split(' ');
            Version = responseFirstLine[0];

            // Placeholder version to get the regarding status message.
            string statusCode = responseFirstLine[1];
            StatusMessage = HttpStatusMessageConverter.GetPlaceholderStatusCode(int.Parse(statusCode));
        }        

        public void Send()
        {
            // Send received serialized data via http as answer.
        }

        /*public void AddBody(string bodyType, string bodyContent)
        {
            BodyType = bodyType;
            BodyContent = bodyContent;
        }*/

        
    }
}
