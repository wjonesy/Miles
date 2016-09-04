using System;
using System.Text.RegularExpressions;

namespace Miles.Sample.Domain.Command.Teams
{
    public class TeamAbbreviation
    {
        private static readonly Regex Valid = new Regex("[a-z]+", RegexOptions.Singleline | RegexOptions.IgnoreCase);

        public static TeamAbbreviation Parse(string abbr)
        {
            TeamAbbreviation teamAbbr;
            if (!TryParse(abbr, out teamAbbr))
                throw new FormatException("The supplied abbreviation is not correctly formatted.");

            return teamAbbr;
        }

        public static bool TryParse(string abbr, out TeamAbbreviation teamAbbr)
        {
            teamAbbr = null;
            if (!Valid.IsMatch(abbr))
                return false;

            teamAbbr = new TeamAbbreviation(abbr);
            return true;
        }

        protected TeamAbbreviation()
        { }

        public TeamAbbreviation(string abbreviation)
        {
            if (!Valid.IsMatch(abbreviation))
                throw new ArgumentOutOfRangeException("abbreviation");

            this.Abbreviation = abbreviation;
        }

        public string Abbreviation { get; private set; }

        public override bool Equals(object obj)
        {
            if (obj == null)
                return false;

            var otherObj = obj as TeamAbbreviation;
            if (otherObj == null)
                return false;

            return this.Abbreviation.Equals(otherObj.Abbreviation);
        }

        public override int GetHashCode()
        {
            return this.Abbreviation.GetHashCode();
        }

        public override string ToString()
        {
            return this.Abbreviation;
        }
    }
}
