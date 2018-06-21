using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using AuthRoutes.WebApplication.Models;
using Microsoft.AspNetCore.Authentication;
using AuthRoutes.Core.Services;
using AuthRoutes.Core;

namespace AuthRoutes.WebApplication.Controllers
{
    public class HomeController : Controller
    {
        private readonly IWebApiHttpClient _webApiHttpClient;
        public HomeController(IWebApiHttpClient webApiHttpClient)
        {
            _webApiHttpClient = webApiHttpClient;
        }

        public async Task<IActionResult> Index()
        {
            var result = await _webApiHttpClient.GetValues();

            var values = result.Result;
            ViewData["values"] = string.Join(",", values);
            return View();
        }

        public IActionResult About()
        {
            ViewData["Message"] = "Your application description page.";

            return View();
        }

        public IActionResult Contact()
        {
            ViewData["Message"] = "Your contact page.";

            return View();
        }

        public async Task SignOut()
        {
            await HttpContext.SignOutAsync("Cookies");
            await HttpContext.SignOutAsync("oidc");
        }

        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
