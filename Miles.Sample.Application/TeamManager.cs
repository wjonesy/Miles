using Miles.Persistence;
using Miles.Sample.Domain.Command.Teams;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Miles.Sample.Application
{
    public class TeamManager
    {
        private readonly ITransactionContext transactionContext;
        private readonly ITeamRepository teamRepository;

        public TeamManager(
            ITransactionContext transactionContext,
            ITeamRepository teamRepository)
        {
            this.transactionContext = transactionContext;
            this.teamRepository = teamRepository;
        }

        public async Task CreateTeam(TeamAbbreviation teamAbbr, string name)
        {
            using (var transaction = await transactionContext.BeginAsync())
            {
                var team = new Team(teamAbbr, name);
                await teamRepository.SaveAsync(team);

                await transaction.CommitAsync();
            }
        }
    }
}
