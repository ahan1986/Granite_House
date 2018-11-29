using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Granite_House.Models.ViewModels
{   // ViewModel is when your view require more than one model to generate e.g. doing "joins" in sql.  You can just add it in Models folder but it will be more organized if you have another folder called ViewModels and store it in there.  Also you should name all your viewModel files with ViewModel at the end of the name.
    public class ProductsViewModel
    {
        public Products Products { get; set; }
        public IEnumerable<ProductTypes> ProductTypes { get; set; }
        public IEnumerable<SpecialTag> SpecialTag { get; set; }
    }
}
