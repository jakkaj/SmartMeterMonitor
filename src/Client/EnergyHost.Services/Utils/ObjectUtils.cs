using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;

namespace EnergyHost.Services.Utils
{
    public static class ObjectUtils
    {
        public static Dictionary<string, object> ConvertToDictionary(this object obj)
        {
            return obj.GetType()
                .GetProperties(BindingFlags.Instance | BindingFlags.Public)
                .ToDictionary(prop => prop.Name, prop => prop.GetValue(obj, null));
        }

        public static Dictionary<string, string> ConvertToDictionaryOfString(this object obj)
        {
            return obj.GetType()
                .GetProperties(BindingFlags.Instance | BindingFlags.Public)
                .ToDictionary(prop => prop.Name, prop => prop.GetValue(obj, null).ToString());
        }
    }
}
