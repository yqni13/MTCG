using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MTCG_SWEN1.BL.Service;
using MTCG_SWEN1.DB.DAL;
using MTCG_SWEN1.Endpoints.Attributes;
using MTCG_SWEN1.HTTP;
using MTCG_SWEN1.Models;
using Newtonsoft.Json;

namespace MTCG_SWEN1.Endpoints
{
    [Endpoint("/sessions")]
    class SessionsEndpoint
    {
        private HttpRequest _request;
        private HttpResponse _response;

        public SessionsEndpoint(HttpRequest request, HttpResponse response)
        {
            _request = request;
            _response = response;
        }

        [Method("POST")]
        public void Login()
        {
            try
            {
                var credentials = JsonConvert.DeserializeObject<Dictionary<string, string>>(_request.Body);

                if(!UserService.CheckIfCredentialsComplete(credentials["Username"], credentials["Password"]))
                {
                    Console.WriteLine($"{DateTime.UtcNow}, Credentials of user are invalid.");
                    string jsonError = JsonConvert.SerializeObject("Invalid credentials.");
                    _response.SendWithHeaders(jsonError, EHttpStatusMessages.NotAcceptable406.GetDescription());
                    return;
                }
                UserDAL userTABLE = new();
                SessionsDAL sessionTABLE = new();
                User user = new();
                userTABLE.GetUserByUsername(credentials["Username"], user);

                if(UserService.CheckIfSessionExisting(user, sessionTABLE))
                {                    
                    Console.WriteLine($"{DateTime.UtcNow}, Session already existing for user.");
                    string jsonError = JsonConvert.SerializeObject("Session for user already exists.");
                    _response.SendWithHeaders(jsonError, EHttpStatusMessages.BadRequest400.GetDescription());
                    return;
                }

                UserService.LoginService(user, credentials["Password"], sessionTABLE);
            }
            catch (Exception err)
            {
                Console.WriteLine($"{DateTime.UtcNow}, SessionsEndpoint POST error: {err.Message}");
                string jsonException = JsonConvert.SerializeObject("Error for POST/sessions.");
                _response.SendWithHeaders(jsonException, EHttpStatusMessages.NotFound404.GetDescription());
            }
            
            Console.WriteLine($"{DateTime.UtcNow}, User login was successful.");
            string json = JsonConvert.SerializeObject("User login successful.");
            _response.SendWithHeaders(json, EHttpStatusMessages.OK200.GetDescription());
        }
    }
}
