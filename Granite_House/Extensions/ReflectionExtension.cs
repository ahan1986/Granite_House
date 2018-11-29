using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Granite_House.Extensions
{
    public static class ReflectionExtension
    {
        //creating another extension for IEnumerableExtentions.cs method
        public static string GetPropertyValue<T>(this T item, string propertyName)
        {
            //propertyName will the argument you added in IEnumerableExtensions
            // goes to each item and finds the GetType => Granite_House.Models.ProductsTypes. Which will be what user added into Create.cshtml as the argument.
            // Then get property name => System.String Name AND Int32 Id. 
            // Then it gets the Value which is what the user types in producttypes tab => Granite!!, CoreStatesGroup, etc.
            return item.GetType().GetProperty(propertyName).GetValue(item, null).ToString();
        }
    }
}
