using MTCG_SWEN1.Models.Cards;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MTCG_SWEN1.DB.InterfacesCRUD
{
    interface IDecks
    {
        List<Card> GetDeckCards(int deckID);        

        void CreateNewDeck(Guid userID);

        int GetDeckID(Guid userID);

        void AddDeckCards(List<Card> cards, Guid userID, int deckID);

        
    }
}
