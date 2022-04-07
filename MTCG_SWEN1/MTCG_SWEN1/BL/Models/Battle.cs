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
    public enum BattleStatus
    {
        Win = 2,
        Loss = 1,
        Draw = 0
    }

    public enum BattleParticipants
    {
        User2 = 2,
        User1 = 1,
        Draw = 0
    }

    public class Battle
    {
        public User User1 { get; set; }
        public User User2 { get; set; }
        public List<Card> UserDeck1 { get; set; }
        public List<Card> UserDeck2 { get; set; }
        public BattleStatus Result { get; set; }
        public List<String> BattleLog { get; set; } = new();
        public Boolean BattleStillRunning { get; set; } = false;
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
                Console.WriteLine("Battle ended because of round limit and has no end result.");

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

                Console.WriteLine($"LOG|{DateTime.Now}, NUMBER OF CARDS IN DECK {User1.Username}: {UserDeck1.Count} vs {User2.Username}: {UserDeck2.Count}");
                HandleCardVsCard(uCard1, uCard2);
                /*if (i != 77)
                {
                    Console.WriteLine($"LOG|{DateTime.Now}, NUMBER OF CARDS IN DECK {User1.Username}: {UserDeck1.Count} vs {User2.Username}: {UserDeck2.Count}");
                    HandleCardVsCard(uCard1, uCard2);
                    count1 = UserDeck1.Count;
                }
                else
                    count1 = 0;*/

                if (UserDeck1.Count/*count1*/ < 1)
                {
                    BattleService.HandleUserStatsAfterWin(User2, User1);
                    LogText = $"\nLOG|{DateTime.Now}, Battle {User1.Username} vs {User2.Username} was won by {User2.Username.ToUpper()}.\n---------END OF BATTLE---------";
                    BattleLog.Add(LogText);
                    Console.WriteLine(LogText);
                    return User2.Username;
                }
                else if (UserDeck2.Count < 1)
                {
                    BattleService.HandleUserStatsAfterWin(User1, User2);
                    LogText = $"\nLOG|{DateTime.Now}, Battle {User1.Username} vs {User2.Username} was won by {User1.Username.ToUpper()}.\n---------END OF BATTLE---------";
                    BattleLog.Add(LogText);
                    Console.WriteLine(LogText);
                    return User1.Username;
                }
                else if (UserDeck1.Count < 1 && UserDeck1.Count < 1)
                {
                    BattleService.HandleUserStatsAfterDraw(User1, User2);
                    LogText = $"\nLOG|{DateTime.Now}, Battle {User1.Username} vs {User2.Username} ended in draw because of battle round limit (100 rounds) between.\n---------END OF BATTLE---------";
                    BattleLog.Add(LogText);
                    Console.WriteLine(LogText);
                    return "draw";
                }
                else
                {
                    ++Roundnumber;
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
                else
                {                   
                    dmgCard1 -= uCard2.Damage;
                    dmgCard2 -= uCard1.Damage;
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
                else
                {                    
                    dmgCard1 -= (double)uCard2.Damage;
                    dmgCard2 -= (double)uCard1.Damage;
                }

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
                else
                {                    
                    dmgCard1 -= (double)uCard2.Damage;
                    dmgCard2 -= (double)uCard1.Damage;
                }
            }
            else if (uCard1.CardType == "Spell" && uCard2.CardType == "Monster")
            {
                if (uCard2.Name.Contains("Kraken"))
                    dmgCard1 = 0.0;
                else if ((uCard1.CardType == "Spell" && uCard1.ElementType == "Water") && uCard2.Name.Contains("Knight"))
                    dmgCard2 = 0.0;
                else if (uCard1.ElementType == "Regular" && uCard2.ElementType == "Water" ||
                         uCard1.ElementType == "Water" && uCard2.ElementType == "Fire"  ||
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
                else
                {                    
                    dmgCard1 -= (double)uCard2.Damage;
                    dmgCard2 -= (double)uCard1.Damage;
                }
            }
            else
            {
                dmgCard1 -= (double)uCard2.Damage;
                dmgCard2 -= (double)uCard1.Damage;

                LogText = $"LOG|{DateTime.Now}, Ghost cards are fighting - {uCard1.Name}[dmg:{dmgCard1}] vs {uCard2.Name}[dmg:{dmgCard2}].";
                BattleLog.Add(LogText);
                Console.WriteLine(LogText);
            }

            Console.WriteLine($"LOG|{DateTime.Now}, Actual damage: dmg1({dmgCard1}) vs dmg2({dmgCard2})");

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
