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
            int id = sessionTABLE.GetUserIDByToken(token);
            
            return userTABLE.GetUserByID(id);
        }
    }
}
