using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MTCG_SWEN1.BL.Service;
using MTCG_SWEN1.DB.DAL;
using MTCG_SWEN1.Endpoints.Attributes;
using MTCG_SWEN1.HTTP;
using MTCG_SWEN1.Models;
using MTCG_SWEN1.Models.Cards;
using Newtonsoft.Json;

namespace MTCG_SWEN1.Endpoints
{
    [Endpoint("/packages")]
    class PackagesEndpoint
    {
        private HttpRequest _request;
        private HttpResponse _response;

        public PackagesEndpoint(HttpRequest request, HttpResponse response)
        {
            _request = request;
            _response = response;
        }

        [Method("POST")]
        public void HandlePackages()
        {
            try
            {
                User user = new();
                if (!_request.Headers.ContainsKey("Authorization"))
                {
                    Console.WriteLine($"{DateTime.UtcNow}, No token for authentication of user found.");
                    string jsonError = JsonConvert.SerializeObject("Error no token for authentication found.");
                    _response.SendWithHeaders(jsonError, EHttpStatusMessages.Unauthorized401.GetDescription());
                    return;
                }                
                                
                if (!UserService.CheckIfLoggedIn(_request.Headers["Authorization"]))
                {
                    Console.WriteLine($"{DateTime.UtcNow}, Admin is not logged in.");
                    string jsonError = JsonConvert.SerializeObject("Admin not logged in.");
                    _response.SendWithHeaders(jsonError, EHttpStatusMessages.Forbidden403.GetDescription());
                    return;
                }

                // Create new package.
                if (_request.Headers["Authorization"] == "Basic admin-mtcgToken")
                {
                    List<Card> cardList = new();
                    var cards = JsonConvert.DeserializeObject<List<Card>>(_request.Body);

                    cardList = PackageService.PrepareNewPackage(cards);                
                    PackageService.AddPackageToPurchase(cardList);
                }
                // Purchase package.
                else
                {
                    string token = _request.Headers["Authorization"];                    
                                     
                    user = StatsService.GetUserStats(token);
                    CardsDAL cardTABLE = new();                                    
                    List<Card> cardsToPurchase = cardTABLE.GetMaxNumberOfCardsToPurchase();
                    Console.WriteLine($"card counter Endpoint: {cardsToPurchase.Count}");

                    if(cardsToPurchase.Count < 5)
                    {
                        Console.WriteLine($"{DateTime.UtcNow}, Not enough cards to purchase package.");
                        string jsonError = JsonConvert.SerializeObject("Not enough cards to purchase package.");
                        _response.SendWithHeaders(jsonError, EHttpStatusMessages.Forbidden403.GetDescription());
                        return;
                    }                    
                    else if(user.Coins > 4)
                        CardService.PurchasePackagesByUser(user.Username);
                    else
                    {
                        Console.WriteLine($"{DateTime.UtcNow}, User has less coins than necessary.");
                        string jsonError = JsonConvert.SerializeObject("User has less coins than necessary.");
                        _response.SendWithHeaders(jsonError, EHttpStatusMessages.Forbidden403.GetDescription());
                        return;
                    }                        
                }
            }
            catch (Exception err)
            {
                Console.WriteLine($"{DateTime.UtcNow}, PackagesEndpoint POST error: {err.Message}");
                string jsonException = JsonConvert.SerializeObject("Error for POST/packages.");
                _response.SendWithHeaders(jsonException, EHttpStatusMessages.BadRequest400.GetDescription());
            }
            
            Console.WriteLine($"{DateTime.UtcNow}, New package added in DB.");
            string json = JsonConvert.SerializeObject("New package added.");
            _response.SendWithHeaders(json, EHttpStatusMessages.OK200.GetDescription());
        }
    }
}
