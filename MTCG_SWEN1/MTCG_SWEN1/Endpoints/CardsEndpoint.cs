using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MTCG_SWEN1.Endpoints.Attributes;
using MTCG_SWEN1.HTTP;

namespace MTCG_SWEN1.Endpoints
{
    [Endpoint("/cards")]
    class CardsEndpoint
    {
        private HttpRequest _request;
        private HttpResponse _response;

        public CardsEndpoint(HttpRequest request, HttpResponse response)
        {
            _request = request;
            _response = response;
        }

        [Method("GET")]
        public void CardsGet()
        {
            try
            {
                _response.StatusMessage = EHttpStatusMessages.OK200.GetDescription();
                _response.Body = "Demo content for /cards GET";
            }
            catch (Exception err)
            {
                Console.WriteLine(err.Message);
                _response.Body = "Error for /cards GET";
                _response.StatusMessage = EHttpStatusMessages.NotFound404.GetDescription();
            }
            _response.Send();
        }
    }
}
