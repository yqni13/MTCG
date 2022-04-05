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
    class DeckService
    {
        public static List<Card> GetDeck(string token)
        {
            DeckDAL deckTABLE = new();
            SessionsDAL sessionTABLE = new();
            int userID = sessionTABLE.GetUserIDByToken(token);
            return deckTABLE.GetDeckCards(userID);
        }

        public static bool IsListEmpty(List<Card> deckCards)
        {            
            return !deckCards.Any();
        }

        public static void AddDeck(int userID, List<String> chosenCards)
        {
            //get user
            //get id of cards of user
            //add the chosen cards from user to deck
            UserDAL userTABLE = new();
            User user = userTABLE.GetUserByID(userID);
            DeckDAL deckTABLE = new();            
            deckTABLE.AddDeckCards(chosenCards, userID, user.Username);


        }

        public static Guid CreateNewDeckID()
        {
            return new Guid();
        }

        

        
    }
}
