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
        public Guid Id { get; set; }
        public string Username { get; set; }
        public string Name { get; set; }
        public List<Card> Cards { get; set; } = new();

        public Decks(Guid deckID, string username, string name)
        {
            Id = deckID;
            Username = username;
            Name = name;
        }
    }
}
