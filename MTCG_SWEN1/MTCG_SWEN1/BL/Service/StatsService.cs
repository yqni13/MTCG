using MTCG_SWEN1.DB.DAL;
using MTCG_SWEN1.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MTCG_SWEN1.BL.Service
{
    class StatsService
    {
        public static User GetUserStats(string token)
        {
            //User user = new();
            SessionsDAL sessionTABLE = new();
            UserDAL userTABLE = new();
            Guid id = sessionTABLE.GetUserIDByToken(token);
            
            return userTABLE.GetUserByID(id);
        }

        public static Dictionary<string, string> PrepareStatsDisplay(User user)
        {
            Dictionary<string, string> statistics = new();
            statistics.Add("Username", user.Username);
            statistics.Add("ELO", user.ELO.ToString());
            statistics.Add("Coins", user.Coins.ToString());
            statistics.Add("Games", user.Games.ToString());
            statistics.Add("Wins", user.Wins.ToString());
            statistics.Add("Losses", user.Losses.ToString());

            return statistics;
        }
    }
}
