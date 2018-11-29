using System;
using System.Collections.Generic;
using System.Text;
using Granite_House.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

// this is where we set our models and this class will transfer and bring in information from the database. This is where it also connects to the Startup.cs. Other controllers will grab the data from this location and to whatever you named the model/object
namespace Granite_House.Data
{
    public class ApplicationDbContext : IdentityDbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }
        // you set the model to put in database like mongodb and to retrieve the data from the controller, you target what you defined it.
        public DbSet<ProductTypes> ProductTypes { get; set; }
        public DbSet<SpecialTag> SpecialTag { get; set; }
        public DbSet<Products> Products { get; set; }
    }
}
