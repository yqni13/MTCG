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
        void AddPackage(List<Card> cards);

        void PurchasePackage(Guid userID);

        List<Card> GetAllCardsOfUser(Guid id);


    }
}
