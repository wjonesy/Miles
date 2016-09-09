using Miles.Sample.Application;
using Miles.Sample.Domain.Command.Fixtures;
using Miles.Sample.Domain.Command.Leagues;
using Miles.Sample.Domain.Command.Teams;
using Miles.Sample.Domain.Read.Leagues;
using Miles.Sample.Web.Models.Leagues;
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

        public FixturesController(LeagueManager leagueManager)
        {
            this.leagueManager = leagueManager;
        }

        [HttpPost]
        public async Task<ActionResult> Schedule(string leagueId, FixturesModel model)
        {
            var league = LeagueAbbreviation.Parse(leagueId);
            var teamA = TeamAbbreviation.Parse(model.TeamA);
            var teamB = TeamAbbreviation.Parse(model.TeamB);
            var scheduledDateTime = model.ScheduledDateTime;

            await leagueManager.ScheduleFixture(league, teamA, teamB, scheduledDateTime);

            return RedirectToAction("Fixtures", "Leagues", new { id = leagueId });
        }

        [HttpPost]
        public async Task<ActionResult> Start(string leagueId, string fixtureId)
        {
            var fixture = FixtureId.Parse(fixtureId);

            await leagueManager.StartFixture(fixture);

            return RedirectToAction("Fixtures", "Leagues", new { id = leagueId });
        }

        [HttpPost]
        public async Task<ActionResult> IncreasePoints(string leagueId, string fixtureId, string teamId, int points)
        {
            var fixture = FixtureId.Parse(fixtureId);
            var team = TeamAbbreviation.Parse(teamId);

            await leagueManager.IncreatePoints(fixture, team, points);

            return RedirectToAction("Fixtures", "Leagues", new { id = leagueId });
        }

        [HttpPost]
        public async Task<ActionResult> Finish(string leagueId, string fixtureId)
        {
            var fixture = FixtureId.Parse(fixtureId);

            await leagueManager.FinishFixture(fixture);

            return RedirectToAction("Fixtures", "Leagues", new { id = leagueId });
        }
    }
}