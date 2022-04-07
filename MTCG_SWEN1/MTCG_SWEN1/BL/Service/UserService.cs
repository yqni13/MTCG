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
        public static bool RegisterService(Dictionary<string, string> credentials)
        {
            if (CheckIfUserExists(credentials["Username"]))
                return false;
            
            UserDAL userTable = new();
            userTable.CreateUser(credentials);
            return true;
        }

        public static void LoginService(User user, string pwd, SessionsDAL sessionTABLE)
        {            
            if (user.Password == pwd)
                sessionTABLE.AddSession(CreateToken(user), user.Id);
        }

        public static bool CheckIfUserExists(string username)
        {
            UserDAL userTABLE = new();
            User user = new();
            userTABLE.GetUserByUsername(username, user);
            if (user.Username == username)
                return true;

            return false;
        }        

        public static bool CheckIfSessionExisting(User user, SessionsDAL sessionTABLE)
        {
            if (sessionTABLE.CheckExistingSession(user.Id))
                return true;

            return false;
        }

        public static bool CheckIfLoggedIn(string token)
        {
            SessionsDAL sessionTABLE = new();
            //if (sessionTABLE.GetUserIDByToken(token))
                //return true;

            if (sessionTABLE.GetUserIDByToken(token) != Guid.Empty)
                return true;

            return false;
        }

        public static bool CheckIfCredentialsComplete(string name, string pwd)
        {
            if ((name == null || name == "") || (pwd == null || pwd == ""))
                return false;

            return true;
        }

        public static string CreateToken(User user)
        {            
            return $"Basic {user.Username}-mtcgToken";
        }

        public static Guid GetUserID(string username)
        {
            UserDAL userTABLE = new();
            User user = new();
            userTABLE.GetUserByUsername(username, user);
            return user.Id;
        }

        public static User GetUserInformation(string token)
        {
            UserDAL userTABLE = new();
            SessionsDAL sessionTABLE = new();
            return userTABLE.GetUserByID(sessionTABLE.GetUserIDByToken(token));
        }

        public static void EditUserInformation(Dictionary<string, string> userEdit, string token)
        {
            UserDAL userTABLE = new();
            SessionsDAL sessionTABLE = new();
            userTABLE.EditUser(userEdit, sessionTABLE.GetUserIDByToken(token));
        }

        public static User GetUser(string token)
        {
            SessionsDAL sessionsTABLE = new();
            UserDAL userTABLE = new();
            Guid userID = sessionsTABLE.GetUserIDByToken(token);
            return userTABLE.GetUserByID(userID);
        }
    }
}
