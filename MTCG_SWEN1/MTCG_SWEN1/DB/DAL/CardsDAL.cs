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
    public class CardsDAL : ICards
    {

        private readonly string _tableName = ETableNames.mctg_cards.GetDescription();

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

        public void PurchasePackage()
        {
            throw new NotImplementedException();
        }
    }
}
