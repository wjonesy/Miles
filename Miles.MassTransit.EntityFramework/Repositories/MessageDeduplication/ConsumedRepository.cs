using MassTransit;
using Miles.MassTransit.MessageDeduplication;
using System;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;

namespace Miles.MassTransit.EntityFramework.MessageDeduplication
{
    public class ConsumedRepository : IConsumedRepository
    {
        private readonly DbContext dbContext;

        public ConsumedRepository(DbContext dbContext)
        {
            this.dbContext = dbContext;
        }

        public async Task<bool> RecordAsync(MessageContext context, string queueName)
        {
            dbContext.Set<IncomingMessage>().Add(new IncomingMessage(context.MessageId.Value, queueName, DateTime.Now));

            try
            {
                await dbContext.SaveChangesAsync().ConfigureAwait(false);
                return true;
            }
            catch
            {
                return false;
            }
        }

        public async Task DeleteOldRecordsAsync()
        {
            var messages = await dbContext.Set<IncomingMessage>().Where(x => x.When < DateTime.Now.AddMonths(-1)).ToListAsync().ConfigureAwait(false);
            dbContext.Set<IncomingMessage>().RemoveRange(messages);
            await dbContext.SaveChangesAsync().ConfigureAwait(false);
        }
    }
}
