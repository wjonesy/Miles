﻿using Miles.Persistence;
using Miles.Sample.Domain.Teams;
using System.Threading.Tasks;

namespace Miles.Sample.Application.Command
{
    public class TeamManager
    {
        private readonly IRepository<Team> teamRepository;

        public TeamManager(IRepository<Team> teamRepository)
        {
            this.teamRepository = teamRepository;
        }

        public async Task CreateTeam(TeamAbbreviation id, string name)
        {
            var team = new Team(id, name);
            await teamRepository.SaveAsync(team);
        }
    }
}
