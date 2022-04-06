using MTCG_SWEN1.Models.Cards;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MTCG_SWEN1.Models
{
    class Decks
    {
        public Guid ID { get; set; }
        public Guid User { get; set; }        
        public Guid Card { get; set; }

        public Decks(Guid deckID, Guid user, Guid card)
        {
            ID = deckID;
            User = user;
            Card = card;
        }
    }
}
