using Microsoft.AspNetCore.Mvc;
using Handler.Models;
using Handler.Models.Repositories.Interfaces;
using Handler.Models.Search;
using System.Diagnostics;
using System.Collections.Generic;

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
            FormSearch Ob = new FormSearch();
            return View(Ob);
        }

        [HttpGet]
        public ActionResult SearchView(FormSearch search)
        {
            try
            {
                switch (search.SelectedOption) {
                    case 1:
                        {
                            return RedirectToAction("SearchYearView", "Search", search);
                        }
                    case 2:
                        {
                            return RedirectToAction("SearchSourseView", "Search", search);
                        }
                    case 3:
                        {
                            return RedirectToAction("SearchChronologyView", "Search", search);
                        }
                    default:
                        {
                            throw new System.Exception();
                        }
                }
            }
            catch (System.Exception)
            {
                return NotFound();
            }
        }

        [HttpGet]
        public ActionResult SearchYearView(FormSearch search)
        {
            try
            {
                List<ResponseSearch> Ob = repo.SearchYearView(search);
                if (Ob.Count == 0)
                {
                    throw new System.Exception();
                }
                return View(Ob);

            } catch (System.Exception) {
                return NotFound();
            }
        }

        [HttpGet]
        public ActionResult SearchChronologyView(FormSearch search)
        {
            try
            {
                List<ResponseSearch> Ob = repo.SearchChronologyView(search);
                if (Ob.Count == 0)
                {
                    throw new System.Exception();
                }
                return View(Ob);

            }
            catch (System.Exception)
            {
                return NotFound();
            }
        }

        [HttpGet]
        public ActionResult SearchSourseView(FormSearch search)
        {
            try
            {
                List<ResponseSearch> Ob = repo.SearchSourseView(search);
                if (Ob.Count == 0)
                {
                    throw new System.Exception();
                }
                return View(Ob);

            }
            catch (System.Exception)
            {
                return NotFound();
            }
        }

        [HttpGet]
        public ActionResult DetailsInfo(string name)
        {
            FormSearch search = new FormSearch {
                Name = name,
                SelectedOption = 3
            };

            return RedirectToAction("SearchView", "Search", search);
        }
    }
}
