using Miles.Persistence;
using Miles.Sample.Domain.Leagues;
using Miles.Sample.Domain.Teams;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Miles.Sample.Application.Command
{
    public class FixtureManager
    {
        private readonly IRepository<League> leagueRepository;

        public FixtureManager(IRepository<League> leagueRepository)
        {
            this.leagueRepository = leagueRepository;
        }

        public async Task StartFixtureAsync(Guid leagueId, Guid fixtureId, DateTime when)
        {
            var league = await leagueRepository.GetByIdAsync(leagueId).ConfigureAwait(false);
            league.StartFixture(fixtureId, when);
            await leagueRepository.SaveAsync(league);
        }

        public async Task RecordGoalAsync(Guid leagueId, Guid fixtureId, TeamAbbreviation team, DateTime when)
        {
            var league = await leagueRepository.GetByIdAsync(leagueId).ConfigureAwait(false);
            league.RecordGoal(fixtureId, team, when);
            await leagueRepository.SaveAsync(league);
        }

        public async Task FinishFixtureAsync(Guid leagueId, Guid fixtureId, DateTime when)
        {
            var league = await leagueRepository.GetByIdAsync(leagueId).ConfigureAwait(false);
            league.FinishFixture(fixtureId, when);
            await leagueRepository.SaveAsync(league);
        }
    }
}
