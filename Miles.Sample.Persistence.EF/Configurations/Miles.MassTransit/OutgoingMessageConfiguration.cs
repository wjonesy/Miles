using Miles.MassTransit;
using System;
using System.Collections.Generic;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Miles.Sample.Persistence.EF.Configurations.Miles.MassTransit
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
