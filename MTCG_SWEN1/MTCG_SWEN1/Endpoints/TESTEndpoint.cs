using MTCG_SWEN1.Endpoints.Attributes;
using MTCG_SWEN1.HTTP;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MTCG_SWEN1.Endpoints
{
    [Endpoint("/test")]
    class TESTEndpoint
    {
        private HttpRequest _request;
        private HttpResponse _response;

        public TESTEndpoint(HttpRequest request, HttpResponse response)
        {
            _request = request;
            _response = response;
        }

        [Method("POST")]
        public void TestPOST()
        {
            _response.Body = "Users example Response";
            _response.StatusMessage = EHttpStatusMessages.BadRequest400.GetDescription();
            _response.Send();
        }
    }
}
