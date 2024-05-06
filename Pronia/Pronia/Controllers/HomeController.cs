using Microsoft.AspNetCore.Mvc;
using Pronia.Helper;

namespace Pronia.Controllers
{
    public class HomeController : Controller
    {
        public async Task<IActionResult> Index()
        {
            var data = HelpMethod.GetDataQuery("SELECT * FROM Cards");
            List<string> title = [];
            for (int i = 0; i < 3; i++)
            {
                title.Add(data.Rows[i][1].ToString());
            }
            return View(title);
        }

        public IActionResult About()
        {
            return View();
        }
    }
}
