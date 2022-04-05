using MTCG_SWEN1.DB.InterfacesCRUD;
using MTCG_SWEN1.HTTP;
using Npgsql;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MTCG_SWEN1.DB.DAL
{
    class SessionsDAL : ISessions
    {
        private readonly string _tableName = ETableNames.mctg_sessions.GetDescription();

        public void AddSession(string token, int id)
        {
            NpgsqlConnection connection = DBConnection.Connect();
            
            try
            {
                connection.Open();
                var command = connection.CreateCommand();

                command.CommandText = $"INSERT INTO {_tableName} (s_token, s_user, s_timestamp ) VALUES (@token, @user, @timestamp)";
                command.Parameters.AddWithValue("@token", token);
                command.Parameters.AddWithValue("@user", id);
                command.Parameters.AddWithValue("@timestamp", DateTime.Now);
                command.ExecuteNonQuery();
            }
            catch (Exception err)
            {                
                connection.Close();
                throw new Exception($"Error adding session-token: {err.Message}");
            }
            connection.Close();
        }

        public bool CheckExistingSession(int id)
        {
            NpgsqlConnection connection = DBConnection.Connect();
            string token;
            try
            {
                connection.Open();
                var command = connection.CreateCommand();
                command.CommandText = $"SELECT * FROM {_tableName} WHERE s_user=@id";
                command.Parameters.AddWithValue("@id", id);
                var reader = command.ExecuteReader();

                reader.Read();
                token = reader.GetString(0);
                var userID = reader.GetInt32(1);
                reader.Close();
            }
            catch (Exception)
            {
                connection.Close();
                return false;
            }

            connection.Close();
            if (token == "")
                return false;

            return true;
        }

        public bool CheckLoggedInUserByToken(string requestingToken)
        {
            NpgsqlConnection connection = DBConnection.Connect();
            string dbToken;
            try
            {
                Console.WriteLine($"\"{requestingToken}\"");
                connection.Open();
                var command = connection.CreateCommand();
                command.CommandText = $"SELECT s_token FROM {_tableName} WHERE s_token=@token";
                command.Parameters.AddWithValue("@token", requestingToken);
                var reader = command.ExecuteReader();

                reader.Read();
                dbToken = reader.GetString(0);                
                reader.Close();
            }
            catch (Exception err) when (err.Message == "No row is available")
            {
                connection.Close();
                Console.WriteLine($"SessionDAL, CheckLoggedInUserByToken(): User not logged in.");
                return false;
            }
            catch (Exception)
            {
                connection.Close();
                throw new Exception("Could not fetch data.");
            }

            connection.Close();            
            return true;
        }
    }
}
