using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MTCG_SWEN1.Endpoints.Attributes;
using MTCG_SWEN1.HTTP;

namespace MTCG_SWEN1.Endpoints
{
    [Endpoint("/tradings")]
    class TradingEndpoint
    {
        public TradingEndpoint()
        {
            // ?
        }

        [Method("GET")]
        public void TradeGet()
        {

        }

        [Method("POST")]
        public void TradePost()
        {

        }

        [Method("DELETE")]
        public void TradeDelete()
        {

        }
    }
}
