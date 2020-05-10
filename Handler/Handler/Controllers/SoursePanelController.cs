using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using Microsoft.AspNetCore.Http;
using Handler.Models;
using Handler.Algorithms;

namespace Handler.Controllers
{
    public class SoursePanelController : Controller
    {
        IDBRepository repo;
        public SoursePanelController(IDBRepository r)
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
            string where = "";

            if (SourseOb.Name != null)
            {
                where += "Name = '" + SourseOb.Name +"'";
            }

            if (SourseOb.Author != null)
            {
                if (where.Length > 0)
                {
                    where += " AND ";
                }

                where += "Author = '" + SourseOb.Author + "'";
            }

            if (SourseOb.Type != null) {
                if (where.Length > 0)
                {
                    where += " AND ";
                }

                where += "Type = '" + SourseOb.Type + "'";
            }

            if (SourseOb.Year != 0)
            {
                if (where.Length > 0)
                {
                    where += " AND ";
                }

                where += "YearCreate = '" + SourseOb.Year + "'";
            }

            if (SourseOb.YearRelevance != null)
            {
                if (where.Length > 0)
                {
                    where += " AND ";
                }

                where += "CAST(SUBSTRING('1900-2000', 1, 4) AS INT) < " + SourseOb.YearRelevance + " AND CAST(SUBSTRING('1900-2000', 6, 9) AS INT) > " + SourseOb.YearRelevance;
            }

            return View(repo.SELECT<Sourse>("idSourse AS id, Name, Type, Author, YearCreate AS Year, YearRelevance", "Sourse", where));
        }

        [HttpGet]
        public ActionResult DeleteSourse(int id)
        {
            List<int> M_IDs = repo.SELECT<int>("Local_point.Midle_id",
                "Local_point JOIN Sourse_LocalPoint ON (Local_point.idLP = Sourse_LocalPoint.Local_point_idLP) " +
"JOIN Sourse ON(Sourse.idSourse = Sourse_LocalPoint.Sourse_idSourse) WHERE Sourse.idSourse = 2 " +
"UNION SELECT Region.Midle_id FROM Region JOIN Sourse_Region ON(Region.idR = Sourse_Region.Region_idR) " +
"JOIN Sourse ON(Sourse.idSourse = Sourse_Region.Sourse_idSourse) WHERE Sourse.idSourse = 2 " +
"UNION SELECT Administrative_unit.Midle_id FROM Administrative_unit JOIN Sourse_Administrative_unit ON(Administrative_unit.idAU = Sourse_Administrative_unit.Administrative_unit_idAU) " +
"JOIN Sourse ON(Sourse.idSourse = Sourse_Administrative_unit.Sourse_idSourse) WHERE Sourse.idSourse = 2 " +
"UNION SELECT Country.Midle_id FROM Country JOIN Sourse_Country ON(Country.idC = Sourse_Country.Country_idC) " +
"JOIN Sourse ON(Sourse.idSourse = Sourse_Country.Sourse_idSourse)",
                "Sourse.idSourse = " + id);

            for (int i = 0; i < M_IDs.Count; i++)
            {
                repo.DELETE("Midle", "id = " + M_IDs[i]);
            }
            repo.DELETE("Sourse", "idSourse = " + id);

            return RedirectToAction("Index");
        }

        [HttpGet]
        public ActionResult EditSourse(int id, string name, string author, string type, string year1, string year2)
        {
            Sourse ObS = new Sourse { id = id, Name = name, Author = author, Type = type, Year = System.Int32.Parse(year1), YearRelevance = year2};
            return View(ObS);
        }

        [HttpPost]
        public ActionResult SaveEdit(Sourse ObS)
        {
            repo.UPDATE("Sourse", 
                "Name = '" + ObS.Name + "', Author = '" + ObS.Author + "', Type = '" + ObS.Type + "', YearCreate = '" + ObS.Year + "', YearRelevance = '" + ObS.YearRelevance + "'", 
                "idSourse = " + ObS.id);
            return RedirectToAction("Index");
        }

        [HttpPost]
        public ActionResult AddSourse()
        {
            return View();
        }

        [HttpPost]
        public async Task<ActionResult> NewSourseAsync(Sourse ObS)
        {
            SourseAnalysis Analysis = new SourseAnalysis(repo, ObS);
            await Task.Run(() => Analysis.Start());

            return View();
        }

    }
}
