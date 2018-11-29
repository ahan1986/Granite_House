using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace Granite_House.Extensions
{   
    // extension methods have a rule and it must be a static class
    public static class IEnumerableExtensions
    {
        // we are trying to convert the IEnumerable of productTypes to a SelectListItem <-- we have to import "using" of Mvc.Rendering
        // Another rule, first params should be of the extended class preceded by "this "
        // 2nd params, int for the selected value
        public static IEnumerable<SelectListItem> ToSelectListItem<T>(this IEnumerable<T> items, int selectedValue)
        {
            return from item in items
                   select new SelectListItem
                   {
                       // we need a method that will retrieve the Name property from this item collection.
                       // create another extension method using GetPropertyValue where we will create in ReflectionExtension.cs
                       Text = item.GetPropertyValue("Name"),
                       Value = item.GetPropertyValue("Id"),
                       Selected = item.GetPropertyValue("Id").Equals(selectedValue.ToString()) //if the user selects something from the dropdown menu, value we got equals the selectedValue from this argument then Selected becomes true;
                   };
        }
    }
}
