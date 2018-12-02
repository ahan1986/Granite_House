using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Granite_House.Models;
using Granite_House.Data;
using Microsoft.EntityFrameworkCore;
using Granite_House.Extensions;
using Microsoft.AspNetCore.Http;
using Granite_House.Extensions;

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
            //1. Goes into ApplicationDbContext.cs and finds Products where we have DbSet so that the database will have Products table
            var productList = await _db.Products.Include(m => m.ProductTypes).Include(m => m.SpecialTag).ToListAsync();

            return View(productList);
        }

        //GET action method for Details Page
        public async Task<IActionResult> Details(int id)
        {   // on the Index.cshtml in the "a" tag, we pass the "id" when we use "asp-route-id" and pass in the product.Id. So that Id becomes the int id in the argument for Details. We have to also tell which action the imformation is being sent to so "asp-action" and which controller "asp-controller"
            ViewData["Message"] = "Your application description page.";

            var product = await _db.Products.Include(m => m.ProductTypes).Include(m => m.SpecialTag).Where(m=>m.Id == id).FirstOrDefaultAsync();

            return View(product);
        }

        //POST action method for Details Page
        [HttpPost, ActionName("Details")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DetailsPost( int id )
        {
            //1.
            List<int> lstShoppingCart = HttpContext.Session.Get<List<int>>("ssShoppingCart");
            //2.
            if (lstShoppingCart == null) 
            {
                lstShoppingCart = new List<int>();
            }
            //3.
            lstShoppingCart.Add(id);
            //4.
            HttpContext.Session.Set("ssShoppingCart", lstShoppingCart);
            //5.
            return RedirectToAction("Index", "Home", new { area = "Customer" });
        }
        /* 
         1. check is anything is in the session and try to retrieve if anything is in here. "ssShoppingCart" will be used throughout the website. We need to pass what is the object in the session.get. We will find that it's a list of integers and a session name which is ssShoppingCart
         2. first time is obviously null
         3. add lstshoppingcart with the id we grabbed from the View.
         4. set our session by adding it to our session name "ssShoppingCart" to the value lstShoppingCart.
         5. redirecttoAction.  Action = Index, Controller = Home, area = Customer.  The area will direct to go into the Customer folder instead of Admin and etc.
         6.  to find out if we stored anything into our session storage go into Views/Shared/_Layout.cshtml.  More notes about checking our session storage there.
             */

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
