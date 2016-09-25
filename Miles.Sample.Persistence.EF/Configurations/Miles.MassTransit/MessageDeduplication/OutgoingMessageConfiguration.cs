using Miles.MassTransit.MessageDeduplication;
using System.Data.Entity.ModelConfiguration;

namespace Miles.Sample.Persistence.EF.Configurations.Miles.MassTransit.MessageDeduplication
{
    class OutgoingMessageConfiguration : EntityTypeConfiguration<OutgoingMessage>
    {
        public OutgoingMessageConfiguration()
        {
            HasKey(x => x.MessageId);
            Property(x => x.ClassTypeName).IsRequired().HasMaxLength(255);
        }
    }
}
