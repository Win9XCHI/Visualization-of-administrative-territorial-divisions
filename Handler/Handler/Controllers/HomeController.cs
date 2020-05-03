using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Handler.Models;
using Microsoft.SqlServer.Types;

namespace Handler.Controllers {
    public class HomeController : Controller {
        IDBRepository repo;
        public HomeController(IDBRepository r) {
            repo = r;
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }

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

            List<InfoMaps> ob = repo.SELECT<InfoMaps>
                            ("ROW_NUMBER() OVER(PARTITION BY " + Name + ".Name ORDER BY Сoordinates.Counter) AS NumberRecord, " +
                            Name + ".Name, " + Name + ".Information, Years.Year_first, Years.Year_second, " +
                            "Сoordinates.Counter",
                            Name + " JOIN Midle ON " + Name + ".Midle_id = Midle.id JOIN Years ON Years.Midle_id = Midle.id " +
                            "JOIN Сoordinates ON(Сoordinates.Years_id = Years.id)",
                            "Years.Year_first < " + map.Year + " AND (Years.Year_second > " + map.Year + " OR Years.Year_second IS NULL)");

            List<SqlGeography> geo = repo.SELECT<SqlGeography>
                            ("ROW_NUMBER() OVER(PARTITION BY " + Name + ".Name ORDER BY Сoordinates.Counter) AS NumberRecord, Сoordinates.СoordinatesPoint",
                            Name + " JOIN Midle ON " + Name + ".Midle_id = Midle.id JOIN Years ON Years.Midle_id = Midle.id " +
                            "JOIN Сoordinates ON(Сoordinates.Years_id = Years.id)",
                            "Years.Year_first < " + map.Year + " AND (Years.Year_second > " + map.Year + " OR Years.Year_second IS NULL)");

            for (int i = 0; i < ob.Count; i++) {
                ob[i].СoordinatesPoint = geo[i];
            }
            return View(ob);
        }
    }
}
