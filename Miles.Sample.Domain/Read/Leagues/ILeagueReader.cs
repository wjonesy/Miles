using Miles.Sample.Domain.Command.Leagues;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Miles.Sample.Domain.Read.Leagues
{
    public interface ILeagueReader
    {
        Task<List<League>> GetLeaguesAsync();

        Task<List<Standing>> GetStandingsAsync(string id);

        Task<List<string>> GetTeamsAsync(string leagueId);

        Task<List<Fixture>> GetFixturesAsync(string leagueId);
    }
}
