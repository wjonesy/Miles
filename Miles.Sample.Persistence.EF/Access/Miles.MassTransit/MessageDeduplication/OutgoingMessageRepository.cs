using Miles.MassTransit.MessageDeduplication;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;

namespace Miles.Sample.Persistence.EF.Access.Miles.MassTransit.MessageDeduplication
{
    public class OutgoingMessageRepository : IOutgoingMessageRepository
    {
        private readonly SampleDbContext dbContext;

        public OutgoingMessageRepository(SampleDbContext dbContext)
        {
            this.dbContext = dbContext;
        }

        public async Task SaveAsync(IEnumerable<OutgoingMessage> messages)
        {
            dbContext.OutgoingMessages.AddRange(messages);
            await dbContext.SaveChangesAsync();
        }

        public async Task DeleteOldRecordsAsync()
        {
            var expiryDate = DateTime.Now.AddDays(-3);
            var msgs = await dbContext.OutgoingMessages.Where(x => x.DispatchedDate.HasValue && x.DispatchedDate < expiryDate).ToListAsync();
            dbContext.OutgoingMessages.RemoveRange(msgs);
            await dbContext.SaveChangesAsync();
        }
    }
}
