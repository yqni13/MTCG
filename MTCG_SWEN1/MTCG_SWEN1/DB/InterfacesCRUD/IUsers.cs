using MTCG_SWEN1.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MTCG_SWEN1.DB.InterfacesCRUD
{
    interface IUsers
    {
        void CreateUser(Dictionary<string, string> credentials);

        void GetUserByUsername(string username, User user);

        List<User> ReadAll();

        User GetUserByID(Guid id);

        void EditUser(Dictionary<string, string> userEdit, Guid userID);

        Dictionary<string, int> GetScoreboardSorted();

        void UpdateBattleStats(List<User> battleUsers);
    }
}
