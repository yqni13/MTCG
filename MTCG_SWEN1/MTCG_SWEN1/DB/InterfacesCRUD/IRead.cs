using MTCG_SWEN1.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MTCG_SWEN1.DB.InterfacesCRUD
{
    public interface IRead
    {
        void ReadAll();

        void ReadSpecific(string username, User user);

        string GetUsernameByID(int id);
    }
}
