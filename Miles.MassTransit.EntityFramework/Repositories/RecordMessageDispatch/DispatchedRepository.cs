using MassTransit;
using Miles.MassTransit.RecordMessageDispatch;
using System;
using System.Data.SqlClient;
using System.Threading.Tasks;

namespace Miles.MassTransit.EntityFramework.RecordMessageDispatch
{
    public class DispatchedRepository : IDispatchedRepository
    {
        private readonly string connectionString;

        public DispatchedRepository(string connectionString)
        {
            this.connectionString = connectionString;
        }

        public async Task RecordAsync(SendContext context)
        {
            using (var connection = new SqlConnection(connectionString))
            using (var command = new SqlCommand("UPDATE [dbo].[OutgoingMessages] SET [DispatchedDate] = @DispatchedDate WHERE MessageId = @MessageId", connection))
            {
                command.Parameters.AddWithValue("@DispatchedDate", DateTime.Now);
                command.Parameters.AddWithValue("@MessageId", context.MessageId.Value);

                await connection.OpenAsync();
                await command.ExecuteNonQueryAsync();
            }
        }
    }
}
