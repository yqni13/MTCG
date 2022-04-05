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

        public List<Card> GetDeckCards(int userID)
        {
            NpgsqlConnection connection = DBConnection.Connect();
            List<Card> cards = new();
            
            try
            {
                connection.Open();
                var command = connection.CreateCommand();
                command.CommandText = $"SELECT d_card FROM {_tableName} WHERE d_user=@id";
                command.Parameters.AddWithValue("@id", userID);
                var reader = command.ExecuteReader();

                while(reader.Read())
                {
                    Guid cardID = Guid.Parse(reader[0].ToString());
                    cards.Add(new Card(cardID));
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

        public void AddDeckCards(List<String> cards, int userID, string userName)
        {
            NpgsqlConnection connection = DBConnection.Connect();
            var deckID = DeckService.CreateNewDeckID();

            try
            {
                connection.Open();
                var command = connection.CreateCommand();
                foreach(var card in cards)
                {
                    command.CommandText = $"INSERT INTO {_tableName} (d_id, d_user, d_name, d_card) VALUES (@deckId, @userId, @deckName, @cardId)";
                    command.Parameters.AddWithValue("@deckId", deckID);
                    command.Parameters.AddWithValue("@userId", userID);
                    command.Parameters.AddWithValue("@deckName", $"deck-{userName}-{userID}");
                    command.Parameters.AddWithValue("@cardId", card);
                }
               
            }
            
            catch (Exception err)
            {
                connection.Close();
                throw new Exception($"Error adding deck cards for user: {err.Message}");
            }

            connection.Close();

        }
    }
}
