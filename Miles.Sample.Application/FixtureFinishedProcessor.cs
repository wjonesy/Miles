using Miles.Messaging;
using Miles.Sample.Domain.Command.Fixtures;
using Miles.Sample.Domain.Command.Leagues;
using Miles.Sample.Domain.Command.Teams;
using System;
using System.Threading.Tasks;

namespace Miles.Sample.Application
{
    public class FixtureFinishedProcessor : IMessageProcessor<FixtureFinished>
    {
        private readonly ILeagueRepository leagueRepository;

        public FixtureFinishedProcessor(
            ILeagueRepository leagueRepository)
        {
            this.leagueRepository = leagueRepository;
        }

        public async Task ProcessAsync(FixtureFinished message)
        {
            var teamA = TeamAbbreviation.Parse(message.TeamA.Abbreviation);
            var teamB = TeamAbbreviation.Parse(message.TeamB.Abbreviation);
            var leagueAbbr = LeagueAbbreviation.Parse(message.League);
            var league = await leagueRepository.GetByAbbreviationAsync(leagueAbbr);
            var result = (FixtureResults)Enum.ToObject(typeof(FixtureResults), message.Result);

            league.RecordResult(result, teamA, message.TeamA.Points, teamB, message.TeamB.Points);
            await leagueRepository.SaveAsync(league);
        }
    }
}
