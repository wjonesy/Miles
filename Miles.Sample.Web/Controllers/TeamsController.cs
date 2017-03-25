using Miles.Sample.Application.Command;
using Miles.Sample.Domain.Teams;
using Miles.Sample.Web.Models.Teams;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace Miles.Sample.Web.Controllers
{
    public class TeamsController : Controller
    {
        private readonly TeamManager teamManager;

        public TeamsController(TeamManager teamManager)
        {
            this.teamManager = teamManager;
        }

        // GET: Team
        public ActionResult Index()
        {
            // TODO: List of teams
            return View(new List<IndexModelTeam>());
        }

        // GET: Team/Create
        public ActionResult Create()
        {
            return View(new CreateModel());
        }

        // POST: Team/Create
        [HttpPost]
        public async Task<ActionResult> Create(CreateModel model)
        {
            if (!ModelState.IsValid)
                return View(model);

            await teamManager.CreateTeam(TeamAbbreviation.Parse(model.Abbreviation), model.Name);

            return RedirectToAction("Index");
        }
    }
}
