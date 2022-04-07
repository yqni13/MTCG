using MTCG_SWEN1.BL.Service;
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

        public void AddPackage(List<Card> cards)
        {
            NpgsqlConnection connection = DBConnection.Connect();
            try
            {
                connection.Open();
                var transaction = connection.BeginTransaction();
                for(int i = 0; i < cards.Count; ++i)
                {
                    var command = connection.CreateCommand();

                    command.CommandText = $"INSERT INTO {_tableName} (c_id, c_name, c_damage, c_cardtype, c_elementtype, c_packagetimestamp) " +
                        $"VALUES (@id, @name, @damage, @type, @element, @timestamp)";
                    command.Parameters.AddWithValue("@id", cards[i].ID);
                    command.Parameters.AddWithValue("@name", cards[i].Name);
                    command.Parameters.AddWithValue("@damage", cards[i].Damage);
                    command.Parameters.AddWithValue("@type", cards[i].CardType);
                    command.Parameters.AddWithValue("@element", cards[i].ElementType);
                    command.Parameters.AddWithValue("@timestamp", cards[i].PackageTimestamp);
                    command.ExecuteNonQuery();
                }
                transaction.Commit();
            }
            catch (Exception err)
            {
                connection.Close();                
                throw new Exception($"Error adding package of cards: {err}");
            }
            connection.Close();
        }

        public void PurchasePackage(Guid userID)
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
                    //var transaction = connection.BeginTransaction();

                    var command = connection.CreateCommand();

                    //command.CommandText = $"SELECT c_packageid, c_packagetimestamp FROM {_tableName} WHERE c_packagetimestamp = (SELECT MIN(c_packagetimestamp) FROM {_tableName})";
                    command.CommandText = $"SELECT c_id, c_packagetimestamp FROM {_tableName} WHERE c_stackuser IS NULL ORDER BY c_packagetimestamp FETCH FIRST 5 row only";
                    var reader = command.ExecuteReader();

                    while(reader.Read() && cards.Count != 5)
                    {                                                
                        Guid cardid = Guid.Parse(reader[0].ToString());
                        DateTime timestamp = (DateTime)reader.GetValue(1);
                        
                        cards.Add(new Card(cardid, timestamp));                    
                    }
                    reader.Close();

                    // 2. Update owner of 5 cards by changing to current user
                    //  => count if exactly 5 cards or not for exception                

                        

                    if (!PackageService.CheckForEnoughFreeCards(cards.Count))
                    {
                        connection.Close();
                        return;
                    }
                       

                }
                
                using (NpgsqlConnection connection = DBConnection.Connect())
                {
                    connection.Open();
                    for (int i = 0; i < cards.Count; ++i)
                    {
                        var command = connection.CreateCommand();
                        command.CommandText = $"UPDATE {_tableName} SET c_stackuser=@id WHERE c_id=@card AND c_packagetimestamp=@timestamp ";
                        command.Parameters.AddWithValue("@id", userID);
                        command.Parameters.AddWithValue("@card", cards[i].ID);
                        command.Parameters.AddWithValue("@timestamp", cards[i].PackageTimestamp);
                        
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
                throw new Exception($"Error purchasing new package: {err}");
            }

            //connectionUsers.Close();
            //connection.Close();
        }
                

        public List<Card> GetAllCardsOfUser(Guid id)
        {
            NpgsqlConnection connection = DBConnection.Connect();
            List<Card> cards = new();            
            try
            {                
                connection.Open();
                var command = connection.CreateCommand();
                command.CommandText = $"SELECT c_id, c_name, c_stackuser, c_damage, c_cardtype, c_elementtype FROM {_tableName} WHERE c_stackuser=@userId";
                command.Parameters.AddWithValue("@userId", id);
                var reader = command.ExecuteReader();

                while (reader.Read())
                {
                    Guid cardID = Guid.Parse(reader[0].ToString());
                    string cardName = reader.GetString(1);
                    Guid cardUser = Guid.Parse(reader[2].ToString());
                    double cardDamage = reader.GetInt32(3);
                    string type = reader.GetString(4);
                    string element = reader.GetString(5);

                    cards.Add(new Card(cardID, cardName, cardUser, cardDamage, type, element));
                }
                reader.Close();
            }
            catch (Exception err) when (err.Message == "No row is available")
            {
                connection.Close();
                Console.WriteLine($"CardDAL, GetAllCardsOfUser(): User does not own cards.");
            }

            connection.Close();
            return cards;
        }

        public List<Card> GetMaxNumberOfCardsToPurchase()
        {
            NpgsqlConnection connection = DBConnection.Connect();            
            List<Card> cards = new();
            try
            {
                connection.Open();
                var command = connection.CreateCommand();
                command.CommandText = $"SELECT c_id FROM {_tableName} WHERE c_stackuser IS NULL";
                
                var reader = command.ExecuteReader();

                while (reader.Read())
                {
                    Guid cardID = Guid.Parse(reader[0].ToString());                    

                    cards.Add(new Card(cardID));
                }
                reader.Close();
                
            }
            catch (Exception err) when (err.Message == "No row is available")
            {
                connection.Close();
                Console.WriteLine($"CardDAL, GetAllCardsOfUser(): User does not own cards.");
                return cards;
            }

            connection.Close();
            return cards;
        }
    }
}
