using Miles.MassTransit.MessageDeduplication;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;

namespace Miles.MassTransit.EntityFramework.MessageDeduplication
{
    public class OutgoingMessageRepository : IOutgoingMessageRepository
    {
        private readonly DbContext dbContext;
        private readonly TimeSpan expiryTimeSpan;

        public OutgoingMessageRepository(DbContext dbContext, TimeSpan expiryTimeSpan)
        {
            this.dbContext = dbContext;
        }

        public async Task SaveAsync(IEnumerable<OutgoingMessage> messages)
        {
            dbContext.Set<OutgoingMessage>().AddRange(messages);
            await dbContext.SaveChangesAsync();
        }

        public async Task DeleteOldRecordsAsync()
        {
            var expiryDate = DateTime.Now.Add(-expiryTimeSpan);
            var msgs = await dbContext.Set<OutgoingMessage>().Where(x => x.DispatchedDate.HasValue && x.DispatchedDate < expiryDate).ToListAsync();
            dbContext.Set<OutgoingMessage>().RemoveRange(msgs);
            await dbContext.SaveChangesAsync();
        }
    }
}
