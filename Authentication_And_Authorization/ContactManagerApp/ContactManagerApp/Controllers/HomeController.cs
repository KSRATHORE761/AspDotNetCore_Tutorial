using ContactManagerApp.Data;
using ContactManagerApp.Models;
using Microsoft.AspNet.Identity;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;

namespace ContactManagerApp.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly ApplicationDbContext _context;
        private readonly UserManager<IdentityUser> _userManager;
        private IAuthorizationService _authorizationService;

        public HomeController(ILogger<HomeController> logger, ApplicationDbContext context, UserManager<IdentityUser> userManager, IAuthorizationService authorizationService)
        {
            _logger = logger;
            _context = context;
            _userManager = userManager;
            _authorizationService = authorizationService;

        }
        public IActionResult Index()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }

        public async Task<IActionResult> OnGetAsync()
        {
            var contacts = from c in _context.Contacts
                           select c;
            var isAuthorized = User.IsInRole(Constants.Constants.ContactManagersRole) ||
                           User.IsInRole(Constants.Constants.ContactAdministratorsRole);

            var currentUserId = UserManager.GetUserId(User);
        }
    }
}
