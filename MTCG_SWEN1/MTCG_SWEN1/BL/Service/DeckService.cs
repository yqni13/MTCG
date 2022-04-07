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
    public class DeckService
    {
        public static List<Card> GetDeck(string token)
        {
            DeckDAL deckTABLE = new();
            SessionsDAL sessionTABLE = new();
            Guid userID = sessionTABLE.GetUserIDByToken(token);
            int deckID = deckTABLE.GetDeckID(userID);
            return deckTABLE.GetDeckCards(deckID);
        }

        public static bool IsListEmpty(List<Card> deckCards)
        {            
            return !deckCards.Any();
        }

        public static void AddDeck(Guid userID, List<Card> chosenCards)
        {            
            UserDAL userTABLE = new();
            User user = userTABLE.GetUserByID(userID);
            DeckDAL deckTABLE = new();
            deckTABLE.CreateNewDeck(user.Id);
            deckTABLE.AddDeckCards(chosenCards, user.Id, deckTABLE.GetDeckID(user.Id));
        }

        public static Guid CreateNewDeckID()
        {
            return Guid.NewGuid();
        }

        public static List<Card> PrepareCards(List<String> cardIDs)
        {
            List<Card> cards = new();
            foreach(var card in cardIDs)
                cards.Add(new Card(Guid.Parse(card)));

            return cards;
        }

        public static List<String> ConvertToPlainOutput(List<Card> cards)
        {
            List<String> cardIDs = new();
            foreach (var card in cards)
                cardIDs.Add(card.ID.ToString());

            return cardIDs;
        }
    }
}
