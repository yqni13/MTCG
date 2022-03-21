using MTCG_SWEN1.HTTP;
using MTCG_SWEN1.Server;
using System;
using System.Linq;
using System.Windows;

namespace MTCG_SWEN1
{
    class Program
    {
        static void Main()
        {
            
            Console.CancelKeyPress += (sender, e) => Environment.Exit(0);
                        
            HttpServer.GetServerStatic.StartServerThread();

            string a = "/transactions/packages";
            int slashCount = a.ToCharArray().Count(symbol => symbol == '/');
            if (slashCount > 1)
                Console.WriteLine($"returned string: {a.Substring(0, a.LastIndexOf("/"))}");
            
        }
    }
}
