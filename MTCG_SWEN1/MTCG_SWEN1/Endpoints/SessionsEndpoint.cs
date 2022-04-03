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
                    _response.StatusMessage = EHttpStatusMessages.NotAcceptable406.GetDescription();
                    _response.Body = "Invalid credentials.";
                    _response.Send();
                    return;
                }
                UserDAL userTABLE = new();
                SessionsDAL sessionTABLE = new();
                User user = new();
                userTABLE.ReadSpecific(credentials["Username"], user);

                if(UserService.CheckIfSessionExisting(user, sessionTABLE))
                {
                    Console.WriteLine($"{DateTime.UtcNow}, Session already existing for user.");
                    _response.StatusMessage = EHttpStatusMessages.BadRequest400.GetDescription();
                    _response.Body = "Session for user already exists.";
                    _response.Send();
                    return;
                }

                UserService.LoginService(user, credentials["Password"], sessionTABLE);
            }
            catch (Exception err)
            {
                Console.WriteLine(err.Message);
                _response.Body = "Error for POST/sessions.";
                _response.StatusMessage = EHttpStatusMessages.NotFound404.GetDescription();
                _response.Send();
            }

            // Fill body and statusmsg of response and display status on backend console.
            Console.WriteLine($"{DateTime.UtcNow}, User login was successful.");
            _response.StatusMessage = EHttpStatusMessages.OK200.GetDescription();
            _response.Body = "User login successful.";
            _response.Send();
        }
    }
}
