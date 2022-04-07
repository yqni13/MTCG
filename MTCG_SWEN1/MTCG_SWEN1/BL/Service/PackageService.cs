using MTCG_SWEN1.DB.DAL;
using MTCG_SWEN1.HTTP;
using MTCG_SWEN1.Models.Cards;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MTCG_SWEN1.BL.Service
{
    public class PackageService
    {
        public static bool CheckForEnoughFreeCards(int cardCount)
        {
            if (cardCount < 4)
                return false;

            return true;
        }

        public static List<Card> PrepareNewPackage(List<Card> cards)
        {
            List<Card> cardPackage = new();
            
            foreach (Card card in cards)
            {
                var type = GetCardTypeFromName(card.Name);
                var element = GetCardElementFromName(card.Name);

                cardPackage.Add(new Card(
                    card.ID,
                    card.Name,
                    card.Damage,
                    card.CardType = type,
                    card.ElementType = element,
                    card.PackageTimestamp = DateTime.Now
                    ));
            }

            return cardPackage;
        }        

        public static void AddPackageToPurchase(List<Card> cards)
        {
            CardsDAL cardTABLE = new();            
            cardTABLE.AddPackage(cards);
        }

        public static string GetCardElementFromName(string description)
        {
            if (description.Contains("Fire"))
                return EElementType.FIRE.GetDescription();
            else if (description.Contains("Water"))
                return EElementType.WATER.GetDescription();
            else
                return EElementType.REGULAR.GetDescription();
        }

        public static string GetCardTypeFromName(string description)
        {
            if (description.Contains("Spell"))
                return ECardType.SPELL.GetDescription();
            else
                return ECardType.MONSTER.GetDescription();
        }        
    }
}
