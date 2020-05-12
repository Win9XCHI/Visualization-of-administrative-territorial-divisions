using System.Collections.Generic;
using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using Handler.Models;
using Handler.Models.Map;
using Handler.Models.Repositories.Interfaces;
using Microsoft.SqlServer.Types;

namespace Handler.Controllers {
    public class HomeController : Controller {
        IHome_mapRepository repo;
        public HomeController(IHome_mapRepository r) {
            repo = r;
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }

        [HttpGet]
        public ActionResult Index() {
           return  View();
        }

        [HttpPost]
        public ActionResult MapView(FormMap map)
        {
            string Name;
            switch (map.Level) {
                case 1:
                    {
                        Name = "Local_point";
                        break;
                    }
                case 2:
                    {
                        Name = "Region";
                        break;
                    }
                case 3:
                    {
                        Name = "Administrative_unit";
                        break;
                    }
                case 4:
                    {
                        Name = "Country";
                        break;
                    }
                default:
                    {
                        return NotFound();
                    }
            }

            List<InfoMaps> ob = repo.GetInformation(Name, map.Year.ToString());
            List<SqlGeography> geo = repo.GetCoordinates(Name, map.Year.ToString());

            for (int i = 0; i < ob.Count; i++) {
                ob[i].СoordinatesPoint = geo[i];
            }
            return View(ob);
        }
    }
}
