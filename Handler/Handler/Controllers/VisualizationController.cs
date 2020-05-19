using System.Collections.Generic;
using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using Handler.Models;
using Handler.Models.Map;
using Handler.Models.Repositories.Interfaces;
//using Microsoft.SqlServer.Types;
using System.Text.Json;

namespace Handler.Controllers {
    public class VisualizationController : Controller {
        IVisualizationRepository repo;
        public VisualizationController(IVisualizationRepository r) {
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
        public IActionResult MapView(FormMap map)
        {
            string Name = "";
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
                        Name = "Local_point,Region,Administrative_unit,Country";
                        break;
                    }
            }

            string[] ex = { };

            if (map.Exeptions != null)
            {
                ex = map.Exeptions.Split(";");
            }

            List<InfoMaps> ob = repo.GetInformation(Name, map.Year.ToString(), ex);

            return Json(JsonSerializer.Serialize(ob));
        }
    }
}
