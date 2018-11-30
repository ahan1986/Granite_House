using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Granite_House.Data;
using Granite_House.Models;
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

        //creating a new product =========================================================
        //GET : Products Create
        public IActionResult Create()
        {
            //1.
            //2. 
            return View(ProductsVM);
        }
        /* 1. goal is to display a dropdown with all of producttypes and specialtags
           2. if you see where we defined ProductsVM above, the producttypes and specialtag are getting data from our db and making it into a list. They are in IEnumerable(or list of special tag) SO inorder to make a dropdown for them we have to convert them to an IEnumerable of select list item. To do this we need an extension. We would need to extend the IEnumerable so we can add a function to do what we want. Extensions allow you to add to existing types. */


        //POST: Products Create =========================================================
        [HttpPost, ActionName("Create")]
        [ValidateAntiForgeryToken] //in post we need this token
        public async Task<IActionResult> CreatePOST() // usually we have ProductsViewModel productsVM inside the params but bc we did [BindProperty] at the top, we don't need any params
        {
            if (!ModelState.IsValid)
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
            if (files.Count != 0)
            {
                //Images has been uploaded. Retrieve the path of the uploaded images. Combine the paths of webRootPath to get to the folder and then SD which is static details. We can get exact path to the ProductImage folder in wwwroot.
                var uploads = Path.Combine(webRootPath, SD.ImageFolder);
                // once found, find the extension of the file. we say files[0] bc it can be more than one files
                var extension = Path.GetExtension(files[0].FileName);

                // filestream object to copy the file from the uploaded to the server. destination path. 2nd param is the rename of the file and extension. Then filemode.create will create the file in the server
                using (var filestream = new FileStream(Path.Combine(uploads, ProductsVM.Products.Id + extension), FileMode.Create))
                {
                    // this will copy the files to the server and rename it 
                    files[0].CopyTo(filestream);
                }
                // we will have the exact spot where the images are saved on the server with the file name and extension 
                productsFromDb.Image = @"\" + SD.ImageFolder + @"\" + ProductsVM.Products.Id + extension;

            }
            else
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

        //GET : Edit =========================================================
        // grabbing the id of the product which user wants to edit.  with that id we have to retrieve the details from the database and show it to the View.
        public async Task<IActionResult> Edit( int? id )
        {
            if (id == null)
            {
                return NotFound();
            }
            //populate the products. We use include so that we can include the specialtag and producttypes. Once we retrieve that we need to grab the first one using SingleOrDefaultAsync.
            ProductsVM.Products = await _db.Products.Include(m => m.SpecialTag).Include(m => m.ProductTypes).SingleOrDefaultAsync(m => m.Id == id);

            // check if it's been retrieved or not
            if (ProductsVM.Products == null)
            {
                return NotFound();
            }

            return View(ProductsVM);
        }

        //POST : Edit =========================================================
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit( int Id )
        {
            if (ModelState.IsValid)
            {
                string webRootPath = _hostingEnvironment.WebRootPath;// webRootPath => to change the exisiting image

                var files = HttpContext.Request.Form.Files; // get all the files that were uploaded
                //1.
                var productFromDb = _db.Products.Where(m => m.Id == ProductsVM.Products.Id).FirstOrDefault();

                if (files.Count > 0 && files[0] != null)
                {
                    //2.
                    var uploads = Path.Combine(webRootPath, SD.ImageFolder);
                    var extension_new = Path.GetExtension(files[0].FileName);
                    var extension_old = Path.GetExtension(productFromDb.Image);
                    //3.
                    if (System.IO.File.Exists(Path.Combine(uploads, ProductsVM.Products.Id + extension_old)))
                    {
                        System.IO.File.Delete(Path.Combine(uploads, ProductsVM.Products.Id + extension_old));
                    }
                    //4.
                    using (var filestream = new FileStream(Path.Combine(uploads, ProductsVM.Products.Id + extension_new), FileMode.Create))
                    {
                        files[0].CopyTo(filestream); // this will copy the files to the server and rename it 
                    }
                    // we will have the exact spot where the images are saved on the server with the file name and extension 
                    ProductsVM.Products.Image = @"\" + SD.ImageFolder + @"\" + ProductsVM.Products.Id + extension_new;
                }
                //5.
                if (ProductsVM.Products.Image != null)
                {
                    productFromDb.Image = ProductsVM.Products.Image;
                }
                //6.
                productFromDb.Name = ProductsVM.Products.Name;
                productFromDb.Price = ProductsVM.Products.Price;
                productFromDb.Available = ProductsVM.Products.Available;
                productFromDb.ProductTypeId = ProductsVM.Products.ProductTypeId;
                productFromDb.SpecialTagsID = ProductsVM.Products.SpecialTagsID;
                productFromDb.ShadeColor = ProductsVM.Products.ShadeColor;

                await _db.SaveChangesAsync();

                return RedirectToAction(nameof(Index));
            }
            //if it's not valid
            return View(ProductsVM);
        }
        //1. when we replace an image, we have to delete the existing image and then update the new one. find out the product from the database.
        //2. if user uploads a new image.  We are grabbing the new one and the old files so that the new one can replace the old one.
        //3. here we are checking to see if the old one does Exists and the inside the if block we will Delete that file.
        //4. then we need to append the file into the server.  grab the filestream and add the image.
        //5. then update the productFromDb and save everything to the database. if the ProductsVM.Products.Image is NOT null, it means that we have gone through the process and have appended the image. Then we save the image to the productFromDb.Image.
        //6. Now update all the other property in the database. And save all the changes.


        //GET : Details =========================================================
        public async Task<IActionResult> Details( int? id )
        {
            if (id == null)
            {
                return NotFound();
            }
            //populate the products. We use include so that we can include the specialtag and producttypes. Once we retrieve that we need to grab the first one using SingleOrDefaultAsync.
            ProductsVM.Products = await _db.Products.Include(m => m.SpecialTag).Include(m => m.ProductTypes).SingleOrDefaultAsync(m => m.Id == id);

            // check if it's been retrieved or not
            if (ProductsVM.Products == null)
            {
                return NotFound();
            }

            return View(ProductsVM);
        }

        //GET : Delete =========================================================
        public async Task<IActionResult> Delete( int? id )
        {
            if (id == null)
            {
                return NotFound();
            }
            
            ProductsVM.Products = await _db.Products.Include(m => m.SpecialTag).Include(m => m.ProductTypes).SingleOrDefaultAsync(m => m.Id == id);

            if (ProductsVM.Products == null)
            {
                return NotFound();
            }

            return View(ProductsVM);
        }

        //POST : Delete =========================================================
        //6.
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed( int Id )
        {
            //1.
            string webRootPath = _hostingEnvironment.WebRootPath;
            //2.
            Products products = await _db.Products.FindAsync(Id);

            if (products == null)
            {
                return NotFound();
            }
            else
            {

                var uploads = Path.Combine(webRootPath, SD.ImageFolder); // this will give you the path name for this image EXCEPT the extension. The next step is to get the extension.
                //3.
                var extension = Path.GetExtension(products.Image);
            
                //4.
                if(System.IO.File.Exists(Path.Combine(uploads, products.Id + extension)))
                {
                    System.IO.File.Delete(Path.Combine(uploads, products.Id + extension));
                }
                //5.
                _db.Products.Remove(products);
                await _db.SaveChangesAsync();

                return RedirectToAction(nameof(Index));
            }
        }
        //1. Gets into the wwwroot folder
        //2. finding the product from the database with the specific Id we are looking for. You can use where clause and firstOrDefault
        //3. extension is the end part of the path. e.g. .ext .jpg .png
        //4. find if the image exists in the file that you've created locally then delete the image in the 'images' folder inside wwwroot.
        //5. then remove the image from the database
        //6. Because we named it "DeleteConfirmed" which is not Delete which is the name of the action we have given, We will type ActionName("Delete") to tell visual studio that it's a delete post action method

    }
}
