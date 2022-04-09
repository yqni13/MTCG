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
        public Guid Id { get; set; }
        public string Username { get; set; }
        public string Password { get; set; }
        public int Coins { get; set; }
        public int ELO { get; set; }
        public string Bio { get; set; } = "";
        public string Image { get; set; } = "";
        public int Games { get; set; }
        public int Wins { get; set; }
        public int Losses { get; set; }

        public User() { }

        public User(Guid id, string user, string pwd, int coins, int elo, string bio, string image, int games, int wins, int losses)
        {
            Id = id;
            Username = user;
            Password = pwd;
            Coins = coins;
            ELO = elo;
            Bio = bio;
            Image = image;
            Games = games;
            Wins = wins;
            Losses = losses;
        }        
    }
}
