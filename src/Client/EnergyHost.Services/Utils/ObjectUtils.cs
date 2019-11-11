using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Net.NetworkInformation;
using System.Reflection;
using System.Text;
using System.Web;

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

        public static string ConvertToISO(this DateTime date)
        {
            return date.ToString("yyyy-MM-ddTHH:mm:ssZ", CultureInfo.InvariantCulture);
        }

        //thanks https://stackoverflow.com/questions/6848296/how-do-i-serialize-an-object-into-query-string-format
        public static string GetQueryString(this object obj)
        {
            var properties = from p in obj.GetType().GetProperties()
                             where p.GetValue(obj, null) != null
                             select p.Name + "=" + HttpUtility.UrlEncode(p.GetValue(obj, null).ToString());

            return String.Join("&", properties.ToArray());
        }

    }
}
