using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MTCG_SWEN1.BL.Service;
using MTCG_SWEN1.DB.DAL;
using MTCG_SWEN1.Endpoints.Attributes;
using MTCG_SWEN1.HTTP;
using MTCG_SWEN1.Models.Cards;
using Newtonsoft.Json;

namespace MTCG_SWEN1.Endpoints
{
    [Endpoint("/deck")]
    class DeckEndpoint
    {
        private HttpRequest _request;
        private HttpResponse _response;

        public DeckEndpoint(HttpRequest request, HttpResponse response)
        {
            _request = request;
            _response = response;
        }

        [Method("GET")]
        public void GetDeckCards()
        {
            List<Card> cards = new();
            string json;
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

                cards = DeckService.GetDeck(_request.Headers["Authorization"]);
                if(DeckService.IsListEmpty(cards))
                {
                    Console.WriteLine($"{DateTime.UtcNow}, No deck for user existing.");
                    string jsonError = JsonConvert.SerializeObject("No deck for user existing.");
                    _response.SendWithHeaders(jsonError, EHttpStatusMessages.NotFound404.GetDescription());
                    return;
                }
            }
            catch (Exception err)
            {
                Console.WriteLine($"{DateTime.UtcNow}, DeckEndpoint GET error: {err.Message}");
                string jsonException = JsonConvert.SerializeObject("Error for GET/deck.");
                _response.SendWithHeaders(jsonException, EHttpStatusMessages.BadRequest400.GetDescription());
            }

            if (_request.EndpointParameters.ContainsKey("format"))
            {
                List<String> plainCardIDs = DeckService.ConvertToPlainOutput(cards);
                json = JsonConvert.SerializeObject(plainCardIDs);
                Console.WriteLine($"{DateTime.UtcNow}, Deck listed only IDs as string instead Guid.");
                _response.SendWithHeaders(json, EHttpStatusMessages.OK200.GetDescription());
                return;
            }
                
            Console.WriteLine($"{DateTime.UtcNow}, Deck with content successfully listed for user.");
            json = JsonConvert.SerializeObject(cards);
            _response.SendWithHeaders(json, EHttpStatusMessages.OK200.GetDescription());
        }        

        [Method("PUT")]
        public void SetDeckCards()
        {            
            List<String> cardIDs = new();            
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

                SessionsDAL sessionTABLE = new();
                string token = _request.Headers["Authorization"];
                Guid userID = sessionTABLE.GetUserIDByToken(token);
                cardIDs = JsonConvert.DeserializeObject<List<String>>(_request.Body);
                List<Card> cards = DeckService.PrepareCards(cardIDs);
                if(cards.Count != 4)
                {
                    Console.WriteLine($"{DateTime.UtcNow}, User needs to choose 4 cards.");
                    string jsonError = JsonConvert.SerializeObject("User needs to choose 4 cards.");
                    _response.SendWithHeaders(jsonError, EHttpStatusMessages.NotAcceptable406.GetDescription());                   
                    return;
                }
                else if (CardService.CheckIfUserOwnChosenCards(cardIDs, token))
                {
                    DeckService.AddDeck(userID, cards);
                }
                else
                {      
                    List<Card> cardsOld = DeckService.GetDeck(_request.Headers["Authorization"]);
                    string jsonError = JsonConvert.SerializeObject(cardsOld);
                    Console.WriteLine($"{DateTime.UtcNow}, Not all cards are owned by user.");
                    _response.SendWithHeaders(jsonError, EHttpStatusMessages.NotFound404.GetDescription());
                    return;
                }
            }
            catch (Exception err)
            {
                Console.WriteLine($"{DateTime.UtcNow}, DeckEndpoint PUT error: {err.Message}");
                string jsonException = JsonConvert.SerializeObject("Error for PUT/deck.");
                _response.SendWithHeaders(jsonException, EHttpStatusMessages.BadRequest400.GetDescription());
            }

            Console.WriteLine($"{DateTime.UtcNow}, Card added to users deck.");
            string json = JsonConvert.SerializeObject("Card added to users deck.");
            _response.SendWithHeaders(json, EHttpStatusMessages.OK200.GetDescription());
        }
    }
}
