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

        // 1.
        [Display(Name = "Product Type")]
        public int ProductTypeId { get; set; }
        // 2.
        [ForeignKey("ProductTypeId")]//5.
        public virtual ProductTypes ProductTypes { get; set; }

        // 3.
        [Display(Name = "Special tag")] 
        public int SpecialTagsID { get; set; }
        // 4.
        [ForeignKey("SpecialTagsID")]//5.
        public virtual SpecialTag SpecialTag { get; set; }
    }
}
//1. attribute for display. Change the name of this to ProductType
//2. we will link the foreign key to the product type above. This way we will have the integrity of foreign relation
//3. attribute for display. Change the name of this to SpecialTag
//4. we will link the foreign key to the product type above. This way we will have the integrity of foreign relation. Adding virtual, the following model will not go to the database.
//5. Without ForeignKey, the database will automatically produce ProductTypes_Id and SpecialTags_Id from using entity Framework.  To set it a name of the property you want in the database, you will have to create the name that you want it to be e.g. ProductTypeId and SpecialTagsID. Then set [ForeignKey("ProductTypeId")] on top of the constructor that you want the name to be set upon.