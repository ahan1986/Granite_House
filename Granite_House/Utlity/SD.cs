using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Granite_House.Utlity
{
    public class SD
    {
        //static details
        //adding 2 properties
        public const string DefaultProductImage = "default_product.jpg";
        public const string ImageFolder = @"image\ProductImage"; // adding the @ will allow you to not add another slash in the beginning of the folder path.... 
        // after creating the ImageFolder property, create an ProductImage folder in the project in the wwwroot
    }
}
