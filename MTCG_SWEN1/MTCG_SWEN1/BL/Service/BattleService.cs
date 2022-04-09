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
    public class BattleService
    {
        
        public static Card GetRandomDeckCards(List<Card> cards)
        {
            Random random = new();
            var index = random.Next(cards.Count);
            return cards[index];
        }

        public static void UpdatesUserStatsAfterWin(User win, User loss)
        {
            List<User> users = CalculateUserStatsAfterWin(win, loss);
           
            UserDAL userTABLE = new();
            userTABLE.UpdateBattleStats(users);
        }

        public static List<User> CalculateUserStatsAfterWin(User win, User loss)
        {
            win.ELO += 3;
            win.Games += 1;
            win.Wins += 1;

            loss.ELO -= 5;
            loss.Games += 1;
            loss.Losses += 1;

            List<User> users = new();
            users.Add(win);
            users.Add(loss);

            return users;
        }

        public static void UpdatesUserStatsAfterDraw(User user1, User user2)
        {
            List<User> users = CalculatesUserStatsAfterDraw(user1, user2);

            UserDAL userTABLE = new();
            userTABLE.UpdateBattleStats(users);
        }

        public static List<User> CalculatesUserStatsAfterDraw(User user1, User user2)
        {
            user1.Games += 1;
            user2.Games += 1;

            List<User> users = new();
            users.Add(user1);
            users.Add(user2);

            return users;
        }

        public static List<Card> GetDeckCards(Guid userID)
        {
            DeckDAL deckTABLE = new();
            var deckID = deckTABLE.GetDeckID(userID);
            return deckTABLE.GetDeckCards(deckID);
        }

        public static List<Double> CalculateDamage(Card uCard1, Card uCard2)
        {
            double dmgCard1 = uCard1.Damage;
            double dmgCard2 = uCard2.Damage;

            if (uCard1.CardType == "Spell" && uCard2.CardType == "Spell")
            {
                if (uCard1.ElementType == "Regular" && uCard2.ElementType == "Water" ||
                    uCard1.ElementType == "Water" && uCard2.ElementType == "Fire" ||
                    uCard1.ElementType == "Fire" && uCard2.ElementType == "Regular")
                {
                    dmgCard1 *= 2;
                    dmgCard2 /= 2;
                }
                else if (uCard1.ElementType == "Water" && uCard2.ElementType == "Regular" ||
                         uCard1.ElementType == "Fire" && uCard2.ElementType == "Water" ||
                         uCard1.ElementType == "Regular" && uCard2.ElementType == "Fire")
                {
                    dmgCard1 /= 2;
                    dmgCard2 *= 2;
                }


            }
            // No impact from spell effects in fight between monsters, only history effect.
            else if (uCard1.CardType == "Monster" && uCard2.CardType == "Monster")
            {
                if (uCard1.Name.Contains("Goblin") && uCard2.Name.Contains("Dragon"))
                    dmgCard1 = 0.0;
                else if (uCard1.Name.Contains("Dragon") && uCard2.Name.Contains("Goblin"))
                    dmgCard2 = 0.0;
                else if (uCard1.Name.Contains("Ork") && uCard2.Name.Contains("Wizzard"))
                    dmgCard1 = 0.0;
                else if (uCard1.Name.Contains("Wizzard") && uCard2.Name.Contains("Ork"))
                    dmgCard2 = 0.0;
                else if (uCard1.Name.Contains("FireElve") && uCard2.Name.Contains("Dragon"))
                    dmgCard2 = 0.0;
                else if (uCard1.Name.Contains("Dragon") && uCard2.Name.Contains("FireElve"))
                    dmgCard1 = 0.0;


            }
            else if (uCard1.CardType == "Monster" && uCard2.CardType == "Spell")
            {
                if (uCard1.Name.Contains("Kraken"))
                    dmgCard2 = 0.0;
                else if (uCard1.Name.Contains("Knight") && (uCard2.CardType == "Spell" && uCard2.ElementType == "Water"))
                    dmgCard1 = 0.0;
                else if (uCard1.ElementType == "Regular" && uCard2.ElementType == "Water" ||
                         uCard1.ElementType == "Water" && uCard2.ElementType == "Fire" ||
                         uCard1.ElementType == "Fire" && uCard2.ElementType == "Regular")
                {
                    dmgCard1 *= 2;
                    dmgCard2 /= 2;
                }
                else if (uCard1.ElementType == "Water" && uCard2.ElementType == "Regular" ||
                         uCard1.ElementType == "Fire" && uCard2.ElementType == "Water" ||
                         uCard1.ElementType == "Regular" && uCard2.ElementType == "Fire")
                {
                    dmgCard1 /= 2;
                    dmgCard2 *= 2;
                }

            }
            else if (uCard1.CardType == "Spell" && uCard2.CardType == "Monster")
            {
                if (uCard2.Name.Contains("Kraken"))
                    dmgCard1 = 0.0;
                else if ((uCard1.CardType == "Spell" && uCard1.ElementType == "Water") && uCard2.Name.Contains("Knight"))
                    dmgCard2 = 0.0;
                else if (uCard1.ElementType == "Regular" && uCard2.ElementType == "Water" ||
                         uCard1.ElementType == "Water" && uCard2.ElementType == "Fire" ||
                         uCard1.ElementType == "Fire" && uCard2.ElementType == "Regular")
                {
                    dmgCard1 *= 2;
                    dmgCard2 /= 2;
                }
                else if (uCard1.ElementType == "Water" && uCard2.ElementType == "Regular" ||
                         uCard1.ElementType == "Fire" && uCard2.ElementType == "Water" ||
                         uCard1.ElementType == "Regular" && uCard2.ElementType == "Fire")
                {
                    dmgCard1 /= 2;
                    dmgCard2 *= 2;
                }
            }
            
            List<double> calculatedDamages = new();
            calculatedDamages.Add(dmgCard1);
            calculatedDamages.Add(dmgCard2);

            return calculatedDamages;
        }

    }
}
