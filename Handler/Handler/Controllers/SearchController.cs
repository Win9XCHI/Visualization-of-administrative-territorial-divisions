using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Handler.Models;
using System.Diagnostics;

namespace Handler.Controllers
{
    public class SearchController : Controller
    {
        IDBRepository repo;
        public SearchController(IDBRepository r)
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

        private ActionResult Search(string Name, string columns, string table_name, string where, string limit = "", string order = "")
        {
            List<ResponseSearch> GeneralInfo = repo.SELECT<ResponseSearch>("Local_point.Name, Midle.id, Midle.Type",
                "Local_point JOIN Midle ON (Midle.id = Local_point.Midle_id) WHERE Local_point.Name LIKE '%" + Name + "%' OR Local_point.Name = '%" + Name + "' OR Local_point.Name = '" + Name + "%' OR Local_point.Name = '" + Name + "' " +
                "UNION SELECT Region.Name, Midle.id, Midle.Type FROM Region JOIN Midle ON(Midle.id = Region.Midle_id) WHERE Region.Name = '%" + Name + "%'  OR Region.Name = '%" + Name + "' OR Region.Name = '" + Name + "%' OR Region.Name = '" + Name + "' " +
                "UNION SELECT Administrative_unit.Name, Midle.id, Midle.Type FROM Administrative_unit JOIN Midle ON(Midle.id = Administrative_unit.Midle_id) WHERE Administrative_unit.Name = '%" + Name + "%' OR Administrative_unit.Name = '%" + Name + "' OR Administrative_unit.Name = '" + Name + "%' OR Administrative_unit.Name = '" + Name + "' " +
                "UNION SELECT Country.Name, Midle.id, Midle.Type FROM Country JOIN Midle ON(Midle.id = Country.Midle_id) WHERE Country.Name = '%" + Name + "%' OR Country.Name = '%" + Name + "' OR Country.Name = '" + Name + "%' OR Country.Name = '" + Name + "'");

            if (GeneralInfo.Count == 0)
            {
                return NotFound();
            }

            for (int i = 0; i < GeneralInfo.Count; i++)
            {
                switch (GeneralInfo[i].Type)
                {
                    case "Local_point":
                        {
                            GeneralInfo[i].ReferenceOut = repo.SELECT<Reference>("Region.Name, Region.idR AS ID",
                                "Region JOIN Local_point ON Local_point.Region_idR = Region.idR JOIN Midle ON (Midle.id =  Local_point.Midle_id)",
                                "Midle.id = " + GeneralInfo[0].id);
                            break;
                        }
                    case "Region":
                        {
                            GeneralInfo[i].ReferenceIn = repo.SELECT<Reference>("Local_point.Name, Local_point.idLP AS ID",
                                "Local_point JOIN Region ON Local_point.Region_idR = Region.idR JOIN Midle ON (Midle.id = Region.Midle_id)",
                                "Midle.id = " + GeneralInfo[0].id);

                            GeneralInfo[i].ReferenceOut = repo.SELECT<Reference>("Administrative_unit.Name, Administrative_unit.idAU AS ID",
                                "Administrative_unit JOIN Region_AU ON Administrative_unit.idAU = Region_AU.Administrative_unit_idAU JOIN Region ON Region.idR = Region_AU.Region_idR JOIN Midle ON(Midle.id = Region.Midle_id)",
                                "Midle.id = " + GeneralInfo[0].id);
                            break;
                        }
                    case "Administrative unit":
                        {
                            GeneralInfo[i].ReferenceIn = repo.SELECT<Reference>("Region.Name, Region.idR AS ID",
                                "Region JOIN Region_AU ON Region_AU.Region_idR = Region.idR JOIN Administrative_unit ON Administrative_unit.idAU = Region_AU.Administrative_unit_idAU JOIN Midle ON(Midle.id = Administrative_unit.Midle_id)",
                                "Midle.id = " + GeneralInfo[0].id);

                            GeneralInfo[i].ReferenceOut = repo.SELECT<Reference>("Country.Name, Country.idC AS ID",
                                "Country JOIN Administrative_unit ON Administrative_unit.Country_idC = Country.idC JOIN Midle ON (Midle.id = Administrative_unit.Midle_id)",
                                "Midle.id = " + GeneralInfo[0].id);
                            break;
                        }
                    case "Country":
                        {
                            GeneralInfo[i].ReferenceIn = repo.SELECT<Reference>("Administrative_unit.Name, Administrative_unit.idAU AS ID",
                                "Administrative_unit JOIN Country ON Administrative_unit.Country_idC = Country.idC JOIN Midle ON (Midle.id = Country.Midle_id)",
                                "Midle.id = " + GeneralInfo[0].id);
                            break;
                        }
                }

                GeneralInfo[i].ListRecords = repo.SELECT<RecordTableSearch>(columns, table_name, where + GeneralInfo[i].id, limit, order);
            }

            return View(GeneralInfo);
        }

        [HttpPost]
        public ActionResult SearchYearView(FormSearch search)
        {
            return Search(search.Name, "Part.Information, DetailsInformation.Year",
                "Part JOIN DetailsInformation ON (DetailsInformation.id = Part.DetailsInformation_id) JOIN Midle ON(Midle.id = DetailsInformation.Midle_id)",
                "YEAR(DetailsInformation.Year) = '" + search.Year + "' AND Midle.id = ");
        }

        [HttpPost]
        public ActionResult SearchChronologyView(FormSearch search)
        {
            return Search(search.Name, "Part.Information, DetailsInformation.Year",
                "Part JOIN DetailsInformation ON (DetailsInformation.id = Part.DetailsInformation_id) JOIN Midle ON(Midle.id = DetailsInformation.Midle_id)",
                "Midle.id = ", "", "DetailsInformation.Year");
        }

        [HttpPost]
        public ActionResult SearchSourseView(FormSearch search)
        {
            return Search(search.Name, "Sourse.Name AS Information",
                "Sourse JOIN Part ON (Part.Sourse_idSourse = Sourse.idSourse) JOIN DetailsInformation ON (DetailsInformation.id = Part.DetailsInformation_id) JOIN Midle ON(Midle.id = DetailsInformation.Midle_id)",
                "YEAR(DetailsInformation.Year) = '" + search.Year + "' AND Midle.id = ");
        }
    }
}
