using MTCG_SWEN1.Models.Cards;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace MTCG_SWEN1.Models
{
    public class User
    {
        public int Id { get; set; }
        public string Username { get; set; }
        public string Password { get; set; }
        public int Coins { get; set; }
        public Guid DeckID { get; set; }
        public int ELO { get; set; }

        public User() { }


        public User(string user, string pwd)
        {
            Username = user;
            Password = pwd;
        }        
    }
}
