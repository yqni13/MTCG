using MTCG_SWEN1.DB.DAL;
using MTCG_SWEN1.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MTCG_SWEN1.BL.Service
{
    class UserService
    {
        public static bool Register(Dictionary<string, string> credentials)
        {
            if(CheckIfUserExists(credentials["Username"]))
            {
                return false;
            }

            UserDAL userTable = new();
            userTable.Create(credentials);
            return true;
        }

        public static bool CheckIfUserExists(string username)
        {
            UserDAL userTABLE = new();
            User user = new();
            userTABLE.ReadSpecific(username, user);
            if (user.Username == username)
                return true;

            return false;
        }

        public static void Login()
        {

        }

    }
}
