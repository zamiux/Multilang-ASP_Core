using Microsoft.AspNetCore.Localization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Localization;
using Multilang.Context;
using Multilang.Models;
using System.Diagnostics;
using System.Globalization;

namespace Multilang.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly MultilangDBContext _context;
        IStringLocalizer<HomeController> _localizer;
        public HomeController(ILogger<HomeController> logger,
            MultilangDBContext context,
            IStringLocalizer<HomeController> localizer)
        {
            _context = context;
            _logger = logger;
            _localizer = localizer;
        }

        public IActionResult Index()
        {
            string lang = CultureInfo.CurrentCulture.Name;
            // modelBuilder.Entity<New>().HasQueryFilter(n => n.Language.LangTitle == lang);

            return View(_context.News.Where(n=>n.Language.LangTitle == lang));
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }

        public PartialViewResult Languages()
        {
            return PartialView(_context.Languages);
        }

        public IActionResult ChangeLanguages(string culture)
        {
            Response.Cookies.Append(
                CookieRequestCultureProvider.DefaultCookieName,
                CookieRequestCultureProvider.MakeCookieValue(new RequestCulture(culture)),
                new CookieOptions()
                {
                    Expires = DateTimeOffset.Now.AddYears(1)
                });
            return Redirect(Request.Headers["Referer"].ToString());
        }
    }
}