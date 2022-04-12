using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MTCG_SWEN1.BL.Service;
using MTCG_SWEN1.Endpoints.Attributes;
using MTCG_SWEN1.HTTP;
using MTCG_SWEN1.Models.Cards;
using Newtonsoft.Json;

namespace MTCG_SWEN1.Endpoints
{
    [Endpoint("/cards")]
    class CardsEndpoint
    {
        private HttpRequest _request;
        private HttpResponse _response;

        public CardsEndpoint(HttpRequest request, HttpResponse response)
        {
            _request = request;
            _response = response;
        }

        [Method("GET")]
        public void GetCards()
        {
            List<Card> cards = new();
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

                cards = CardService.ShowAllCardsOfUser(_request.Headers["Authorization"]);
            }
            catch (Exception err)
            {
                Console.WriteLine($"{DateTime.UtcNow}, CardsEndpoint GET error: {err.Message}");
                string jsonException = JsonConvert.SerializeObject("Error for GET/cards.");
                _response.SendWithHeaders(jsonException, EHttpStatusMessages.BadRequest400.GetDescription());
            }
            
            Console.WriteLine($"{DateTime.UtcNow}, Cards successfully listed for user.");
            string json = JsonConvert.SerializeObject(cards);
            _response.SendWithHeaders(json, EHttpStatusMessages.OK200.GetDescription());            
        }
    }
}
