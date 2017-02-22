using MassTransit;
using Miles.Sample.Domain.Command.Fixtures;
using Miles.Sample.Domain.Command.Leagues;
using Miles.Sample.Domain.Command.Teams;
using System;
using System.Threading.Tasks;

namespace Miles.Sample.Application
{
    public class FixtureFinishedProcessor : IConsumer<FixtureFinished>
    {
        private readonly ILeagueRepository leagueRepository;

        public FixtureFinishedProcessor(
            ILeagueRepository leagueRepository)
        {
            this.leagueRepository = leagueRepository;
        }

        async Task IConsumer<FixtureFinished>.Consume(ConsumeContext<FixtureFinished> context)
        {
            var teamA = TeamAbbreviation.Parse(context.Message.TeamA.Abbreviation);
            var teamB = TeamAbbreviation.Parse(context.Message.TeamB.Abbreviation);
            var leagueAbbr = LeagueAbbreviation.Parse(context.Message.League);
            var league = await leagueRepository.GetByAbbreviationAsync(leagueAbbr).ConfigureAwait(false);
            var result = (FixtureResults)Enum.ToObject(typeof(FixtureResults), context.Message.Result);

            league.RecordResult(result, teamA, context.Message.TeamA.Points, teamB, context.Message.TeamB.Points);
            await leagueRepository.SaveAsync(league).ConfigureAwait(false);
        }
    }
}
