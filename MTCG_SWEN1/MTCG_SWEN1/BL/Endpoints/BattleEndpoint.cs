using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MTCG_SWEN1.BL.Models;
using MTCG_SWEN1.BL.Service;
using MTCG_SWEN1.Endpoints.Attributes;
using MTCG_SWEN1.HTTP;
using MTCG_SWEN1.Models;
using Newtonsoft.Json;

namespace MTCG_SWEN1.Endpoints
{
    [Endpoint("/battles")]
    class BattleEndpoint
    {
        private HttpRequest _request;
        private HttpResponse _response;

        public BattleEndpoint(HttpRequest request, HttpResponse response)
        {
            _request = request;
            _response = response;
        }

        [Method("POST")]
        public void ExecuteMTCGBattle()
        {
            try
            {
                if (!_request.Headers.ContainsKey("Authorization"))
                {
                    Console.WriteLine($"{DateTime.UtcNow}, No token for authentication of user found.");
                    string jsonError = JsonConvert.SerializeObject("Error no token for authentication found.");
                    _response.SendWithHeaders(jsonError, EHttpStatusMessages.Unauthorized401.GetDescription());
                    return;
                }

                if (!UserService.CheckIfLoggedIn(_request.Headers["Authorization"]))
                {
                    Console.WriteLine($"{DateTime.UtcNow}, User is not logged in.");
                    string jsonError = JsonConvert.SerializeObject("User not logged in.");
                    _response.SendWithHeaders(jsonError, EHttpStatusMessages.Forbidden403.GetDescription());
                    return;
                }

                string token = _request.Headers["Authorization"];

                User user = UserService.GetUser(token);
                BattleRegistration.Instance.RequestBattle(user);

            }
            catch (Exception err)
            {
                Console.WriteLine($"{DateTime.UtcNow}, BattlesEndpoint POST error: {err.Message}");
                string jsonException = JsonConvert.SerializeObject("Error for POST/battles.");
                _response.SendWithHeaders(jsonException, EHttpStatusMessages.BadRequest400.GetDescription());
            }

            Console.WriteLine($"{DateTime.UtcNow}, Battle finished and LOG saved as .txt file");
            string json = JsonConvert.SerializeObject("Battle finished. Log was created as .txt file -> '/BattleLogs'.");
            _response.SendWithHeaders(json, EHttpStatusMessages.OK200.GetDescription());
        }
    }
}
