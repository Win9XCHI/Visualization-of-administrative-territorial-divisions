using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using Handler.Models;
using Handler.Models.Repositories.Interfaces;
using Handler.Models.UserPanel;

namespace Handler.Controllers
{
    public class UserPanelController : Controller
    {
        IUserPanelRepository repo;
        public UserPanelController(IUserPanelRepository r)
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
            return View(repo.GetUsers(U));
        }

        [HttpGet]
        public async Task<ActionResult> DeleteUser(int code)
        {
            await Task.Run(() => repo.DeleteUser(code));

            return RedirectToAction("Index");
        }

        [HttpGet]
        public ActionResult EditSourse(int code, string pib, string phone, string birthday, string login, string rights)
        {
            return View(new User { Code = code, PIB = pib, Phone = phone, Birthday = birthday, Login = login, Rights = rights });
        }

        [HttpPost]
        public async Task<IActionResult> SaveEdit(User U)
        {
            await Task.Run(() => repo.UpdateUser(U));
            return Json("UserPanel/SearchUsers");
        }
    }
}
