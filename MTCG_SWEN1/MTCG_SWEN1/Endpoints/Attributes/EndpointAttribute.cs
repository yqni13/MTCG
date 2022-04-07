using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MTCG_SWEN1.Endpoints.Attributes
{
    // Use AttributeTarget to save meta data for routing.
    [AttributeUsage(AttributeTargets.Class)]
    class EndpointAttribute : Attribute
    {
        
        public string Path { get; set; }

        public EndpointAttribute(string path)
        {
            Path = path;
        }
    }
}
