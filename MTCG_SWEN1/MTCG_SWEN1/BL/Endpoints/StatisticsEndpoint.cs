using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;
using MTCG_SWEN1.BL.Service;
using MTCG_SWEN1.Endpoints.Attributes;
using MTCG_SWEN1.HTTP;
using MTCG_SWEN1.Models;
using Newtonsoft.Json;

namespace MTCG_SWEN1.Endpoints
{
    [Endpoint("/stats")]
    class StatisticsEndpoint
    {
        private HttpRequest _request;
        private HttpResponse _response;

        public StatisticsEndpoint(HttpRequest request, HttpResponse response)
        {
            _request = request;
            _response = response;
        }

        [Method("GET")]
        public void GetUserStatistics()
        {
            User user = new();
            Dictionary<string, string> statistics = new();
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

                user = StatsService.GetUserStats(_request.Headers["Authorization"]);
                statistics = StatsService.PrepareStatsDisplay(user);
            }
            catch (Exception err)
            {
                Console.WriteLine($"{DateTime.UtcNow}, StatisticsEndpoint GET error: {err.Message}");
                string jsonException = JsonConvert.SerializeObject("Error for GET/stats.");
                _response.SendWithHeaders(jsonException, EHttpStatusMessages.BadRequest400.GetDescription());
            }

            Console.WriteLine($"{DateTime.UtcNow}, Statistics successfully listed for user.");
            string json = JsonConvert.SerializeObject(statistics);
            _response.SendWithHeaders(json, EHttpStatusMessages.OK200.GetDescription());
            
        }
    }
}
