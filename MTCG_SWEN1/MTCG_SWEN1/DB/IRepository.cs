using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MTCG_SWEN1.DB
{
    public interface IRepository
    {
        public void Create() { }

        public void Insert() { }

        public void ReadAll() { }

        public void ReadSpecific() { }

        public void Update() { }

        public void Delete() { }
    }
}
