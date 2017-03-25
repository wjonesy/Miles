using Miles.Persistence;
using Miles.Sample.Domain.Leagues;
using Miles.Sample.Domain.Teams;
using System;
using System.Threading.Tasks;

namespace Miles.Sample.Application.Command
{
    public class LeagueManager
    {
        private readonly IRepository<League, LeagueAbbreviation> leagueRepository;

        public LeagueManager(IRepository<League, LeagueAbbreviation> leagueRepository)
        {
            this.leagueRepository = leagueRepository;
        }

        public async Task CreateLeagueAsync(LeagueAbbreviation id, string name)
        {
            var league = new League(id, name);
            await leagueRepository.SaveAsync(league);
        }

        public async Task RegisterTeamAsync(LeagueAbbreviation id, TeamAbbreviation team)
        {
            var league = await leagueRepository.GetByIdAsync(id);
            league.RegisterTeam(team);
            await leagueRepository.SaveAsync(league);
        }

        public async Task ScheduleFixtureAsync(LeagueAbbreviation id, TeamAbbreviation teamA, TeamAbbreviation teamB, DateTime scheduledDateTime)
        {
            var league = await leagueRepository.GetByIdAsync(id);
            league.ScheduleFixture(teamA, teamB, scheduledDateTime);
            await leagueRepository.SaveAsync(league);
        }
    }
}
