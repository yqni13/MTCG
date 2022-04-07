using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MTCG_SWEN1.HTTP
{
    
    public enum EHttpStatusMessages
    {
        // Using https://techketo.com/quick-tip-enum-to-description-in-csharp/
        // for how to create 'Descriptions' to use strings for enumerations.

        // Client error responses.
        [Description("400 Bad Request")]
        BadRequest400 = 400,
        [Description("401 Unauthorized")]
        Unauthorized401 = 401,
        [Description("403 Forbidden")]
        Forbidden403 = 403,
        [Description("404 Not Found")]
        NotFound404 = 404,
        [Description("406 Not Acceptable")]
        NotAcceptable406 = 406,

        // Server error responses
        [Description("500 Internal Server Error")]
        InternalServerError500 = 500,

        // Success response.
        [Description("200 OK")]
        OK200 = 200
    }

    
}
