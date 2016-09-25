using Miles.MassTransit.MessageDeduplication;
using System.Collections.Generic;
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
    }
}
