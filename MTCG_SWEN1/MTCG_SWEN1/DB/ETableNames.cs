using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MTCG_SWEN1.DB
{
    public enum ETableNames
    {
        [Description("users")]
        mtcg_users = 5,
        [Description("cards")]
        mtcg_cards = 4,
        [Description("decks")]
        mtcg_decks = 3,
        [Description("sessions")]
        mtcg_sessions = 2,
        [Description("own_cards")]
        mtcg_own_cards = 1,
        [Description("tradings")]
        mtcg_tradings = 0
    }
    
}
