﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MTCG_SWEN1.BL.Service
{
    class PackageService
    {
        public static bool CheckForEnoughFreeCards(int cardCount)
        {
            if (cardCount < 4)
                return false;

            return true;
        }
    }
}
