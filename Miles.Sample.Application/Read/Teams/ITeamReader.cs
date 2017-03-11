using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Miles.Sample.Application.Read.Teams
{
    public interface ITeamReader
    {
        Task<List<string>> GetTeamsNotInLeagueAsync(string id);
    }
}
