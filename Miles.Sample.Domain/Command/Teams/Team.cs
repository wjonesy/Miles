namespace Miles.Sample.Domain.Command.Teams
{
    public class Team
    {
        protected Team()
        { }

        public Team(TeamAbbreviation abbreviation, string name)
        {
            this.Abbreviation = abbreviation;
            this.Name = name;
        }

        public int SurrogateId { get; private set; }

        public TeamAbbreviation Abbreviation { get; private set; }

        public string Name { get; private set; }
    }
}
