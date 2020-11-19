using Microsoft.AspNetCore.Mvc;

namespace RacingSite.Controllers
{
    public class HomeController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }

        public IActionResult RealTimePage()
        {
            return View();
        }
    }
}
