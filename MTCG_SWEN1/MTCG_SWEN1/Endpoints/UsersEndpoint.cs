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
                    _response.StatusMessage = EHttpStatusMessages.Unauthorized401.GetDescription();
                    _response.Body = "User not logged in.";
                    _response.Send();
                    return;
                }
                user = UserService.GetUserInformation(token);
                if(userParameter != user.Username)
                {
                    _response.StatusMessage = EHttpStatusMessages.Unauthorized401.GetDescription();
                    _response.Body = "Wrong user.";
                    _response.Send();
                    return;
                }

                user = UserService.GetUserInformation(token);
            }
            catch (Exception err)
            {
                Console.WriteLine(err.Message);
                _response.StatusMessage = EHttpStatusMessages.NotFound404.GetDescription();
                _response.Body = "Error for GET/users.";
                return;
            }

            string json = JsonConvert.SerializeObject(user);
            Console.WriteLine($"{DateTime.UtcNow}, User attributes successfully listed.");
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
                    _response.StatusMessage = EHttpStatusMessages.NotAcceptable406.GetDescription();
                    _response.Body = "Invalid credentials.";
                    _response.Send();
                    return;
                }
                
                if (!UserService.RegisterService(credentials))
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
                _response.StatusMessage = EHttpStatusMessages.BadRequest400.GetDescription();
                _response.Body = "Error for POST/users.";
                _response.Send();
                return;
            }
            
            Console.WriteLine($"{DateTime.UtcNow}, New User added in DB.");
            _response.StatusMessage = EHttpStatusMessages.OK200.GetDescription();
            _response.Body = $"User registration successful.";
            _response.Send();
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
                    _response.StatusMessage = EHttpStatusMessages.Unauthorized401.GetDescription();
                    _response.Body = "User not logged in.";
                    _response.Send();
                    return;
                }

                user = UserService.GetUserInformation(token);
                if (userParameter != user.Username)
                {
                    _response.StatusMessage = EHttpStatusMessages.Unauthorized401.GetDescription();
                    _response.Body = "Wrong user.";
                    _response.Send();
                    return;
                }

                userEdit = JsonConvert.DeserializeObject<Dictionary<string, string>>(_request.Body);
                UserService.EditUserInformation(userEdit, token);
            }
            catch (Exception err)
            {
                Console.WriteLine(err.Message);
                _response.StatusMessage = EHttpStatusMessages.NotFound404.GetDescription();
                _response.Body = "Error for PUT/users.";
            }

            Console.WriteLine($"{DateTime.UtcNow}, User information successfully updated.");
            _response.StatusMessage = EHttpStatusMessages.OK200.GetDescription();
            _response.Body = "User successfully updated.";
            _response.Send();
        }
    }
}
