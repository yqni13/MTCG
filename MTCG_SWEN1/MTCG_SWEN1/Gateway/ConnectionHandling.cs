using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using MTCG_SWEN1.Endpoints.Attributes;
using MTCG_SWEN1.HTTP;

namespace MTCG_SWEN1.Server
{
    /// <summary>
    /// Combined with the classes HttpRequest and HttpResponse, the ConnectionHandling class
    /// will act as central allocation unit of connection traffic and replace the original HttpProcessor.
    /// </summary>
    public class ConnectionHandling
    {
        private readonly TcpClient _socket;
        private readonly HttpRequest _request;
        private readonly HttpResponse _response;


        public ConnectionHandling(TcpClient socket)
        {           
            _socket = socket;
            _request = new HttpRequest(socket);
            _response = new HttpResponse(socket);            
            ConnectionThreading();
        }

        private async void ConnectionThreading()
        {
            
            await Task.Run(() => { Process(); });
            _socket.Close();
        }

        private void Process()
        {
            
            //Console.WriteLine(_request.Method);
            _request.Receive();
            
            try
            {
                // Assemble EndpointPath and check which class will reach
                var endpointPath = _request.GetValidEndpoint();
                var endpointMethod = _request.Method;
                var endType = GetEndpointType();
                var endMethodInfo = GetEndpointMethodInfo(endType);
                Console.WriteLine($"Endpoint reached: \"{endpointMethod}{endpointPath}\"");
                InvokingEndpoint(endMethodInfo, endType);
            }
            catch(Exception e)
            {
                Console.WriteLine(e.Message);
                //_response.Send();
                
            }            
        }

        

        private void InvokingEndpoint(MethodInfo method, Type path)
        {
            method.Invoke(Activator.CreateInstance(path, _request, _response), null);
        }

        private Type GetEndpointType()
        {
            var endpointType = Assembly.GetExecutingAssembly().GetTypes().Where(type => type.GetCustomAttribute<EndpointAttribute>()?.Path == _request.GetValidEndpoint()).Single();
            return endpointType;
        }

        private MethodInfo GetEndpointMethodInfo(Type pathType)
        {
            var methodInfo = pathType.GetMethods().Where(method => method.GetCustomAttribute<MethodAttribute>()?.Method == _request.Method).Single();
            return methodInfo;
        }
    }
}
