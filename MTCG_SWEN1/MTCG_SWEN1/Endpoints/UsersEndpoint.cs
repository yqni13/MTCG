using MTCG_SWEN1.Endpoints.Attributes;
using MTCG_SWEN1.HTTP;
using MTCG_SWEN1.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace MTCG_SWEN1.Endpoints
{
    [Endpoint("/users")]
    class UsersEndpoint
    {
        private HttpRequest _request;
        private HttpResponse _response;

        public UsersEndpoint(HttpRequest request, HttpResponse response)
        {
            _request = request;
            _response = response;
        }

        [Method("GET")]
        public void GetUsers()
        {
            try
            {
                _response.StatusMessage = EHttpStatusMessages.OK200.GetDescription();
                _response.Body = "Demo content for /users GET";
            }
            catch (Exception err)
            {
                Console.WriteLine(err.Message);
                _response.Body = "Error for /user GET";
                _response.StatusMessage = EHttpStatusMessages.NotFound404.GetDescription();
            }
            _response.Send();
        }

        [Method("POST")]
        public void Registration()
        {
            try
            {
                // Fill user as model with credentials from request.
                User user = JsonSerializer.Deserialize<User>(_request.Body);
                if(user.Username == null || user.Username == "") 
                {
                    _response.StatusMessage = EHttpStatusMessages.NotAcceptable406.GetDescription();
                    _response.Body = "Content insufficient => invalid credentials";
                    _response.Send();
                    return;
                }

                // Call Service and catch Exceptions from Service or DB?
            }
            catch (Exception err)
            {
                Console.WriteLine(err.Message);
                _response.Body = "Error for /user POST";
                _response.StatusMessage = EHttpStatusMessages.BadRequest400.GetDescription();                
            }

            // Fill body and statusmsg of response.
            _response.Body = "User successfully registered.";
            _response.StatusMessage = EHttpStatusMessages.OK200.GetDescription();
            _response.Send();
        }

        [Method("PUT")]
        public void PutUsers()
        {
            try
            {
                _response.StatusMessage = EHttpStatusMessages.OK200.GetDescription();
                _response.Body = "Demo content for /users PUT";
            }
            catch (Exception err)
            {
                Console.WriteLine(err.Message);
                _response.Body = "Error for /user PUT";
                _response.StatusMessage = EHttpStatusMessages.NotFound404.GetDescription();
            }
            _response.Send();
        }
    }
}
