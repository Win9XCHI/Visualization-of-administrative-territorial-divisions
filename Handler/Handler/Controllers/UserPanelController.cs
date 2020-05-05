using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using Handler.Models;

namespace Handler.Controllers
{
    public class UserPanelController : Controller
    {
        IDBRepository repo;
        public UserPanelController(IDBRepository r)
        {
            repo = r;
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }

        [HttpGet] //Index
        public ActionResult SearchUsers()
        {
            return View();
        }

        [HttpPost]
        public ActionResult ListUsers(User U)
        {
            string where = "";

            if (U.PIB != null)
            {
                where += "PIB = '" + U.PIB + "'";
            }

            if (U.Phone != null)
            {
                if (where.Length > 0)
                {
                    where += " AND ";
                }

                where += "Phone = '" + U.Phone + "'";
            }

            if (U.Login != null)
            {
                if (where.Length > 0)
                {
                    where += " AND ";
                }

                where += "Login = '" + U.Login + "'";
            }

            if (U.Birthday != null)
            {
                if (where.Length > 0)
                {
                    where += " AND ";
                }

                where += "Birthday = '" + U.Birthday + "'";
            }

            if (U.Rights != null)
            {
                if (where.Length > 0)
                {
                    where += " AND ";
                }
                where += "Rights = '" + U.Rights + "'";
            }

            return View(repo.SELECT<Sourse>("*", "Input", where));
        }

        [HttpGet]
        public ActionResult DeleteUser(int code)
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
                "Sourse.Input_Code = " + code);

            for (int i = 0; i < M_IDs.Count; i++)
            {
                repo.DELETE("Midle", "id = " + M_IDs[i]);
            }

            repo.DELETE("Input", "Code = " + code);

            return RedirectToAction("Index");
        }

        [HttpGet]
        public ActionResult EditSourse(int code, string pib, string phone, string birthday, string login, string rights)
        {
            User ObU = new User { Code = code, PIB = pib, Phone = phone, Birthday = birthday, Login = login, Rights = rights };
            return View(ObU);
        }

        [HttpPost]
        public ActionResult SaveEdit(User ObU)
        {
            repo.UPDATE("Input",
                "PIB = '" + ObU.PIB + "', Phone = '" + ObU.Phone + "', Birthday = '" + ObU.Birthday + "', Login = '" + ObU.Login + "', Rights = '" + ObU.Rights + "'",
                "Code = " + ObU.Code);
            return RedirectToAction("SearchUsers");
        }
    }
}
