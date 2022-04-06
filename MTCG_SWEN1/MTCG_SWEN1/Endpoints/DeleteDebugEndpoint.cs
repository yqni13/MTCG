using MTCG_SWEN1.DB;
using MTCG_SWEN1.Endpoints.Attributes;
using MTCG_SWEN1.HTTP;
using Npgsql;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MTCG_SWEN1.Endpoints
{
    [Endpoint("/deletedebug")]
    class DeleteDebugEndpoint
    {
        private HttpRequest _request;
        private HttpResponse _response;

        public DeleteDebugEndpoint(HttpRequest request, HttpResponse response)
        {
            _request = request;
            _response = response;
        }

        [Method("POST")]
        public void EmptyTables()
        {
            NpgsqlConnection connection = DBConnection.Connect();
            try
            {
                connection.Open();

                var command = connection.CreateCommand();
                command.CommandText = $"DELETE FROM tradings CASCADE;";
                command.ExecuteNonQuery();
                command.CommandText = $"DELETE FROM sessions CASCADE;";
                command.ExecuteNonQuery();
                command.CommandText = $"DELETE FROM decks CASCADE;";
                command.ExecuteNonQuery();
                command.CommandText = $"DELETE FROM cards CASCADE;";
                command.ExecuteNonQuery();
                command.CommandText = $"DELETE FROM users CASCADE;";
                command.ExecuteNonQuery();

            }
            catch (Exception err)
            {
                connection.Close();
                throw new Exception($"Error adding package of cards: {err.Message}");
            }
            connection.Close();
            _response.StatusMessage = EHttpStatusMessages.OK200.GetDescription();
            _response.Body = "Deleted.";
            _response.Send();
        }
    }
}
