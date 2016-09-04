using System.Threading.Tasks;

namespace Miles.Sample.Domain.Command.Leagues
{
    public interface ILeagueRepository
    {
        Task<League> GetByAbbreviationAsync(LeagueAbbreviation abbr);

        Task SaveAsync(League league);
    }
}
