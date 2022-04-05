using MTCG_SWEN1.Models.Cards;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MTCG_SWEN1.DB.InterfacesCRUD
{
    public interface ICards
    {
        void AddPackage(List<Card> cards, int id);

        void PurchasePackage(int userID, int adminID);

        void UpdateUserCoins(int userID);

        List<Card> GetAllCardsOfUser(int id);
    }
}
