using Miles.Sample.Application;
using Miles.Sample.Domain.Command.Leagues;
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
    public class LeaguesController : Controller
    {
        private readonly LeagueManager leagueManager;
        private readonly ILeagueReader leagueReader;

        public LeaguesController(
            LeagueManager leagueManager,
            ILeagueReader leagueReader)
        {
            this.leagueManager = leagueManager;
            this.leagueReader = leagueReader;
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
    }
}