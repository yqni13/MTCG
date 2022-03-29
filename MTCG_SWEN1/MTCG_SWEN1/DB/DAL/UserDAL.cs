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
        private readonly DataBaseConnection _db = DataBaseConnection.GetStaticDBConnection;
        private readonly string _tableName = ETableNames.mctg_users.GetDescription();
        
        public void Create(Dictionary<string, string> credentials)
        {                        
            try
            {
                using (var command = _db.UpdateConnection().CreateCommand())
                {
                    command.CommandText = $"INSERT INTO {_tableName} (u_id, u_username, u_password) VALUES (@id, @username, @password)";
                    command.Parameters.AddWithValue("@u_id", Guid.NewGuid());
                    command.Parameters.AddWithValue("@username", credentials["Username"]);
                    command.Parameters.AddWithValue("@password", credentials["Password"]);
                    command.ExecuteNonQuery();
                }
                

            } 
            catch(Exception err)
            {
                Console.WriteLine($"UserDAL error => Create():\n{err}");                
                throw new DuplicateNameException("Error creating new user.");
            }
        }

        public User ReadSpecific(string username)
        {
            string read = $"SELECT u_username, u_password, u_coins, u_elo FROM users WHERE u_username=@username";
            
            try
            {
                using (var command = _db.UpdateConnection().CreateCommand())
                {
                    //command.CommandText = $"SELECT * FROM {_tableName} WHERE u_username=@username";
                    command.CommandText = read;
                    command.Parameters.AddWithValue("@username", username);
                    var reader = command.ExecuteReader();

                    reader.Read();
                    User user = new();
                    //user.Id = Guid.Parse(reader.GetString(0));                
                    user.Username = reader.GetString(0);
                    user.Password = reader.GetString(1);
                    user.Coins = reader.GetInt32(2);
                    //user.DeckID = Guid.Parse(reader.GetString(4));
                    /*if (reader.GetValue(4).ToString() != "")
                    {
                        user.DeckID = Guid.Parse(reader.GetValue(4).ToString());
                    }*/
                    user.ELO = reader.GetInt32(3);
                    reader.Close();
                    //_db.EndDBConnection();
                    return user;
                }
                
            } 
            catch (Exception err) when (err.Message == "No row is available")
            {
                /*if (err.Message == "No row is available")
                {
                    Console.WriteLine("User does not exist.");
                    throw new Exception("User not existing");
                }
                else
                    Console.WriteLine($"UserDAL error => ReadSpecific():\n{err}"); */
                Console.WriteLine($"UserDAL error => User existing - ReadSpecific():\n{err.Message}");
                return new User();
            }
            
        }

        

        public string CreateToken(Guid id)
        {
            string insert = "INSERT INTO sessions (s_token, s_user, s_timestamp) VALUES (@s_token, @s_user, @s_timestamp)";
            var command = _db.UpdateConnection().CreateCommand();
            command.CommandText = insert;

            NpgsqlCommand commandInsert = command as NpgsqlCommand;
            commandInsert.Parameters.Add("s_token", NpgsqlDbType.Varchar, 36);
            commandInsert.Parameters.Add("s_user", NpgsqlDbType.Integer);
            commandInsert.Parameters.Add("s_timestamp", NpgsqlDbType.Timestamp, 50);

            commandInsert.Prepare();
            string token = new string(Convert.ToBase64String(Guid.NewGuid().ToByteArray()).Take(36).ToArray());
            commandInsert.Parameters["s_token"].Value = token;
            commandInsert.Parameters["s_user"].Value = id;
            commandInsert.Parameters["s_timestamp"].Value = DateTime.Now;

            command.ExecuteNonQuery();
            return token;
        }

        public bool UserIsLoggedIn(string token)
        {
            string select = "SELECT s_user FROM sessions WHERE s_token = @Token";
            var command = _db.UpdateConnection().CreateCommand();
            command.CommandText = select;

            var userToken = command.CreateParameter();
            userToken.ParameterName = "Token";
            userToken.DbType = DbType.String;
            userToken.Value = token;
            command.Parameters.Add(userToken);

            var commandReader = command.ExecuteReader();
            if(commandReader.Read())
            {
                commandReader.Close();
                return true;
            }
            else
            {
                commandReader.Close();
                return false;
            }
        }       

        public void ReadAll()
        {
            throw new NotImplementedException();
        }

        
    }
}
