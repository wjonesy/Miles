using MassTransit;
using Miles.MassTransit.MessageDeduplication;
using System;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;

namespace Miles.Sample.Persistence.EF.Access.Miles.MassTransit.MessageDeduplication
{
    public class ConsumedRepository : IConsumedRepository
    {
        private readonly SampleDbContext dbContext;

        public ConsumedRepository(SampleDbContext dbContext)
        {
            this.dbContext = dbContext;
        }

        public async Task<bool> RecordAsync(MessageContext context, string queueName)
        {
            dbContext.IncomingMessages.Add(new IncomingMessage(context.MessageId.Value, queueName, DateTime.Now));

            try
            {
                await dbContext.SaveChangesAsync();
                return false;
            }
            catch
            {
                return true;
            }
        }

        public async Task DeleteOldRecordsAsync()
        {
            var messages = await dbContext.IncomingMessages.Where(x => x.When < DateTime.Now.AddMonths(-1)).ToListAsync();
            dbContext.IncomingMessages.RemoveRange(messages);
            await dbContext.SaveChangesAsync();
        }
    }
}
