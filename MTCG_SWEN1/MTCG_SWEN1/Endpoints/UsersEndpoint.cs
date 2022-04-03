using MTCG_SWEN1.BL.Service;
using MTCG_SWEN1.Endpoints.Attributes;
using MTCG_SWEN1.HTTP;
using MTCG_SWEN1.Models;
using Newtonsoft.Json;
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
                //user = JsonSerializer.Deserialize<User>(_request.Body);
                var credentials = JsonConvert.DeserializeObject<Dictionary<string, string>>(_request.Body);

                if(credentials["Username"] == null || credentials["Username"] == "") 
                {
                    _response.StatusMessage = EHttpStatusMessages.NotAcceptable406.GetDescription();
                    _response.Body = "Content insufficient => invalid credentials";
                    _response.Send();
                    return;
                }

                // Call Service and catch Exceptions from Service or DB?
                if (!UserService.Register(credentials))
                {
                    Console.WriteLine($"{DateTime.UtcNow}, User does already exist in DB.");
                    _response.StatusMessage = EHttpStatusMessages.BadRequest400.GetDescription();
                    _response.Body = "User already exists.";
                    _response.Send();
                    return;
                }
            }            
            catch (Exception err)
            {
                Console.WriteLine($"UserEndpoint error: {err.Message}");

                _response.Body = "Error for /user POST";
                _response.StatusMessage = EHttpStatusMessages.BadRequest400.GetDescription();
                _response.Send();
                return;
            }

            // Fill body and statusmsg of response.
            Console.WriteLine($"{DateTime.UtcNow}, New User added in DB.");
            _response.Body = $"User successfully registered.";
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
