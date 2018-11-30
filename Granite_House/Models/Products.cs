using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace Granite_House.Models
{
    public class Products
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public double Price { get; set; }
        public bool Available { get; set; }
        public string Image { get; set; }
        public string ShadeColor { get; set; }

        // attribute for display. Change the name of this to ProductType
        [Display(Name = "Product Type")]
        public int ProductTypeId { get; set; }
        // we will link the foreign key to the product type above. This way we will have the integrity of foreign relation
        [ForeignKey("ProductTypeId")]
        public virtual ProductTypes ProductTypes { get; set; }

        // attribute for display. Change the name of this to SpecialTag
        [Display(Name = "Special tag")] 
        public int SpecialTagsID { get; set; }
        // we will link the foreign key to the product type above. This way we will have the integrity of foreign relation. Adding virtual, the following model will not go to the database.
        [ForeignKey("SpecialTagsID")]
        public virtual SpecialTag SpecialTag { get; set; }
    }
}
