using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Collections;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using Handler.Models;

namespace Handler.Controllers
{
    public class AuthenticationController : Controller
    {
        IDBRepository repo;
        public AuthenticationController(IDBRepository r)
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
        public ActionResult Check(LogIn User)
        {
            try
            {
                User = repo.SELECT<LogIn>("PIB, Rights", "Input", "Login = '" + User.Login + "' AND Password = '" + User.Password + "'")[0];
            }
            catch (System.ArgumentOutOfRangeException)
            {
                return RedirectToAction("Index");
            }
            return RedirectToAction("Index", "SoursePanel");
            //return View(User);
        }

        [HttpGet]
        public ActionResult Registration()
        {
            return View();
        }

        [HttpPost]
        public ActionResult NewUser(Register User)
        {
            ArrayList Columns = new ArrayList { "PIB", "Login", "Password", "Phone", "Birthday"};
            ArrayList Value = new ArrayList { "'" + User.PIB + "'", "'" + User.Login + "'", "'" + User.Password + "'", "'" + User.Phone + "'", "'" + User.Birthday + "'"};

            if (User.DiplomaF != null)
            {
                Columns.Add("Diploma");
                byte[] Data = null;
                using (var binaryReader = new BinaryReader(User.DiplomaF.OpenReadStream()))
                {
                    Data = binaryReader.ReadBytes((int)User.DiplomaF.Length);
                }
                User.Diploma = Data;
                Value.Add(User.Diploma);
            }

            repo.INSERT("Input", Columns, Value);

            return RedirectToAction("Index", "Home");
        }
    }
}
