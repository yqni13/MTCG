using MTCG_SWEN1.DB.InterfacesCRUD;
using MTCG_SWEN1.HTTP;
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
    class UserDAL : IInsert, IRead
    {
        //private readonly DataBaseConnection _db = DataBaseConnection.GetStaticDBConnection;
        private readonly string _tableName = ETableNames.mtcg_users.GetDescription();

        public void Create(Dictionary<string, string> credentials)
        {
            NpgsqlConnection connection = DBConnection.Connect();
            try
            {                
                connection.Open();
                var command = connection.CreateCommand();

                command.CommandText = $"INSERT INTO {_tableName} (u_username, u_password) VALUES (@username, @password)";
                command.Parameters.AddWithValue("@username", credentials["Username"]);
                command.Parameters.AddWithValue("@password", credentials["Password"]);
                command.ExecuteNonQuery();
            }
            catch (Exception)
            {
                //Console.WriteLine($"UserDAL error => Create():\n{err.Message}+still belongs here");
                connection.Close();
                throw new DuplicateNameException("Error creating new user.");
            }
            connection.Close();
        }

       
        public void ReadSpecific(string username, User user)
        {
            NpgsqlConnection connection = DBConnection.Connect();
            try
            {                
                connection.Open();
                var command = connection.CreateCommand();
                command.CommandText = $"SELECT * FROM {_tableName} WHERE u_username=@username";
                command.Parameters.AddWithValue("@username", username);
                var reader = command.ExecuteReader();

                reader.Read();
                user.Id = reader.GetInt32(0);
                user.Username = reader.GetString(1);
                user.Password = reader.GetString(2);
                user.Coins = reader.GetInt32(3);
                if (reader.GetValue(4).ToString() != "")
                {
                    user.DeckID = reader.GetInt32(4);

                }
                user.ELO = reader.GetInt32(5);
                reader.Close();
            }
            catch (Exception err) when (err.Message == "No row is available")
            {
                Console.WriteLine($"UserDAL, ReadSpecific(): User does not exist.");
                connection.Close();
            }
            catch (Exception)
            {
                //Console.WriteLine($"UserDAL, ReadSpecific(): {err.Message}");
                connection.Close();
                throw new Exception("Could not fetch data.");
            }
            connection.Close();
        }


        public void ReadAll()
        {
            throw new NotImplementedException();
        }

        
    }
}
