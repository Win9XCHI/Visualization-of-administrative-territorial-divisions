using Microsoft.AspNetCore.Mvc;
using Handler.Models;
using Handler.Models.Repositories.Interfaces;
using Handler.Models.Search;
using System.Diagnostics;

namespace Handler.Controllers
{
    public class SearchController : Controller
    {
        ISearchRepository repo;
        public SearchController(ISearchRepository r)
        {
            repo = r;
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
        public ActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public ActionResult SearchYearView(FormSearch search)
        {
            try
            {
                return View(repo.SearchYearView(search));

            } catch (System.Exception) {
                return NotFound();
            }
        }

        [HttpPost]
        public ActionResult SearchChronologyView(FormSearch search)
        {
            try
            {
                return View(repo.SearchChronologyView(search));

            }
            catch (System.Exception)
            {
                return NotFound();
            }
        }

        [HttpPost]
        public ActionResult SearchSourseView(FormSearch search)
        {
            try
            {
                return View(repo.SearchSourseView(search));

            }
            catch (System.Exception)
            {
                return NotFound();
            }
        }
    }
}
