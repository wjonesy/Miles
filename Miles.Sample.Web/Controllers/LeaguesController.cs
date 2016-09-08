using Miles.Sample.Application;
using Miles.Sample.Domain.Command.Leagues;
using Miles.Sample.Domain.Command.Teams;
using Miles.Sample.Domain.Read.Leagues;
using Miles.Sample.Domain.Read.Teams;
using Miles.Sample.Web.Models.Leagues;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace Miles.Sample.Web.Controllers
{
    public class LeaguesController : Controller
    {
        private readonly LeagueManager leagueManager;
        private readonly ILeagueReader leagueReader;
        private readonly ITeamReader teamReader;

        public LeaguesController(
            LeagueManager leagueManager,
            ILeagueReader leagueReader,
            ITeamReader teamReader)
        {
            this.leagueManager = leagueManager;
            this.leagueReader = leagueReader;
            this.teamReader = teamReader;
        }

        // GET: Leagues
        public async Task<ActionResult> Index()
        {
            var leagues = await leagueReader.GetLeaguesAsync();
            return View(new IndexModel
            {
                Leagues = leagues.Select(x => new IndexModelLeague
                {
                    Abbreviation = x.Abbreviation,
                    Name = x.Name
                }).ToList()
            });
        }

        public ActionResult Create()
        {
            return View(new CreateModel());
        }

        [HttpPost]
        public async Task<ActionResult> Create(CreateModel model)
        {
            if (!ModelState.IsValid)
                return View(model);

            await leagueManager.CreateLeagueAsync(
                LeagueAbbreviation.Parse(model.Abbreviation),
                model.Name);

            return RedirectToAction("Index");
        }

        public async Task<ActionResult> Standings(string id)
        {
            var standings = await leagueReader.GetStandingsAsync(id);
            return View(new StandingsModel
            {
                Teams = standings.Select(x => new StandingModelTeam
                {
                    Name = x.Name,
                    Played = x.Played,
                    Wins = x.Wins,
                    Draws = x.Draws,
                    Losses = x.Losses,
                    PointsFor = x.PointsFor,
                    PointsAgainst = x.PointsAgainst,
                    Points = x.Points
                }).ToList()
            });
        }

        public async Task<ActionResult> RegisterTeam(string id)
        {
            var teams = await teamReader.GetTeamsNotInLeagueAsync(id);
            return View(new RegisterTeamModel
            {
                LeagueId = id,
                Teams = teams
            });
        }

        [HttpPost]
        public async Task<ActionResult> RegisterTeam(string id, RegisterTeamModel model)
        {
            if (!ModelState.IsValid)
            {
                var teams = await teamReader.GetTeamsNotInLeagueAsync(id);
                model.Teams = teams;
                return View(model);
            }

            var team = TeamAbbreviation.Parse(model.Team);
            var league = LeagueAbbreviation.Parse(id);

            await leagueManager.RegisterTeam(league, team);

            return RedirectToAction("Index");
        }
    }
}