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
            try
            {
                _response.StatusMessage = EHttpStatusMessages.OK200.GetDescription();
                _response.Body = "Demo content for /test POST";
            }
            catch (Exception err)
            {
                Console.WriteLine(err.Message);
                _response.Body = "Error for /test POST";
                _response.StatusMessage = EHttpStatusMessages.NotFound404.GetDescription();
            }
            _response.Send();
        }
    }
}
