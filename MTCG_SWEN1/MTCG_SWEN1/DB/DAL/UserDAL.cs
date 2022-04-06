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

        public void CreateUser(Dictionary<string, string> credentials)
        {
            NpgsqlConnection connection = DBConnection.Connect();
            try
            {                
                connection.Open();
                var command = connection.CreateCommand();

                command.CommandText = $"INSERT INTO {_tableName} (u_id, u_username, u_password) VALUES (@id, @username, @password)";
                command.Parameters.AddWithValue("@id", Guid.NewGuid());
                command.Parameters.AddWithValue("@username", credentials["Username"]);
                command.Parameters.AddWithValue("@password", credentials["Password"]);
                command.ExecuteNonQuery();
            }
            catch (Exception err)
            {
                //Console.WriteLine($"UserDAL error => Create():\n{err.Message}+still belongs here");
                connection.Close();
                Console.WriteLine(err.Message);
                throw new DuplicateNameException("Error creating new user.");
            }
            connection.Close();
        }

       
        public void GetUserByUsername(string username, User user)
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
                user.Id = Guid.Parse(reader[0].ToString());
                user.Username = reader.GetString(1);
                user.Password = reader.GetString(2);
                user.Coins = reader.GetInt32(3);                
                user.ELO = reader.GetInt32(4);
                if (reader.GetValue(5).ToString() != "")
                    user.Bio = reader.GetString(5);
                else
                    user.Bio = "";

                if (reader.GetValue(6).ToString() != "")
                    user.Image = reader.GetString(6);
                else
                    user.Image = "";
                user.Games = reader.GetInt32(7);
                user.Wins = reader.GetInt32(8);
                user.Losses = reader.GetInt32(9);
                reader.Close();
            }
            catch (Exception err) when (err.Message == "No row is available")
            {
                Console.WriteLine($"UserDAL, GetUserByUsername(): User does not exist.");                
                connection.Close();
            }
            catch (Exception err)
            {
                //Console.WriteLine($"UserDAL, ReadSpecific(): {err.Message}");
                connection.Close();
                Console.WriteLine(err.Message);
                throw new Exception("Could not fetch data.");
            }
            connection.Close();
        }


        public List<User> ReadAll()
        {
            NpgsqlConnection connection = DBConnection.Connect();
            List<User> users = new();

            try
            {
                connection.Open();
                var command = connection.CreateCommand();
                command.CommandText = $"SELECT * FROM {_tableName}";                
                var reader = command.ExecuteReader();
                string bio, image;

                while (reader.Read())
                {
                    Guid Id = Guid.Parse(reader[0].ToString());
                    string name = reader.GetString(1);
                    string pwd = reader.GetString(2);
                    int coins = reader.GetInt32(3);
                    int elo = reader.GetInt32(4);
                    if (reader.GetValue(5).ToString() != "")
                        bio = reader.GetString(5);
                    else
                        bio = "";

                    if (reader.GetValue(6).ToString() != "")
                        image = reader.GetString(6);
                    else
                        image = "";                    
                    int games = reader.GetInt32(7);
                    int wins = reader.GetInt32(8);
                    int losses = reader.GetInt32(9);

                    users.Add(new User(Id, name, pwd, coins, elo, bio, image, games, wins, losses));
                }
                reader.Close();
            }
            catch (Exception)
            {
                //Console.WriteLine($"UserDAL, ReadSpecific(): {err.Message}");
                connection.Close();
                throw new Exception("Could not fetch data.");
            }
            connection.Close();
            return users;
        }

        public User GetUserByID(Guid id)
        {
            NpgsqlConnection connection = DBConnection.Connect();
            User user = new();
            try
            {
                connection.Open();
                var command = connection.CreateCommand();
                command.CommandText = $"SELECT * FROM {_tableName} WHERE u_id=@userId";
                command.Parameters.AddWithValue("@userId", id);
                var reader = command.ExecuteReader();                

                reader.Read();
                user.Id = Guid.Parse(reader[0].ToString());
                user.Username = reader.GetString(1);
                user.Password = reader.GetString(2);
                user.Coins = reader.GetInt32(3);
                user.ELO = reader.GetInt32(4);
                if (reader.GetValue(5).ToString() != "")
                    user.Bio = reader.GetString(5);
                else
                    user.Bio = "";

                if (reader.GetValue(6).ToString() != "")
                    user.Image = reader.GetString(6);
                else
                    user.Image = "";
                user.Games = reader.GetInt32(7);
                user.Wins = reader.GetInt32(8);
                user.Losses = reader.GetInt32(9);
                reader.Close();
            }
            catch (Exception err)
            {
                //Console.WriteLine($"UserDAL, ReadSpecific(): {err.Message}");
                connection.Close();
                Console.WriteLine(err.Message);
                throw new Exception("Could not fetch data.");
            }
            connection.Close();
            return user;
        }

        public void EditUser(Dictionary<string, string> userEdit, Guid userID)
        {
            NpgsqlConnection connection = DBConnection.Connect();
            try
            {
                connection.Open();
                // update name, bio, image               

                var command = connection.CreateCommand();
                command.CommandText = $"UPDATE {_tableName} SET u_username=@user, u_bio=@bio, u_image=@image WHERE u_id=@userId";
                command.Parameters.AddWithValue("@user", userEdit["Name"]);
                command.Parameters.AddWithValue("@bio", userEdit["Bio"]);
                command.Parameters.AddWithValue("@image", userEdit["Image"]);
                command.Parameters.AddWithValue("@userId", userID);
                command.ExecuteNonQuery();
                command.Dispose();
                connection.Close();

            }
            catch(Exception err)
            {
                connection.Close();
                throw new Exception($"Error updating User: {err.Message}");
            }


        }
        public Dictionary<string, int> GetScoreboardSorted()
        {
            NpgsqlConnection connection = DBConnection.Connect();
            Dictionary<string, int> scoreboard = new();
            try
            {
                connection.Open();
                // update name, bio, image               

                var command = connection.CreateCommand();
                command.CommandText = $"SELECT u_username, u_elo FROM {_tableName} ORDER BY u_elo DESC";
                var reader = command.ExecuteReader();

                while (reader.Read())
                {
                    scoreboard.Add(reader.GetString(0), reader.GetInt32(1));
                }
                reader.Close();
                command.Dispose();
                connection.Close();

            }
            catch (Exception err)
            {
                connection.Close();
                throw new Exception($"Error updating User: {err.Message}");
            }

            return scoreboard;
        } 
    }
}
