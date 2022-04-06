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
        public void ScoresGet()
        {
            Dictionary<string, int> scoreboard = new();
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

                scoreboard = ScoreService.GetScoreboard();
            }
            catch (Exception err)
            {
                Console.WriteLine($"ScoresEndpoint error: {err.Message}");
                _response.StatusMessage = EHttpStatusMessages.NotFound404.GetDescription();
                _response.Body = "Error for GET/score.";
            }

            string json = JsonConvert.SerializeObject(scoreboard, Formatting.Indented);
            Console.WriteLine($"{DateTime.UtcNow}, Scoreboard successfully listed.");
            _response.SendWithHeaders(json, EHttpStatusMessages.OK200.GetDescription());

            //_response.StatusMessage = EHttpStatusMessages.OK200.GetDescription();
            //_response.Body = "Demo content for /score GET";
            //_response.Send();
        }
    }
}
