using MTCG_SWEN1.Models;
using Npgsql;
using NpgsqlTypes;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MTCG_SWEN1.DB.DAL
{
    class UserDAL
    {
        private readonly IDbConnection _db;

        public UserDAL()
        {
            _db = DataBaseConnection.Connect();
            try
            {
                _db.Open();
                if (_db.State != ConnectionState.Open)
                    throw new Exception("DB could not be opened.");
            }
            catch(Exception err)
            {
                Console.WriteLine(err.Message);
            }            
        }

        public void InsertUser(Credentials credentials)
        {
            string insert = "INSERT INTO users (u_username, u_password) VALUES (@u_username, @u_password)";
            IDbCommand commandGeneral = _db.CreateCommand();
            commandGeneral.CommandText = insert;

            NpgsqlCommand commandInsert = commandGeneral as NpgsqlCommand;
            commandInsert.Parameters.Add("@u_id", NpgsqlDbType.Char, 36);
            commandInsert.Parameters.Add("u_username", NpgsqlDbType.Varchar, 20);
            commandInsert.Parameters.Add("u_password", NpgsqlDbType.Varchar, 36);
            commandInsert.Prepare();

            commandInsert.Parameters["u_id"].Value = Guid.NewGuid();
            commandInsert.Parameters["u_username"].Value = credentials.Username;
            commandInsert.Parameters["u_password"].Value = credentials.Password;

            commandGeneral.ExecuteNonQuery();
        }

        public void ReadUser(Credentials credentials)
        {
            string read = "SELECT u_id, u_username, u_password, u_coins, u_deck, u_elo FROM users WHERE u_username = @UserName";

            IDbCommand commandGeneral = _db.CreateCommand();
            commandGeneral.CommandText = read;

        }


    }
}
