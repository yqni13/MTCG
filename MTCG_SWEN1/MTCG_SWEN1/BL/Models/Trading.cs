using MTCG_SWEN1.Models.Cards;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MTCG_SWEN1.Models
{
    class Trading
    {
        public Guid ID { get; set; }
        public Guid User { get; set; }
        public Guid Card { get; set; }
        public int MinDamage { get; set; }
        public string RequiredType { get; set; }

        public Trading(Guid id, Guid user, Guid card, int minDmg, string requiredType)
        {
            ID = id;
            User = user;
            Card = card;
            MinDamage = minDmg;
            RequiredType = requiredType;
        }
    }
}
