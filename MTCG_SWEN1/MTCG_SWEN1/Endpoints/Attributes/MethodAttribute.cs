using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MTCG_SWEN1.Endpoints.Attributes
{
    // Use AttributeTarget to save meta data for routing.
    [AttributeUsage(AttributeTargets.Method)]
    class MethodAttribute : Attribute
    {
       
        public string Method { get; set; }

        public MethodAttribute(string method)
        {
            Method = method;
        }
    }
}
