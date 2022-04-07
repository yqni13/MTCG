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
                    _response.StatusMessage = EHttpStatusMessages.Unauthorized401.GetDescription();
                    _response.Body = "Error no token for authentication found.";
                    _response.Send();
                    return;
                }                
                                
                if (!UserService.CheckIfLoggedIn(_request.Headers["Authorization"]))
                {
                    _response.StatusMessage = EHttpStatusMessages.Forbidden403.GetDescription();
                    _response.Body = "Admin not logged in.";
                    _response.Send();
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
                        _response.StatusMessage = EHttpStatusMessages.Forbidden403.GetDescription();
                        _response.Body = "Not enough cards to purchase package.";
                        _response.Send();
                        return;
                    }                    
                    else if(user.Coins > 4)
                        CardService.PurchasePackagesByUser(user.Username);
                    else
                    {
                        _response.StatusMessage = EHttpStatusMessages.Forbidden403.GetDescription();
                        _response.Body = "User has less coins than necessary.";
                        _response.Send();
                        return;
                    }                        
                }
            }
            catch (Exception err)
            {
                Console.WriteLine(err.Message);
                _response.StatusMessage = EHttpStatusMessages.BadRequest400.GetDescription();
                _response.Body = "Error for POST/packages.";
                _response.Send();
            }

            Console.WriteLine($"{DateTime.UtcNow}, New package added in DB.");
            _response.StatusMessage = EHttpStatusMessages.OK200.GetDescription();
            _response.Body = "New package added.";
            _response.Send();
        }
    }
}
