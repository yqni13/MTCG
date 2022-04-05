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
        public void DeckGet()
        {
            List<Card> cards = new();
            string json;
            //try
            //{
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
            //}
            /*catch (Exception err)
            {
                Console.WriteLine(err.Message);
                _response.StatusMessage = EHttpStatusMessages.NotFound404.GetDescription();
                _response.Body = "Error for GET/deck";
            }*/

            json = JsonConvert.SerializeObject(cards);
            Console.WriteLine($"{DateTime.UtcNow}, Deck successfully listed for user.");
            _response.SendWithHeaders(json, EHttpStatusMessages.OK200.GetDescription());
        }

        [Method("POST")]
        public void DeckPost()
        {
            try
            {
                _response.StatusMessage = EHttpStatusMessages.OK200.GetDescription();
                _response.Body = "Demo content for /deck POST";
            }
            catch (Exception err)
            {
                Console.WriteLine(err.Message);
                _response.Body = "Error for /deck POST";
                _response.StatusMessage = EHttpStatusMessages.NotFound404.GetDescription();
            }
            _response.Send();
        }

        [Method("PUT")]
        public void DeckPut()
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
                int userID = sessionTABLE.GetUserIDByToken(token);
                cardIDs = JsonConvert.DeserializeObject<List<String>>(_request.Body);

                if (CardService.CheckIfUserOwnChosenCards(cardIDs, token))
                    DeckService.AddDeck(userID, cardIDs);
                else
                {
                    _response.StatusMessage = EHttpStatusMessages.NotFound404.GetDescription();
                    _response.Body = "Not all cards are owned by user.";
                    _response.Send();
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
