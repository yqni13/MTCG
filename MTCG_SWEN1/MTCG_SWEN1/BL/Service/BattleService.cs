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
    class BattleService
    {
        
        public static Card GetRandomDeckCards(List<Card> cards)
        {
            Random random = new();
            var index = random.Next(cards.Count);
            return cards[index];
        }

        public static void HandleUserStatsAfterWin(User win, User loss)
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

            UserDAL userTABLE = new();
            userTABLE.UpdateBattleStats(users);
        }

        public static void HandleUserStatsAfterDraw(User user1, User user2)
        {
            user1.Games += 1;
            user2.Games += 1;

            List<User> users = new();
            users.Add(user1);
            users.Add(user2);

            UserDAL userTABLE = new();
            userTABLE.UpdateBattleStats(users);
        }

        public static List<Card> GetDeckCards(Guid userID)
        {
            DeckDAL deckTABLE = new();
            var deckID = deckTABLE.GetDeckID(userID);
            return deckTABLE.GetDeckCards(deckID);
        }
    }
}
