using Miles.MassTransit;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Miles.Sample.Persistence.EF.Miles.MassTransit
{
    public class IncomingMessageRepository : IIncomingMessageRepository
    {
        private readonly SampleDbContext dbContext;

        public IncomingMessageRepository(SampleDbContext dbContext)
        {
            this.dbContext = dbContext;
        }

        public async Task DeleteOldAsync()
        {
            var messages = await dbContext.IncomingMessages.Where(x => x.When < DateTime.Now.AddMonths(-1)).ToListAsync();
            dbContext.IncomingMessages.RemoveRange(messages);
            await dbContext.SaveChangesAsync();
        }

        public async Task<bool> RecordAsync(IncomingMessage incomingMessage)
        {
            dbContext.IncomingMessages.Add(incomingMessage);

            try
            {
                await dbContext.SaveChangesAsync();
                return true;
            }
            catch
            {
                return false;
            }
        }
    }
}
