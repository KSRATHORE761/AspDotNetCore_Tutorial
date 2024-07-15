using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Text.Encodings.Web;

namespace MvcMovie.Controllers
{
    public class HelloWorldController : Controller
    {
        //Get : /HelloWorld
        public IActionResult Index()
        {
            return View();
        }
        //Get : /HelloWorld/Welcome
        public IActionResult Welcome(string name, int numtimes = 1)
        {
            ViewData["Message"] = "Hello " + name;
            ViewData["numTimes"] = numtimes;
            return View();
        }
    }
}
