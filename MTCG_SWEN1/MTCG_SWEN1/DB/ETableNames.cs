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
        mctg_users = 5,
        [Description("cards")]
        mctg_cards = 4,
        [Description("decks")]
        mctg_decks = 3,
        [Description("sessions")]
        mctg_sessions = 2,
        [Description("own_cards")]
        mctg_own_cards = 1,
        [Description("tradings")]
        mctg_tradings = 0
    }
    
}
