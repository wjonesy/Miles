using Miles.Aggregates;
using Miles.Sample.Domain.Command.Teams;
using System.Collections.Generic;
using System;

namespace Miles.Sample.Domain.Command.Leagues
{
    public class LeagueState : IAppliesEvent<LeagueCreated>, IAppliesEvent<LeagueStarted>
    {
        public LeagueAbbreviation Id { get; private set; }

        public LeagueStates State { get; private set; }

        private readonly List<TeamAbbreviation> registeredTeamsList = new List<TeamAbbreviation>();
        public IEnumerable<TeamAbbreviation> RegisteredTeams => registeredTeamsList;

        public enum LeagueStates
        {
            Planning,
            InProgress,
            Completed
        }

        void IAppliesEvent<LeagueCreated>.ApplyEvent(LeagueCreated @event)
        {
            Id = @event.Id;
            State = LeagueStates.Planning;
        }

        void IAppliesEvent<LeagueStarted>.ApplyEvent(LeagueStarted @event)
        {
            State = LeagueStates.InProgress;
        }

        internal bool HasFixtureStarted(Guid fixtureId)
        {
            throw new NotImplementedException();
        }
    }
}
