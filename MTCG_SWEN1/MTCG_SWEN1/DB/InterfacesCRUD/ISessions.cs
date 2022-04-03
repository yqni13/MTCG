using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MTCG_SWEN1.DB.InterfacesCRUD
{
    interface ISessions
    {
        void AddSession(string token, int id);

        bool CheckExistingSession(int id);
    }
}
