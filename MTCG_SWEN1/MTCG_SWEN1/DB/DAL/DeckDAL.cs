using MTCG_SWEN1.BL.Service;
using MTCG_SWEN1.DB.InterfacesCRUD;
using MTCG_SWEN1.HTTP;
using MTCG_SWEN1.Models.Cards;
using Npgsql;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MTCG_SWEN1.DB.DAL
{
    class DeckDAL : IDecks
    {
        private readonly string _tableName = ETableNames.mtcg_decks.GetDescription();
        private readonly string _tableCardName = ETableNames.mtcg_cards.GetDescription();

        public List<Card> GetDeckCards(int deckID)
        {
            NpgsqlConnection connection = DBConnection.Connect();
            List<Card> cards = new();
            // suche die deckid für user
            // wähle karten mit passender deckid
            try
            {
                connection.Open();
                var command = connection.CreateCommand();
                command.CommandText = $"SELECT c_id, c_name, c_stackuser, c_damage, c_cardtype, c_elementtype FROM {_tableCardName} WHERE c_indeck=@id";
                command.Parameters.AddWithValue("@id", deckID);
                var reader = command.ExecuteReader();

                while(reader.Read())
                {
                    Guid cardID = Guid.Parse(reader[0].ToString());
                    string cardName = reader.GetString(1);
                    Guid user = Guid.Parse(reader[2].ToString());
                    int dmg = reader.GetInt32(3);
                    string type = reader.GetString(4);
                    string element = reader.GetString(5);

                    cards.Add(new Card(cardID, cardName, user, dmg, type, element));
                }                
                reader.Close();
            }
            catch (Exception err) when (err.Message == "No row is available")
            {
                connection.Close();
                Console.WriteLine($"DeckDAL, GetDeckCards(): Deck not existing.");
                return cards;
            }
            catch (Exception)
            {
                connection.Close();
                throw new Exception("Could not fetch data.");
            }

            connection.Close();
            return cards;
        }

        public void CreateNewDeck(Guid userID)
        {
            NpgsqlConnection connection = DBConnection.Connect();

            try
            {                
                connection.Open();                
                var command = connection.CreateCommand();
                
                command.CommandText = $"INSERT INTO {_tableName} (d_user) VALUES (@userId)";
                command.Parameters.AddWithValue("@userId", userID);
                command.ExecuteNonQuery();
                

                Console.WriteLine("finished");
            }

            catch (Exception err)
            {
                connection.Close();
                throw new Exception($"Error adding deck cards for user: {err}");
            }

            connection.Close();
        }

        public int GetDeckID(Guid userID)
        {
            NpgsqlConnection connection = DBConnection.Connect();
            int deckID;

            try
            {
                connection.Open();
                var command = connection.CreateCommand();
                command.CommandText = $"SELECT d_id FROM {_tableName} WHERE d_user=@id";
                command.Parameters.AddWithValue("@id", userID);
                var reader = command.ExecuteReader();

                reader.Read();
                deckID = reader.GetInt32(0);                
                reader.Close();
            }
            catch (Exception err) when (err.Message == "No row is available")
            {
                connection.Close();
                Console.WriteLine($"DeckDAL, GetDeckCards(): Deck not existing.");
                return -1;
            }
            catch (Exception)
            {
                connection.Close();
                throw new Exception("Could not fetch data.");
            }
            connection.Close();
            return deckID;
        }

        public void AddDeckCards(List<Card> cards, Guid userID, int deckID)
        {            
            NpgsqlConnection connection = DBConnection.Connect();
            
            try
            {
                connection.Open();
                foreach(var card in cards)
                {
                    var command = connection.CreateCommand();                    
                    command.CommandText = $"UPDATE {_tableCardName} SET c_indeck=@deckId WHERE c_id=@cardID";                    
                    command.Parameters.AddWithValue("@cardID", card.ID);
                    command.Parameters.AddWithValue("@deckId", deckID);
                    command.ExecuteNonQuery();
                    command.Dispose();
                }
            }
            catch (Exception err)
            {
                connection.Close();
                throw new Exception($"Error adding deck cards for user: {err}");
            }            

            connection.Close();
        }
        
    }
}
