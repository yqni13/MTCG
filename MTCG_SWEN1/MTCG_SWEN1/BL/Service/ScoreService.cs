using MTCG_SWEN1.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MTCG_SWEN1.BL.Service
{
    class ScoreService
    {
        public static Dictionary<string, string> GetScoreboard()
        {
            List<User> users = new();
            Dictionary<string, string> scoreboard = new();
            int index = 0;
            foreach(var highscore in users)
            {
                scoreboard.Add(users[index].Username, users[index].ELO.ToString());
                index++;
            }
            scoreboard = scoreboard.OrderByDescending(u => u.Value).ToDictionary(z => z.Key, y => y.Value);
            return scoreboard;
        }
    }
}
