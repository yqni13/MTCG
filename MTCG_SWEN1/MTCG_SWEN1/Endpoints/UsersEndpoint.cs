using MTCG_SWEN1.Endpoints.Attributes;
using MTCG_SWEN1.HTTP;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MTCG_SWEN1.Endpoints
{
    [Endpoint("/users")]
    class UsersEndpoint
    {
        public UsersEndpoint()
        {
            // ?
        }

        [Method("GET")]
        public void GetUsers()
        {
            
        }

        [Method("POST")]
        public void UsersPost()
        {

        }

        [Method("PUT")]
        public void PutUsers()
        {

        }
    }
}
