using Miles.Aggregates;

namespace Miles.Sample.Domain.Command.Teams
{
    public class TeamState : IAppliesEvent<TeamCreated>
    {
        public TeamAbbreviation Id { get; private set; }

        void IAppliesEvent<TeamCreated>.ApplyEvent(TeamCreated @event)
        {
            this.Id = @event.Id;
        }
    }
}