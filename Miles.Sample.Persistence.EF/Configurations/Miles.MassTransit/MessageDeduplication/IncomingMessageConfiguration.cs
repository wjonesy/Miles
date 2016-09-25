using Miles.Sample.Persistence.EF.Access.Miles.MassTransit.MessageDeduplication;
using System.Data.Entity.ModelConfiguration;

namespace Miles.Sample.Persistence.EF.Configurations.Miles.MassTransit.MessageDeduplication
{
    class IncomingMessageConfiguration : EntityTypeConfiguration<IncomingMessage>
    {
        public IncomingMessageConfiguration()
        {
            HasKey(x => x.MessageId);
        }
    }
}
