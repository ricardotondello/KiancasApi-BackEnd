

using System.Collections.Generic;
using Newtonsoft.Json;

namespace KiancaAPI.Test
{
    public static class JsonExtensions
    {
        public static T ToObject<T>(this string jsonText)
        {
            return JsonConvert.DeserializeObject<T>(jsonText);
        }

        public static string ToJson<T>(this T obj)
        {
            return JsonConvert.SerializeObject(obj);
        }
        
        public static List<T> ToObjects<T>(this string jsonText)
        {
            return JsonConvert.DeserializeObject<List<T>>(jsonText);
        }
    }
}