﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MTCG_SWEN1.Endpoints.Attributes;
using MTCG_SWEN1.HTTP;

namespace MTCG_SWEN1.Endpoints
{
    [Endpoint("/score")]
    class ScoresEndpoint
    {
        public ScoresEndpoint()
        {
            // ?
        }

        [Method("GET")]
        public void ScoresGet()
        {

        }
    }
}
