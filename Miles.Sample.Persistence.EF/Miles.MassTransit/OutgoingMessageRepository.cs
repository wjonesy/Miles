using Miles.MassTransit;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Miles.Sample.Persistence.EF.Miles.MassTransit
{
    public class OutgoingMessageRepository : IOutgoingMessageRepository
    {
        private readonly SampleDbContext dbContext;

        public OutgoingMessageRepository(SampleDbContext dbContext)
        {
            this.dbContext = dbContext;
        }

        public async Task DeleteOldDispatchedAsync()
        {
            var messages = await dbContext.OutgoingMessages
                .Where(x => x.EventDispatched != null && x.EventDispatched < DateTime.Now.AddMonths(-1)).ToListAsync();
            dbContext.OutgoingMessages.RemoveRange(messages);
            await dbContext.SaveChangesAsync();
        }

        public async Task RecordMessageDispatchAsync(DateTime when, IEnumerable<Guid> messageIds)
        {
            var messages = await dbContext.OutgoingMessages.Where(x => messageIds.Contains(x.MessageId)).ToListAsync();
            foreach (var message in messages)
                message.Dispatched(when);
            await dbContext.SaveChangesAsync();
        }

        public async Task RecordMessageDispatchAsync(DateTime when, Guid messageId)
        {
            var message = await dbContext.OutgoingMessages.FindAsync(messageId);
            message.Dispatched(when);
            await dbContext.SaveChangesAsync();
        }

        public async Task SaveAsync(IEnumerable<OutgoingMessage> messages)
        {
            dbContext.OutgoingMessages.AddRange(messages);
            await dbContext.SaveChangesAsync();
        }
    }
}
