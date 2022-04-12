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
        private HttpRequest _request;
        private HttpResponse _response;

        public TradingEndpoint(HttpRequest request, HttpResponse response)
        {
            _request = request;
            _response = response;
        }

        [Method("GET")]
        public void TradeGet()
        {
            try
            {
                _response.StatusMessage = EHttpStatusMessages.BadRequest400.GetDescription();
                _response.Body = "No implementations for GET/tradings.";
            }
            catch (Exception err)
            {
                Console.WriteLine(err.Message);
                _response.Body = "Error for /tradings GET";
                _response.StatusMessage = EHttpStatusMessages.NotFound404.GetDescription();
            }

            Console.WriteLine($"{ DateTime.UtcNow}, No implementations for GET/tradings.");
            _response.Send();
        }

        [Method("POST")]
        public void TradePost()
        {
            try
            {
                _response.StatusMessage = EHttpStatusMessages.BadRequest400.GetDescription();
                _response.Body = "No implementations for POST/tradings.";
            }
            catch (Exception err)
            {
                Console.WriteLine(err.Message);
                _response.Body = "Error for /tradings POST";
                _response.StatusMessage = EHttpStatusMessages.NotFound404.GetDescription();
            }

            Console.WriteLine($"{DateTime.UtcNow}, No implementations for POST/tradings.");
            _response.Send();
        }

        [Method("DELETE")]
        public void TradeDelete()
        {
            try
            {
                _response.StatusMessage = EHttpStatusMessages.BadRequest400.GetDescription();
                _response.Body = "No implementations for DELETE/tradings.";
            }
            catch (Exception err)
            {
                Console.WriteLine(err.Message);
                _response.Body = "Error for /tradings DELETE";
                _response.StatusMessage = EHttpStatusMessages.NotFound404.GetDescription();
            }

            Console.WriteLine($"{DateTime.UtcNow}, No implementations for DELETE/tradings.");
            _response.Send();
        }
    }
}
