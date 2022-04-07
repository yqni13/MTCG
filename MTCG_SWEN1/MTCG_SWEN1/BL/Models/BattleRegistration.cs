using MTCG_SWEN1.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MTCG_SWEN1.BL.Models
{    
    class BattleRegistration
    {
        private static BattleRegistration _instance = null;
        private static readonly object _lock = new object();
        private static List<User> _joiningUsers = new();

        private BattleRegistration() { }

        public static BattleRegistration Instance
        {
            get
            {
                lock(_lock)
                {
                    if (_instance == null)
                    {
                        _instance = new();
                    }
                    return _instance;
                }
            }
        }

        public void RequestBattle(User user)
        {
            if (_joiningUsers.Count < 1)
                _joiningUsers.Add(user);
            else
            {
                Battle battle = new(_joiningUsers[0], user);
                _joiningUsers.Clear();
                battle.StartBattle();
            }
        }

    }
}
