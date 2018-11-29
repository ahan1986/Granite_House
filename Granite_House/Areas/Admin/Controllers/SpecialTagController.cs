using System.Linq;
using Granite_House.Data;
using Granite_House.Models;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace Granite_House.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class SpecialTagController : Controller
    {
        private readonly ApplicationDbContext _db;
        public SpecialTagController( ApplicationDbContext db )
        {
            _db = db;
        }

        public IActionResult Index()
        {
            return View(_db.SpecialTag.ToList());
        }

        // ======================================= Create GET =======================================================
        public IActionResult Create()
        {
            // View() will display the respective View page. In this case, Create() Razor Page
            return View();
        }

        // ======================================= Create POST =======================================================
        [HttpPost]
        [AutoValidateAntiforgeryToken]
        public async Task<IActionResult> Create( SpecialTag specialTag )
        {
            if (!ModelState.IsValid)
            {
                return View(specialTag);
            }

            _db.Add(specialTag);
            await _db.SaveChangesAsync();

            return RedirectToAction(nameof(Index));
        }

        // ======================================= Edit GET =======================================================
        public async Task<IActionResult> Edit( int? Id )
        {
            if (Id == null)
            {
                return NotFound();
            }

            var specialTag = await _db.SpecialTag.FindAsync(Id);
            if (specialTag == null)
            {
                return NotFound();
            }

            return View(specialTag);
        }

        // ======================================= Edit POST =======================================================
        [HttpPost]
        [AutoValidateAntiforgeryToken] // SpecialTag specialTag is the object. it's just a blue print that gets filled in from the Edit Razor page and the information is sent back here to the Post
        public async Task<IActionResult> Edit( int Id, SpecialTag specialTag )
        {
            // specialTag brings in the object of the respective row of the model.
            // compare the Id that was send from the user in the Edit Razor page and then compares the specialtTag.Id that the Edit GET method sent through View(specialTag)
            if (Id != specialTag.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                _db.Update(specialTag);
                await _db.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(specialTag);
        }

        // ======================================= Detail GET =======================================================
        public async Task<IActionResult> Details( int? Id )
        {
            if (Id == null)
            {
                return NotFound();
            }

            var specialTag = await _db.SpecialTag.FindAsync(Id);
            if (specialTag == null)
            {
                return NotFound();
            }

            return View(specialTag);
        }

        // ======================================= Delete GET =======================================================
        public async Task<IActionResult> Delete( int? Id )
        {
            if(Id == null)
            {
                return NotFound();
            }
            var specialTag = await _db.SpecialTag.FindAsync(Id);
            if(specialTag == null)
            {
                return NotFound();
            }


            return View(specialTag);
        }

        // ======================================= Delete POST =======================================================
        [HttpPost("Delete")]
        [AutoValidateAntiforgeryToken]
        public async Task<IActionResult> Delete( int Id )
        {
            var specialTag = await _db.SpecialTag.FindAsync(Id);
            _db.SpecialTag.Remove(specialTag);
            await _db.SaveChangesAsync();

            return RedirectToAction(nameof(Index));
        }

    }
}