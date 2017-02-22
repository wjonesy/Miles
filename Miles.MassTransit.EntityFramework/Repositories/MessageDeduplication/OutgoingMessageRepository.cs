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
        private readonly TimeSpan expiryTimeSpan = TimeSpan.FromDays(3);

        public OutgoingMessageRepository(DbContext dbContext)
        {
            this.dbContext = dbContext;
        }

        public async Task SaveAsync(IEnumerable<OutgoingMessage> messages)
        {
            dbContext.Set<OutgoingMessage>().AddRange(messages);
            await dbContext.SaveChangesAsync().ConfigureAwait(false);
        }

        public async Task DeleteOldRecordsAsync()
        {
            var expiryDate = DateTime.Now.Add(-expiryTimeSpan);
            var msgs = await dbContext.Set<OutgoingMessage>().Where(x => x.DispatchedDate.HasValue && x.DispatchedDate < expiryDate).ToListAsync().ConfigureAwait(false);
            dbContext.Set<OutgoingMessage>().RemoveRange(msgs);
            await dbContext.SaveChangesAsync().ConfigureAwait(false);
        }
    }
}
