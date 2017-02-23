using Miles.Persistence;
using Miles.Sample.Domain.Command.Fixtures;
using Miles.Sample.Domain.Command.Leagues;
using Miles.Sample.Domain.Command.Teams;
using System;
using System.Threading.Tasks;

namespace Miles.Sample.Application
{
    public class LeagueManager
    {
        private readonly ITransactionContext transactionContext;
        private readonly ILeagueRepository leagueRepository;
        private readonly IFixtureRepository fixtureRepository;
        private readonly DomainContext domainContext;

        public LeagueManager(
            ITransactionContext transactionContext,
            ILeagueRepository leagueRepository,
            IFixtureRepository fixtureRepository,
            DomainContext domainContext)
        {
            this.transactionContext = transactionContext;
            this.leagueRepository = leagueRepository;
            this.fixtureRepository = fixtureRepository;
            this.domainContext = domainContext;
        }

        public async Task CreateLeagueAsync(LeagueAbbreviation abbr, string name)
        {
            using (var transaction = await transactionContext.BeginAsync())
            {
                var league = new League(
                    abbr,
                    name);
                await leagueRepository.SaveAsync(league);

                await transaction.CommitAsync();
            }
        }

        public async Task RegisterTeam(LeagueAbbreviation leagueAbbr, TeamAbbreviation teamAbbr)
        {
            using (var transaction = await transactionContext.BeginAsync())
            {
                var league = await leagueRepository.GetByAbbreviationAsync(leagueAbbr);
                league.RegisterTeam(teamAbbr);
                await leagueRepository.SaveAsync(league);

                await transaction.CommitAsync();
            }
        }

        public async Task ScheduleFixture(LeagueAbbreviation leagueAbbr, TeamAbbreviation teamAAbbr, TeamAbbreviation teamBAbbr, DateTime scheduledDateTime)
        {
            using (var transaction = await transactionContext.BeginAsync())
            {
                var league = await leagueRepository.GetByAbbreviationAsync(leagueAbbr);
                var fixture = league.ScheduleFixture(domainContext, teamAAbbr, teamBAbbr, scheduledDateTime);
                await fixtureRepository.SaveAsync(fixture);

                await transaction.CommitAsync();
            }
        }

        public async Task StartFixture(FixtureId fixtureId)
        {
            using (var transaction = await transactionContext.BeginAsync())
            {
                var fixture = await fixtureRepository.GetByIdAsync(fixtureId);
                fixture.Start(domainContext);
                await fixtureRepository.SaveAsync(fixture);

                await transaction.CommitAsync();
            }
        }

        public async Task IncreatePoints(FixtureId fixtureId, TeamAbbreviation team, int points)
        {
            using (var transaction = await transactionContext.BeginAsync())
            {
                var fixture = await fixtureRepository.GetByIdAsync(fixtureId);
                fixture.IncreasePoints(team, points);
                await fixtureRepository.SaveAsync(fixture);

                await transaction.CommitAsync();
            }
        }

        public async Task FinishFixture(FixtureId fixtureId)
        {
            using (var transaction = await transactionContext.BeginAsync())
            {
                var fixture = await fixtureRepository.GetByIdAsync(fixtureId);
                fixture.Finish(domainContext);
                await fixtureRepository.SaveAsync(fixture);

                await transaction.CommitAsync();
            }
        }
    }
}
