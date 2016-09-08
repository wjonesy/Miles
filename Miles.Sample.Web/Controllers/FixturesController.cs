using Miles.Sample.Application;
using Miles.Sample.Domain.Command.Leagues;
using Miles.Sample.Domain.Command.Teams;
using Miles.Sample.Domain.Read.Leagues;
using Miles.Sample.Web.Models.Fixtures;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace Miles.Sample.Web.Controllers
{
    public class FixturesController : Controller
    {
        private readonly LeagueManager leagueManager;
        private readonly ILeagueReader leagueReader;

        public FixturesController(
            LeagueManager leagueManager,
            ILeagueReader leagueReader)
        {
            this.leagueManager = leagueManager;
            this.leagueReader = leagueReader;
        }

        public async Task<ActionResult> Index(string id)
        {
            var teams = await leagueReader.GetTeamsAsync(id);
            var fixtures = await leagueReader.GetFixturesAsync(id);
            return View(new IndexModel
            {
                Teams = teams,
                Fixtures = fixtures.Select(x => new IndexModelFixture
                {
                    TeamA = x.TeamA,
                    TeamAPoints = x.TeamAPoints,
                    TeamB = x.TeamB,
                    TeamBPoints = x.TeamBPoints,
                    ScheduledDateTime = x.ScheduledDateTime,
                    Active = x.Active,
                    Completed = x.Completed
                }).ToList()
            });
        }

        [HttpPost]
        public async Task<ActionResult> Schedule(string leagueId, IndexModel model)
        {
            var league = LeagueAbbreviation.Parse(leagueId);
            var teamA = TeamAbbreviation.Parse(model.TeamA);
            var teamB = TeamAbbreviation.Parse(model.TeamB);
            var scheduledDateTime = model.ScheduledDateTime;

            await leagueManager.ScheduleFixture(league, teamA, teamB, scheduledDateTime);

            return RedirectToAction("Index", new { leagueId });
        }
    }
}