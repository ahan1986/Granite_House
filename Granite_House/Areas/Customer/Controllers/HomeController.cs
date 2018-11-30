using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Granite_House.Models;
using Granite_House.Data;
using Microsoft.EntityFrameworkCore;

namespace Granite_House.Controllers
{   [Area("Customer")]
    public class HomeController : Controller
    {   
        // =============================================================
        // retrieve all of the products from the database
        private readonly ApplicationDbContext _db;
        //constructor to get dependency injection and get applicationDbContext
        public HomeController( ApplicationDbContext db )
        {
            _db = db;
        }
        // =============================================================

        //Index Action Method
        public async Task<IActionResult> Index()
        {   
            //1.
            var productList = await _db.Products.Include(m => m.ProductTypes).Include(m => m.SpecialTag).ToListAsync();

            return View(productList);
        }
        /*
         1. Goes into ApplicationDbContext.cs and finds Products where we have DbSet so that the database will have Products table
        */

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
