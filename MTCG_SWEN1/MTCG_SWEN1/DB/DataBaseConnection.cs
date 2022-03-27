using Npgsql;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MTCG_SWEN1.DB
{
    class DataBaseConnection
    {        
        private readonly string _host = "localhost";
        private readonly string _username = "postgres";
        private readonly string _password = "postgre";
        private readonly string _database = "mtcg_db";
        private readonly string _dbConnectionData = "";
        private NpgsqlConnection _dbConnection;

        private readonly static DataBaseConnection s_staticDBConnection = new DataBaseConnection();
        public static DataBaseConnection GetStaticDBConnection { get => s_staticDBConnection; }


        private DataBaseConnection()
        {
            _dbConnectionData = $"Host={_host};Username={_username};Password={_password};Database={_database}";

            //_dbConnection = new NpgsqlConnection(_dbConnectionData);
            //EndDBConnection();
            //return;
            try
            {
                _dbConnection = new NpgsqlConnection(_dbConnectionData);
                _dbConnection.Open();
            } catch (NpgsqlException err)
            {
                Console.WriteLine($"{DateTime.UtcNow.AddHours(1)}, system failed to connect with database: \"{_database}\" - error message[NpgsqlException]:\n{err.Message}");
            } 

            Console.WriteLine($"{DateTime.UtcNow.AddHours(1)}, system successfully connected to database: \"{_database}\"");
        }

        public NpgsqlConnection UpdateConnection()
        {
            return _dbConnection;
        }

        public void EndDBConnection()
        {
            _dbConnection.Close();
        }
    }
}
