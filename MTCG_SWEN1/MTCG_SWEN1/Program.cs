using MTCG_SWEN1.HTTP;
using MTCG_SWEN1.Server;
using System;


namespace MTCG_SWEN1
{
    class Program
    {
        static void Main()
        {
            
            //Console.CancelKeyPress += (sender, e) => Environment.Exit(0);

            Console.WriteLine(EHttpMessages.BadRequest400.GetDescription());
            HttpServer server = HttpServer.GetServerStatic;
            server.SayHello();

        }
    }
}
