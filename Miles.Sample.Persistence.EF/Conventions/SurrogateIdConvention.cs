using System;
using System.Collections.Generic;
using System.Data.Entity.ModelConfiguration.Conventions;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Miles.Sample.Persistence.EF.Conventions
{
    class SurrogateIdConvention : Convention
    {
        public SurrogateIdConvention()
        {
            Properties<int>().Where(x => x.Name == "SurrogateId").Configure(x => x.HasColumnName("Id").IsKey());
        }
    }
}
