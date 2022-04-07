using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace MTCG_SWEN1.Models.Cards
{
    public class Card
    {
        public Guid ID { get; set; }
        public string Name { get; set; } = "";
        public Guid StackUser { get; set; }
        public Double Damage { get; set; } = 0;
        public string CardType { get; set; }
        public string ElementType { get; set; }
        public bool ForTrade { get; set; } = false;
        //public Guid PackageNumber { get; set; }
        public DateTime PackageTimestamp { get; set; }

        public int DeckID { get; set; }

        public Card() { }

        public Card(Guid id)
        {
            ID = id;
        }

        public Card(Guid id, string name, Guid user, double damage, string type, string element)
        {
            ID = id;
            Name = name;
            StackUser = user;
            Damage = damage;
            CardType = type;
            ElementType = element;
        }

        public Card(Guid cardID, DateTime timestamp)
        {
            //StackUser = userID;
            ID = cardID;
            PackageTimestamp = timestamp;
        }


        public Card(Guid id, string name, double damage, string type, string element, DateTime timestamp)
        {
            ID = id;
            Name = name;
            Damage = damage;
            ElementType = element;
            CardType = type;
            PackageTimestamp = timestamp;
        }
            

        /*public EElementType GetElementType()
        {
            var elementNames = Enum.GetNames(typeof(EElementType));

            foreach (var type in elementNames)
            {
                if (Name == type)
                {
                    return Enum.Parse<EElementType>(type);
                }
            }

            return EElementType.NORMAL;
        }

        public ECardType GetCardType()
        {
            var cardNames = Enum.GetNames(typeof(ECardType));

            foreach (var type in cardNames)
            {
                if (Name == type)
                {
                    return Enum.Parse<ECardType>(type);
                }
            }

            return Enum.Parse<ECardType>("Ghost");
        }*/

    }
}
