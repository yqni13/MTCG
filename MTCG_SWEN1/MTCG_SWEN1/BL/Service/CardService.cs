using MTCG_SWEN1.DB.DAL;
using MTCG_SWEN1.Models;
using MTCG_SWEN1.Models.Cards;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MTCG_SWEN1.BL.Service
{
    class CardService
    {
        

        public static void PurchasePackagesByUser(string username)
        {
            CardsDAL cardTABLE = new();
            UserDAL userTABLE = new();
            User user = new();            
            userTABLE.GetUserByUsername(username, user);            
            cardTABLE.PurchasePackage(user.Id);
        }

        public static List<Card> ShowAllCardsOfUser(string token)
        {
            SessionsDAL sessionTABLE = new();
            CardsDAL cardTABLE = new();
            
            return cardTABLE.GetAllCardsOfUser(sessionTABLE.GetUserIDByToken(token));            
        }

        public static bool CheckIfUserOwnChosenCards(List<String> cardIDs, string token)
        {
            List<Card> allCardsOfUser = ShowAllCardsOfUser(token);
            List<String> idUserCards = new();
            foreach( var card in allCardsOfUser)
            {
                idUserCards.Add(new string(card.ID.ToString()));
            }

            int existingCount = 0;
            foreach(var card in cardIDs)
            {
                if (idUserCards.Contains(card))
                    ++existingCount;
            }

            return existingCount == cardIDs.Count;                 
        }

        public static List<Card> GetNumberOfCardsToPurchase()
        {
            CardsDAL cardTABLE = new();
            return cardTABLE.GetMaxNumberOfCardsToPurchase();
        }
    }
}
