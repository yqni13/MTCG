using MTCG_SWEN1.BL.Service;
using MTCG_SWEN1.DB.DAL;
using MTCG_SWEN1.Models;
using MTCG_SWEN1.Models.Cards;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MTCG_SWEN1.BL.Models
{

    public class Battle
    {
        public User User1 { get; set; }
        public User User2 { get; set; }
        public List<Card> UserDeck1 { get; set; }
        public List<Card> UserDeck2 { get; set; }        
        public List<String> BattleLog { get; set; } = new();        
        public int Roundnumber { get; set; } = 1;

        private readonly int _maxRoundsOfBattle = 100;
        public string LogText { get; set; } = "";


        public Battle(User user1, User user2)
        {
            User1 = user1;
            User2 = user2;            
        }

        public void StartBattle()
        {

            LogText = $"LOG|{DateTime.Now}, Battle {User1.Username} vs {User2.Username} starts, BattleRound: 1.\n";
            BattleLog.Add(LogText);
            Console.WriteLine(LogText);

            try
            {
                UserDeck1 = BattleService.GetDeckCards(User1.Id);
                UserDeck2 = BattleService.GetDeckCards(User2.Id);
            }
            catch (Exception)
            {
                throw new Exception("Deck cards not able to fetch.");
            }
            
            var result = ExecuteBattle();
            if (result == "roundlimit")
            {
                BattleService.UpdatesUserStatsAfterDraw(User1, User2);
                LogText = $"LOG|{DateTime.Now}, END OF BATTLE WITHOUT RESULT {User1.Username}: {UserDeck1.Count} vs {User2.Username}: {UserDeck2.Count}";
                BattleLog.Add(LogText);
                Console.WriteLine(LogText);               
            }

            UniqueFeatureLOG.GetLogAsTXT(BattleLog);
        }

        public string ExecuteBattle()
        {
            //int count1;
            for (int i = 1; i < _maxRoundsOfBattle; ++i)            
            {
                Card uCard1 = BattleService.GetRandomDeckCards(UserDeck1);
                Card uCard2 = BattleService.GetRandomDeckCards(UserDeck2);

                /*foreach (var card in UserDeck1)
                    Console.WriteLine($"card: {card.Name}");
                foreach (var card in UserDeck2)
                    Console.WriteLine($"card2: {card.Name}");*/

                LogText = $"LOG|{DateTime.Now}, NUMBER OF CARDS IN DECK {User1.Username}: {UserDeck1.Count} vs {User2.Username}: {UserDeck2.Count}";
                BattleLog.Add(LogText);
                Console.WriteLine(LogText);
                HandleCardVsCard(uCard1, uCard2);
                /*if (i != 77)
                {
                    LogText = $"LOG|{DateTime.Now}, NUMBER OF CARDS IN DECK {User1.Username}: {UserDeck1.Count} vs {User2.Username}: {UserDeck2.Count}";
                    BattleLog.Add(LogText);
                    Console.WriteLine(LogText);
                    HandleCardVsCard(uCard1, uCard2);
                    count1 = UserDeck1.Count;
                }
                else
                    count1 = 0;*/

                if (UserDeck1.Count/*count1*/ < 1)
                {
                    BattleService.UpdatesUserStatsAfterWin(User2, User1);
                    LogText = $"\nLOG|{DateTime.Now}, Battle {User1.Username} vs {User2.Username} was won by {User2.Username.ToUpper()}.\n---------END OF BATTLE---------";
                    BattleLog.Add(LogText);
                    Console.WriteLine(LogText);
                    return User2.Username;
                }
                else if (UserDeck2.Count < 1)
                {
                    BattleService.UpdatesUserStatsAfterWin(User1, User2);
                    LogText = $"\nLOG|{DateTime.Now}, Battle {User1.Username} vs {User2.Username} was won by {User1.Username.ToUpper()}.\n---------END OF BATTLE---------";
                    BattleLog.Add(LogText);
                    Console.WriteLine(LogText);
                    return User1.Username;
                }
                else if (UserDeck1.Count < 1 && UserDeck1.Count < 1)
                {
                    BattleService.UpdatesUserStatsAfterDraw(User1, User2);
                    LogText = $"\nLOG|{DateTime.Now}, Battle {User1.Username} vs {User2.Username} ended in draw because of error.\n---------END OF BATTLE---------";
                    BattleLog.Add(LogText);
                    Console.WriteLine(LogText);
                    return "draw";
                }
                else
                {
                    ++Roundnumber;
                    if(Roundnumber == _maxRoundsOfBattle)
                        LogText = $"LOG|{DateTime.Now}, last BattleRound: {Roundnumber}.\n";
                    else
                        LogText = $"LOG|{DateTime.Now}, Continue BattleRound: {Roundnumber}.\n";
                    BattleLog.Add(LogText);
                    Console.WriteLine(LogText);
                }
            }
            
            return "roundlimit";
        }

        public string HandleCardVsCard(Card uCard1, Card uCard2)
        {
            
            double dmgCard1 = (double)uCard1.Damage;
            double dmgCard2 = (double)uCard2.Damage;

            LogText = $"LOG|{DateTime.Now}, {User1.Username} uses {uCard1.Name}[damage:{dmgCard1}] vs {uCard2.Name}[damage:{dmgCard2}] by {User2.Username}";
            BattleLog.Add(LogText);
            Console.WriteLine(LogText);

            // Catch scenario of fight.
            List<double> calculatedDamage = BattleService.CalculateDamage(uCard1, uCard2);
            dmgCard1 = calculatedDamage[0];
            dmgCard2 = calculatedDamage[1];

            // Check if rules for calculation of damage are correct.
            LogText = $"LOG|{DateTime.Now}, Actual damage: dmg1({dmgCard1}) vs dmg2({dmgCard2})";
            BattleLog.Add(LogText);
            Console.WriteLine(LogText);

            if (dmgCard1 < dmgCard2)
            {
                LogText = $"LOG|{DateTime.Now}, Card {uCard2.Name} from {User2.Username} wins.";
                BattleLog.Add(LogText);
                Console.WriteLine(LogText);

                // Swap loser card to winner deck.
                UserDeck2.Add(uCard1);
                UserDeck1.Remove(uCard1);
                return User2.Username;
            }
            else if (dmgCard1 > dmgCard2)
            {
                LogText = $"LOG|{DateTime.Now}, Card {uCard1.Name} from {User1.Username} wins.";
                BattleLog.Add(LogText);
                Console.WriteLine(LogText);

                // Swap loser card to winner deck.
                UserDeck1.Add(uCard2);
                UserDeck2.Remove(uCard2);
                return User1.Username;
            }
            else
            {
                LogText = $"LOG|{DateTime.Now}, This round ends in a draw.";
                BattleLog.Add(LogText);
                Console.WriteLine(LogText);
                return "draw";
            }

        }
    }
}
