using Miles.Persistence;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Miles.Sample.Persistence.EF
{
    public class SampleTransactionContext : SimulateNestedTransactionContext
    {
        private readonly SampleDbContext dbContext;
        private DbContextTransaction transaction = null;

        public SampleTransactionContext(SampleDbContext dbContext)
        {
            this.dbContext = dbContext;
        }

        protected override Task DoBeginAsync()
        {
            transaction = dbContext.Database.BeginTransaction();
            return Task.FromResult(0);
        }

        protected override Task DoCommitAsync()
        {
            if (transaction != null)
                transaction.Commit();

            return Task.FromResult(0);
        }

        protected override void DoDispose()
        {
            if (transaction != null)
                transaction.Rollback();
        }

        protected override Task DoRollbackAsync()
        {
            if (transaction != null)
                transaction.Rollback();

            return Task.FromResult(0);
        }
    }
}
