using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using Handler.Models;
using Handler.Models.Authentication;
using Handler.Models.Repositories.Interfaces;
using Microsoft.AspNetCore.Http;

namespace Handler.Controllers
{
    public class AuthenticationController : Controller
    {
        IAuthenticationRepository repo;
        public AuthenticationController(IAuthenticationRepository r)
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
                User = repo.GetUser(User);
                Response.Cookies.Append("code", User.Code.ToString());
            }
            catch (System.ArgumentOutOfRangeException)
            {
                return RedirectToAction("Index");
            }
            return RedirectToAction("Index", "SoursePanel");
        }

        [HttpGet]
        public ActionResult Registration()
        {
            return View();
        }

        [HttpPost]
        public ActionResult NewUser(Register User)
        {
            repo.AddUser(User);

            return RedirectToAction("Index", "Home");
        }
    }
}
