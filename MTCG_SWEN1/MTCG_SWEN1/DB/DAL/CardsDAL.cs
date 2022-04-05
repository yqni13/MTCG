using MTCG_SWEN1.DB.InterfacesCRUD;
using MTCG_SWEN1.HTTP;
using MTCG_SWEN1.Models.Cards;
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
    public class CardsDAL : ICards
    {

        private readonly string _tableName = ETableNames.mtcg_cards.GetDescription();
        private readonly string _tableNameUSER = ETableNames.mtcg_users.GetDescription();

        public void AddPackage(List<Card> cards, int id)
        {
            NpgsqlConnection connection = DBConnection.Connect();
            try
            {
                connection.Open();
                var transaction = connection.BeginTransaction();
                for(int i = 0; i < cards.Count; ++i)
                {
                    var command = connection.CreateCommand();

                    command.CommandText = $"INSERT INTO {_tableName} (c_id, c_name, c_damage, c_user) VALUES (@id, @name, @damage, @user)";
                    command.Parameters.AddWithValue("@id", cards[i].ID);
                    command.Parameters.AddWithValue("@name", cards[i].Name);
                    command.Parameters.AddWithValue("@damage", cards[i].Damage);
                    command.Parameters.AddWithValue("@user", id);
                    command.ExecuteNonQuery();
                }
                transaction.Commit();
            }
            catch (Exception err)
            {
                connection.Close();
                throw new Exception($"Error adding package of cards: {err.Message}");
            }
            connection.Close();
        }

        public void PurchasePackage(int userID, int adminID)
        {
            List<Card> cards = new();
            //NpgsqlConnection connection = DBConnection.Connect();
            
            // 1. Get 5 cards from admin ownership => define to be free to charge.
            //  => select by asking if current owner is admin or not

            try
            {
                using(NpgsqlConnection connection = DBConnection.Connect())
                {
                    connection.Open();
                    var transaction = connection.BeginTransaction();

                    var command = connection.CreateCommand();

                    command.CommandText = $"SELECT c_id, c_name, c_user, c_damage FROM {_tableName} WHERE c_user=@id";
                    command.Parameters.AddWithValue("@id", adminID);
                    var reader = command.ExecuteReader();

                    while(reader.Read())
                    {
                        Guid cardID = Guid.Parse(reader.GetString(0));
                        string cardName = reader.GetString(1);
                        int Id = reader.GetInt32(2);
                        double cardDamage = reader.GetInt32(3);
                        //bool cardInDeck = reader.GetBoolean(4);
                        //int cardType = reader.GetInt32(5);
                        //int elementType = reader.GetInt32(6);
                        //bool cardForTrade = reader.GetBoolean(7);
                        cards.Add(new Card(cardID, cardName, Id, cardDamage));                    
                    }
                    reader.Close();

                    // 2. Update owner of 5 cards by changing to current user
                    //  => count if exactly 5 cards or not for exception                

                    if (cards.Count != 5)
                    {
                        Console.WriteLine($"List of cards count to: {cards.Count}");
                        //transaction.Rollback("Wrong number of cards, need to be 5.");
                        transaction.Rollback();
                    }
                    transaction.Commit();
                    //connection.Close();

                }

                using (NpgsqlConnection connection = DBConnection.Connect())
                {
                    connection.Open();
                    for (int i = 0; i < cards.Count; ++i)
                    {
                        var command = connection.CreateCommand();
                        command.CommandText = $"UPDATE {_tableName} SET c_user=@id WHERE c_id=@cardId";
                        command.Parameters.AddWithValue("@id", userID);
                        command.Parameters.AddWithValue("@cardId", cards[i].ID);                        
                        command.ExecuteNonQuery();                   
                    }
                    connection.Close();
                }

                using (NpgsqlConnection connection = DBConnection.Connect())
                {
                    connection.Open();
                    // 3. Update coins of current user by minus 5                
                    
                    var command = connection.CreateCommand();
                    command.CommandText = $"UPDATE {_tableNameUSER} SET u_coins=u_coins-5 WHERE u_id=@userId";
                    command.Parameters.AddWithValue("@userId", userID);
                    command.ExecuteNonQuery();

                    connection.Close();

                }

            }
            catch (Exception err)
            {
                //connection.Close();
                //connectionUsers.Close();
                throw new Exception($"Error purchasing new package: {err.Message}");
            }

            //connectionUsers.Close();
            //connection.Close();
        }

        public void UpdateUserCoins(int userID)
        {

        }
    }
}
