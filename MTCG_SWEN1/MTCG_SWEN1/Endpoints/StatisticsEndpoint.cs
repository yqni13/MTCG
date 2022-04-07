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
        public void StatsGet()
        {
            User user = new();
            Dictionary<string, string> statistics = new();
            try
            {
                if (!_request.Headers.ContainsKey("Authorization"))
                {
                    _response.StatusMessage = EHttpStatusMessages.Unauthorized401.GetDescription();
                    _response.Body = "Error no token for authentication found.";
                    _response.Send();
                    return;
                }

                if (!UserService.CheckIfLoggedIn(_request.Headers["Authorization"]))
                {
                    _response.StatusMessage = EHttpStatusMessages.Forbidden403.GetDescription();
                    _response.Body = "User not logged in.";
                    _response.Send();
                    return;
                }

                user = StatsService.GetUserStats(_request.Headers["Authorization"]);
                statistics = StatsService.PrepareStatsDisplay(user);
            }
            catch (Exception err)
            {
                Console.WriteLine(err.Message);
                _response.StatusMessage = EHttpStatusMessages.NotFound404.GetDescription();
                _response.Body = "Error for GET/stats.";
            }

            string json = JsonConvert.SerializeObject(statistics);

            Console.WriteLine($"{DateTime.UtcNow}, Statistics successfully listed for user.");
            _response.SendWithHeaders(json, EHttpStatusMessages.OK200.GetDescription());
            
        }
    }
}
