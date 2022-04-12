using MTCG_SWEN1.BL.Service;
using MTCG_SWEN1.DB.DAL;
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
            User user = new();
            try
            {                
                string userParameter = _request.PathParameter;
                string token = _request.Headers["Authorization"];
                
                UserService.CheckIfUserExists(userParameter);                
                if(!UserService.CheckIfLoggedIn(token))
                {
                    Console.WriteLine($"{DateTime.UtcNow}, User is not logged in.");
                    string jsonError = JsonConvert.SerializeObject("User not logged in.");
                    _response.SendWithHeaders(jsonError, EHttpStatusMessages.Forbidden403.GetDescription());                    
                    return;
                }
                user = UserService.GetUserInformation(token);
                if(userParameter != user.Username)
                {
                    Console.WriteLine($"{DateTime.UtcNow}, URL error, wrong user parameter.");
                    string jsonError = JsonConvert.SerializeObject("Wrong user.");
                    _response.SendWithHeaders(jsonError, EHttpStatusMessages.Unauthorized401.GetDescription());                    
                    return;
                }

                user = UserService.GetUserInformation(token);
            }
            catch (Exception err)
            {
                Console.WriteLine($"{ DateTime.UtcNow}, UserEndpoint GET error: {err.Message}");
                string jsonException = JsonConvert.SerializeObject("Error for GET/users.");
                _response.SendWithHeaders(jsonException, EHttpStatusMessages.BadRequest400.GetDescription());   
            }

            Console.WriteLine($"{DateTime.UtcNow}, User attributes successfully listed.");
            string json = JsonConvert.SerializeObject(user);
            _response.SendWithHeaders(json, EHttpStatusMessages.OK200.GetDescription());
        }

        [Method("POST")]
        public void Registration()
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
                
                if (!UserService.RegisterService(credentials))
                {
                    Console.WriteLine($"{DateTime.UtcNow}, User does already exist in DB.");
                    string jsonError = JsonConvert.SerializeObject("User already exists.");
                    _response.SendWithHeaders(jsonError, EHttpStatusMessages.BadRequest400.GetDescription());                    
                    return;
                }
            }            
            catch (Exception err)
            {                
                Console.WriteLine($"{DateTime.UtcNow}, UserEndpoint POST error: {err.Message}");
                string jsonException = JsonConvert.SerializeObject("Error for POST/users.");
                _response.SendWithHeaders(jsonException, EHttpStatusMessages.BadRequest400.GetDescription());
            }
            
            Console.WriteLine($"{DateTime.UtcNow}, New User added in DB.");
            string json = JsonConvert.SerializeObject("User registration successful.");
            _response.SendWithHeaders(json, EHttpStatusMessages.OK200.GetDescription());            
        }

        [Method("PUT")]
        public void UpdateUsers()
        {
            Dictionary<string, string> userEdit = new();
            User user = new();
            try
            {
                string userParameter = _request.PathParameter;
                string token = _request.Headers["Authorization"];
                
                UserService.CheckIfUserExists(userParameter);
                if (!UserService.CheckIfLoggedIn(token))
                {
                    Console.WriteLine($"{DateTime.UtcNow}, User is not logged in.");
                    string jsonError = JsonConvert.SerializeObject("User not logged in.");
                    _response.SendWithHeaders(jsonError, EHttpStatusMessages.Unauthorized401.GetDescription());
                    return;
                }

                user = UserService.GetUserInformation(token);
                if (userParameter != user.Username)
                {
                    Console.WriteLine($"{DateTime.UtcNow}, URL error, wrong user parameter.");
                    string jsonError = JsonConvert.SerializeObject("Wrong user.");
                    _response.SendWithHeaders(jsonError, EHttpStatusMessages.Unauthorized401.GetDescription());
                    return;
                }

                userEdit = JsonConvert.DeserializeObject<Dictionary<string, string>>(_request.Body);
                UserService.EditUserInformation(userEdit, token);
            }
            catch (Exception err)
            {
                Console.WriteLine($"{DateTime.UtcNow}, UserEndpoint PUT error: {err.Message}");
                string jsonException = JsonConvert.SerializeObject("Error for PUT/users.");
                _response.SendWithHeaders(jsonException, EHttpStatusMessages.BadRequest400.GetDescription());                
            }
            
            Console.WriteLine($"{DateTime.UtcNow}, User information successfully updated.");
            string json = JsonConvert.SerializeObject("User successfully updated.");
            _response.SendWithHeaders(json, EHttpStatusMessages.OK200.GetDescription());
        }
    }
}
