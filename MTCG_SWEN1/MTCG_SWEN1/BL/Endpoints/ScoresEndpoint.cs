using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MTCG_SWEN1.BL.Service;
using MTCG_SWEN1.Endpoints.Attributes;
using MTCG_SWEN1.HTTP;
using Newtonsoft.Json;

namespace MTCG_SWEN1.Endpoints
{
    [Endpoint("/score")]
    class ScoresEndpoint
    {
        private HttpRequest _request;
        private HttpResponse _response;

        public ScoresEndpoint(HttpRequest request, HttpResponse response)
        {
            _request = request;
            _response = response;
        }

        [Method("GET")]
        public void GetUserScoreboard()
        {
            Dictionary<string, int> scoreboard = new();
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

                scoreboard = ScoreService.GetScoreboard();
            }
            catch (Exception err)
            {
                Console.WriteLine($"{DateTime.UtcNow}, ScoresEndpoint GET error: {err.Message}");
                string jsonException = JsonConvert.SerializeObject("Error for GET/score.");
                _response.SendWithHeaders(jsonException, EHttpStatusMessages.BadRequest400.GetDescription());
            }

            Console.WriteLine($"{DateTime.UtcNow}, Scoreboard successfully listed.");
            string json = JsonConvert.SerializeObject(scoreboard, Formatting.Indented);
            _response.SendWithHeaders(json, EHttpStatusMessages.OK200.GetDescription());
        }
    }
}
