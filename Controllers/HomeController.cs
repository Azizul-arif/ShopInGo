using ECommerce.Data;
using ECommerce.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Diagnostics;

namespace ECommerce.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
		private readonly ApplicationDbContext _context;
		private readonly IWebHostEnvironment _environment;

		public HomeController(ILogger<HomeController> logger, ApplicationDbContext context, IWebHostEnvironment environment )
        {
            _logger = logger;
			_context = context;
			_environment = environment;
		}

        public async Task<IActionResult>  Index()
        {
			var applicationDbContext = _context.ProductImages.Include(p => p.Product);
			return View(await applicationDbContext.ToListAsync());
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
    }
}
