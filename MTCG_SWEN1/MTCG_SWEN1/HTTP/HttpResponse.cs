using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace MTCG_SWEN1.HTTP
{
    class HttpResponse
    {
        private TcpClient _socket;

        // Use for sending data as answer on clients request.

        public HttpResponse(TcpClient socket)
        {
            _socket = socket;
        }

        public void Send()
        {
            // Send received serialized data via http as answer.
        }

        public void Receive()
        {
            // Get data from service/db communication (Business Logic).
        }

        public void Parse()
        {
            // Parsing Headers, Version, Status and Content -> split to multiple methods.
        }
    }
}
