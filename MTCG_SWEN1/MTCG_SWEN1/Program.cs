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

            
        }
    }
}
