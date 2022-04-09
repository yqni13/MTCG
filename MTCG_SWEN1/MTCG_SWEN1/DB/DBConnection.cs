using Npgsql;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MTCG_SWEN1.DB
{
    class DBConnection
    {
        //private readonly string _host = "localhost";
        //private readonly string _username = "postgres";
        //private readonly string _password = "postgre";
        //private readonly string _password = "postgresql";
        //private readonly string _database = "mtcg_db";
        //private readonly string _dbConnectionData = "";

        public static NpgsqlConnection Connect()
        {
            return new NpgsqlConnection($"Host=localhost;Username=postgres;Password=postgre;Database=mtcg_db;IncludeErrorDetail=true");
        } 

    }
}
