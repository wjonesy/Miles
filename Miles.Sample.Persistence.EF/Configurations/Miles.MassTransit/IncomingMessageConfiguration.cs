using Miles.MassTransit;
using System;
using System.Collections.Generic;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Miles.Sample.Persistence.EF.Configurations.Miles.MassTransit
{
    class IncomingMessageConfiguration : EntityTypeConfiguration<IncomingMessage>
    {
        public IncomingMessageConfiguration()
        {
            HasKey(x => x.MessageId);
        }
    }
}
