using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using Microsoft.AspNetCore.Http;
using Handler.Models;
using Handler.Models.SoursePanel;
using Handler.Models.Repositories.Interfaces;
using Handler.Algorithms;

namespace Handler.Controllers
{
    public class SoursePanelController : Controller
    {
        ISoursePanelRepository repo;
        public SoursePanelController(ISoursePanelRepository r)
        {
            repo = r;
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }

        [HttpGet]
        public ActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public ActionResult ListSources(Sourse SourseOb)
        {
            return View(repo.GetSources(SourseOb));
        }

        [HttpGet]
        public async Task<ActionResult> DeleteSourse(int id)
        {
            await Task.Run(() => repo.DeleteSourse(id));

            return View();
        }

        [HttpGet]
        public ActionResult EditSourse(int id, string name, string author, string type, string year1, string year2)
        { 
            return View(new Sourse { id = id, Name = name, Author = author, Type = type, Year = System.Int32.Parse(year1), YearRelevance = year2 });
        }

        [HttpPost]
        public async Task<IActionResult> SaveEdit(Sourse ObS)
        {
            await Task.Run(() => repo.UpdateSourse(ObS));
            return Json("'SoursePanel/Index'");
        }

        [HttpGet]
        public ActionResult AddSourse()
        {
            return View();
        }

        [HttpPost]
        public async Task<ActionResult> NewSourseAsync(Sourse ObS)
        {
            SourseAnalysis Analysis = new SourseAnalysis(repo, ObS);
            await Task.Run(() => Analysis.Start());

            return RedirectToAction("Index");
        }

    }
}
