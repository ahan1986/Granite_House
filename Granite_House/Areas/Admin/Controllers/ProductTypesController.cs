using Granite_House.Data;
using Granite_House.Models;
using Microsoft.AspNetCore.Mvc;
using System.Linq;
using System.Threading.Tasks;

namespace Granite_House.Areas.Admin.Controllers
{
    [Area("Admin")]// this will identify which controller this is in 
    public class ProductTypesController : Controller
    {
        private readonly ApplicationDbContext _db;
        public ProductTypesController( ApplicationDbContext db )
        {
            _db = db; // this will retrieve the ApplicationDbContext db using dependency injection and this will populate the private readonly _db.
        }
        // =============================================Index1 ===================================================
        //after poplulating the _db, that private readonly ApplicationDbContext _db is has information stored for all everything in this class can use.
        public IActionResult Index1() // the view that it will deliever the data to
        {
            return View(_db.ProductTypes.ToList());
        }
        // =============================================Create GET ===================================================
        // create GET action method
        public IActionResult Create()
        {
            return View();
        }
        // =============================================Create POST ===================================================
        [HttpPost]
        [AutoValidateAntiforgeryToken] // security from asp.net that every request it will create a token and sent to the request
        public async Task<IActionResult> Create( ProductTypes productTypes ) // the asp-for="name". The name will be delivered to the productTypes params from Create.cshtml
        {
            if (ModelState.IsValid)
            {
                _db.Add(productTypes);
                await _db.SaveChangesAsync();
                // returning to action and in params nameof. We can use just Index1 but just in case there is a capitalization error elsewhere this will help with it
                return RedirectToAction(nameof(Index1));
            }
            //if model is not valid, return the View with the productTypes
            return View(productTypes);
        }

        // =============================================Edit GET ===================================================
        // GET action method for Edit
        // when you're using async and await, you need Task.  You need that ? in front of int
        public async Task<IActionResult> Edit( int? Id )
        {
            //if you can't find the id that's selected, return NotFound()
            if (Id == null)
            {
                return NotFound();
            }
            // Go into database and look for the id
            var productType = await _db.ProductTypes.FindAsync(Id);
            if (productType == null)
            {
                return NotFound();
            }
            // Once the id is found, deliver it to the View.
            return View(productType);
        }

        // =============================================Edit POST ===================================================
        //POST Edit action method
        [HttpPost]
        [AutoValidateAntiforgeryToken] // security from asp.net that every request it will create a token and sent to the request
        //we want to also retrieve the id here into this method so put it in the params. Trying to post back to the database of the changed values
        public async Task<IActionResult> Edit( int Id, ProductTypes productTypes )
        {
            //see if the id is equal to the productTypes
            if (Id != productTypes.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                // we will use Update for now. ASp.net core does everything BUT Update() is only for small data with only 1 name field.
                _db.Update(productTypes);
                await _db.SaveChangesAsync();
                // returning to action and in params nameof. We can use just Index1 but just in case there is a capitalization error elsewhere this will help with it
                return RedirectToAction(nameof(Index1));
            }
            //if model is not valid, return the View with the productTypes
            return View(productTypes);
        }



        // =============================================Details GET ===================================================
        // GET action method for Detail
        // when you're using async and await, you need Task.  You need that ? in front of int.  The "Id" == model="item.Id" in Index1.cshtml
        public async Task<IActionResult> Details( int? Id )
        {
            //if you can't find the id that's selected, return NotFound()
            if (Id == null)
            {
                return NotFound();
            }
            // Go into database and look for the id
            var productType = await _db.ProductTypes.FindAsync(Id);
            if (productType == null)
            {
                return NotFound();
            }
            // Once the id is found, deliver it to the View.
            return View(productType);
        }



        // =============================================Delete GET ===================================================
        // GET action method for Delete
        // when you're using async and await, you need Task.  You need that ? in front of int
        public async Task<IActionResult> Delete( int? Id )
        {
            //if you can't find the id that's selected, return NotFound()
            if (Id == null)
            {
                return NotFound();
            }
            // Go into database and look for the id
            var productType = await _db.ProductTypes.FindAsync(Id);

            if (productType == null)
            {
                return NotFound();
            }
            // Once the id is found, deliver it to the View.
            return View(productType);
        }

        // =============================================Delete POST ===================================================
        //POST Edit action method
        [HttpPost, ActionName("Delete")]// When you place ActionName it will tell the front end that this is Delete action name.  You can change the name of the IActionResult 'Delete' name to anything you want but your comp will know this is Delete.
        [AutoValidateAntiforgeryToken] // security from asp.net that every request it will create a token and sent to the request
        //we want to also retrieve the id here into this method so put it in the params. Trying to post back to the database of the changed values
        public async Task<IActionResult> Delete( int Id )
        {
            var productTypes = await _db.ProductTypes.FindAsync(Id);
            // this will Remove the match id from productTypes database/model
            _db.ProductTypes.Remove(productTypes);
            await _db.SaveChangesAsync();
            // returning to action and in params nameof. We can use just Index1 but just in case there is a capitalization error elsewhere this will help with it
            return RedirectToAction(nameof(Index1));
        }

    }
}