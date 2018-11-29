using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Granite_House.Data;
using Granite_House.Models.ViewModels;
using Granite_House.Utlity;
using Microsoft.AspNetCore.Hosting.Internal;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Granite_House.Controllers
{
    //MUST DEFINE THE AREA OR ELSE WE WILL NOT BE ABLE TO DIRECT YOU TO YOUR LOCATIONS
    [Area("Admin")]
    public class ProductsController : Controller
    {
        //private readonly UserManager<IdentityUser> _userManager;

        //public ProductsController( UserManager<IdentityUser> userManager )
        //{
        //    _userManager = userManager;
        //}
        private readonly ApplicationDbContext _db;
        private readonly HostingEnvironment _hostingEnvironment;

        // initialize our ProductViewModel
        [BindProperty] // whenever you post or retrieve anything, it will automatically bind ProductsVM to everything
        public ProductsViewModel ProductsVM { get; set; }

        //constructor for dependency injection. Grabs the data from the "cloud" and then brings it here and then we will save it to a local variable _db and then use it in our application
        public ProductsController( ApplicationDbContext db, HostingEnvironment hostingEnvironment )
        {
            _db = db;
            _hostingEnvironment = hostingEnvironment;
            // initialize our ProductViewModel
            ProductsVM = new ProductsViewModel()
            {
                ProductTypes = _db.ProductTypes.ToList(),
                SpecialTag = _db.SpecialTag.ToList(),
                Products = new Models.Products()
            };
        }

        public async Task<IActionResult> Index()
        {
            // entity framework core will allow include. Which we will grab information that both has ProductTypes and SpecialTag into products. Then return the data into a list.
            var products = _db.Products.Include(m => m.ProductTypes).Include(m => m.SpecialTag);
            return View(await products.ToListAsync());
        }

        //creating a new product
        //GET : Products Create
        public IActionResult Create()
        {
            //goal is to display a dropdown with all of producttypes and specialtags
            //if you see where we defined ProductsVM above, the producttypes and specialtag are getting data from our db and making it into a list. They are in IEnumerable(or list of special tag) SO inorder to make a dropdown for them we have to convert them to an IEnumerable of select list item. To do this we need an extension. We would need to extend the IEnumerable so we can add a function to do what we want. Extensions allow you to add to existing types.
            return View(ProductsVM);
        }

        //POST: Products Create
        [HttpPost, ActionName("Create")]
        [ValidateAntiForgeryToken] //in post we need this token
        public async Task<IActionResult> CreatePOST() // usually we have ProductsViewModel productsVM inside the params but bc we did [BindProperty] at the top, we don't need any params
        {
            if(!ModelState.IsValid)
            {
                return View(ProductsVM);
            }

            _db.Products.Add(ProductsVM.Products);
            await _db.SaveChangesAsync();

            //Image being saved. Need to append the image to the server. Here we need IHostingEnvironment

            //retrieve root path.  This will go into the wwwroot folder in this project.  
            string webRootPath = _hostingEnvironment.WebRootPath;

            // then retrieve all of the files. var files will have all the files that were uploaded from the View
            var files = HttpContext.Request.Form.Files;
            // update the products from the database with the new path
            var productsFromDb = _db.Products.Find(ProductsVM.Products.Id);
            //check to see if the user uploaded images. Use Count to see the amount in the files
            if(files.Count != 0)
            {
                //Images has been uploaded. Retrieve the path of the uploaded images. Combine the paths of webRootPath to get to the folder and then SD which is static details. We can get exact path to the ProductImage folder in wwwroot.
                var uploads = Path.Combine(webRootPath, SD.ImageFolder);
                // once found, find the extension of the file. we say files[0] bc it can be more than one files
                var extension = Path.GetExtension(files[0].FileName);

                // filestream object to copy the file from the uploaded to the server. destination path. 2nd param is the rename of the file and extension. Then filemode.create will create the file in the server
                using(var filestream = new FileStream(Path.Combine(uploads, ProductsVM.Products.Id + extension), FileMode.Create))
                {
                    // this will copy the files to the server and rename it 
                    files[0].CopyTo(filestream);
                }
                // we will have the exact spot where the images are saved on the server with the file name and extension 
                productsFromDb.Image = @"\" + SD.ImageFolder + @"\" + ProductsVM.Products.Id + extension; 
                
            } else
            {
                //when user does not upload image. We will be combining to the default location PLUS default product image bc we have that name defined. So the uploads will have the exact image name. 
                var uploads = Path.Combine(webRootPath, SD.ImageFolder + @"\" + SD.DefaultProductImage);
                //copy the image from the server and rename so that we have the default image as the Product Id
                System.IO.File.Copy(uploads, webRootPath + @"\" + SD.ImageFolder + @"\" + ProductsVM.Products.Id + ".png");
                productsFromDb.Image = @"\" + SD.ImageFolder + @"\" + ProductsVM.Products.Id + ".png";
            }

            await _db.SaveChangesAsync();

            return RedirectToAction(nameof(Index));
        }
    }
}