using System.Collections.Generic;

namespace NameCheck.WebApi
{
    public static class IDictionaryExtensions
    {
        public static void AddOrUpdate<T, U>(this IDictionary<T, U> dictionary, T key, U value)
        {
            if (dictionary.ContainsKey(key))
            {
                dictionary[key] = value;
            }
            else
            {
                dictionary.Add(key, value);
            }
        }
    }
}