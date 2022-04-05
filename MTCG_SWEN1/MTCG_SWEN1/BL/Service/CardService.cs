using MTCG_SWEN1.DB.DAL;
using MTCG_SWEN1.Models.Cards;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MTCG_SWEN1.BL.Service
{
    class CardService
    {
        public static void AddPackagesByAdmin(List<Card> cards)
        {
            CardsDAL cardTABLE = new();
            var id = UserService.GetUserID("admin");
            cardTABLE.AddPackage(cards, id);
        }
    }
}
