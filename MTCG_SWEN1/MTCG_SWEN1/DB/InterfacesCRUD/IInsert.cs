﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MTCG_SWEN1.DB.InterfacesCRUD
{
    public interface IInsert
    {
        void Create(Dictionary<string, string> credentials);
    }
}