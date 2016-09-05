using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Miles.Sample.Domain.Command.Teams
{
    public interface ITeamRepository
    {
        Task SaveAsync(Team team);
    }
}
