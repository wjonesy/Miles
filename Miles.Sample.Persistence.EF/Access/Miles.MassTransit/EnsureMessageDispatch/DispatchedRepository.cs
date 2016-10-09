using MassTransit;
using Miles.MassTransit.RecordMessageDispatch;
using System;
using System.Configuration;
using System.Data.SqlClient;
using System.Threading.Tasks;

namespace Miles.Sample.Persistence.EF.Access.Miles.MassTransit.RecordMessageDispatch
{
    public class DispatchedRepository : IDispatchedRepository
    {
        public async Task RecordAsync(SendContext context)
        {
            using (var connection = new SqlConnection(ConfigurationManager.ConnectionStrings["Miles.Sample"].ConnectionString))
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
