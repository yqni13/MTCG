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
        public void BattlePost()
        {
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

                string token = _request.Headers["Authorization"];

                User user = UserService.GetUser(token);
                BattleRegistration.Instance.RequestBattle(user);

            }
            catch (Exception err)
            {
                Console.WriteLine(err.Message);
                _response.Body = "Error for POST/battle.";
                _response.StatusMessage = EHttpStatusMessages.NotFound404.GetDescription();
            }

            _response.StatusMessage = EHttpStatusMessages.OK200.GetDescription();
            _response.Body = "Battle finished. Log was created as .txt file.";
            _response.Send();
        }
    }
}
