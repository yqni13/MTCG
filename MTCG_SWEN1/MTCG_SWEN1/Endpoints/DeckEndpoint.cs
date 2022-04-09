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

                cards = DeckService.GetDeck(_request.Headers["Authorization"]);
                if(DeckService.IsListEmpty(cards))
                {                    
                    _response.StatusMessage = EHttpStatusMessages.NotFound404.GetDescription();
                    _response.Body = "No deck for user existing.";
                    _response.Send();
                    return;
                }
            }
            catch (Exception err)
            {
                Console.WriteLine(err.Message);
                _response.StatusMessage = EHttpStatusMessages.NotFound404.GetDescription();
                _response.Body = "Error for GET/deck";
            }
            if (_request.EndpointParameters.ContainsKey("format"))
            {
                List<String> plainCardIDs = DeckService.ConvertToPlainOutput(cards);
                json = JsonConvert.SerializeObject(plainCardIDs);
                Console.WriteLine($"{DateTime.UtcNow}, Deck listed only IDs as string instead Guid.");
                _response.SendWithHeaders(json, EHttpStatusMessages.OK200.GetDescription());
                return;
            }
                
            json = JsonConvert.SerializeObject(cards);
            Console.WriteLine($"{DateTime.UtcNow}, Deck with content successfully listed for user.");
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

                SessionsDAL sessionTABLE = new();
                string token = _request.Headers["Authorization"];
                Guid userID = sessionTABLE.GetUserIDByToken(token);
                cardIDs = JsonConvert.DeserializeObject<List<String>>(_request.Body);
                List<Card> cards = DeckService.PrepareCards(cardIDs);
                if(cards.Count != 4)
                {
                    _response.StatusMessage = EHttpStatusMessages.NotAcceptable406.GetDescription();
                    _response.Body = "User needs to choose 4 cards.";
                    _response.Send();
                    return;
                }
                else if (CardService.CheckIfUserOwnChosenCards(cardIDs, token))
                {
                    DeckService.AddDeck(userID, cards);
                }
                else
                {      
                    List<Card> cardsOld = DeckService.GetDeck(_request.Headers["Authorization"]);
                    var json = JsonConvert.SerializeObject(cardsOld);
                    Console.WriteLine($"{DateTime.UtcNow}, Not all cards are owned by user.");
                    _response.SendWithHeaders(json, EHttpStatusMessages.NotFound404.GetDescription());
                    return;
                }
            }
            catch (Exception err)
            {
                Console.WriteLine(err.Message);
                _response.StatusMessage = EHttpStatusMessages.BadRequest400.GetDescription();
                _response.Body = "Error for PUT/deck.";
            }            

            Console.WriteLine($"{DateTime.UtcNow}, Card added to users deck.");
            _response.StatusMessage = EHttpStatusMessages.OK200.GetDescription();
            _response.Body = "Card added to users deck.";
            _response.Send();
        }
    }
}
