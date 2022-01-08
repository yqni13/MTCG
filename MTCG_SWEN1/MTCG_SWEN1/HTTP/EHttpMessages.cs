using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MTCG_SWEN1.HTTP
{
    
    public enum EHttpMessages
    {
        // Using https://techketo.com/quick-tip-enum-to-description-in-csharp/
        // for how to create 'Descriptions' to use strings for enumerations.

        // Client error responses.
        [Description("400 Bad Request")]
        BadRequest400,
        [Description("404 Not Found")]
        NotFound404,

        // Server error responses
        [Description("500 Internal Server Error")]
        InternalServerError500,

        // Success response.
        [Description("200 OK")]
        OK200
    }

    
}
