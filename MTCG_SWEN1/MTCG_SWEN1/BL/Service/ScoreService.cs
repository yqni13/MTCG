using MTCG_SWEN1.DB.DAL;
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
        public static Dictionary<string, int> GetScoreboard()
        {            
            UserDAL userTABLE = new();
            return userTABLE.GetScoreboardSorted();
        }
    }
}
