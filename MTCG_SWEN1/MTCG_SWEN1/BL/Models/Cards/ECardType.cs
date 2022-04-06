using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace MTCG_SWEN1.Models.Cards
{
    public enum ECardType
    {
        [Description("Spell")]
        SPELL,
        [Description("Monster")]
        MONSTER
    }
}
