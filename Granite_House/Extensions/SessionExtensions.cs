﻿using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Granite_House.Extensions
{
    public static class SessionExtensions
    {
        //this extension was obtain from https://docs.microsoft.com/en-us/aspnet/core/fundamentals/app-state?view=aspnetcore-2.1
        // follow docs to set up.  You will need to go to StartUp.cs to add services and use session
        // this is used to handle complex object types like a list or an object and put that into a session
        public static void Set<T>( this ISession session, string key, T value )
        {   //stores generic object 'T' which can store list or objects of any class
            session.SetString(key, JsonConvert.SerializeObject(value));
        }

        public static T Get<T>( this ISession session, string key )
        {
            var value = session.GetString(key);

            return value == null ? default(T) :
                JsonConvert.DeserializeObject<T>(value);
        }
    }
}