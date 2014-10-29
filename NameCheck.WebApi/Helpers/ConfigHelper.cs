using SerialLabs;
using System;
using System.Collections.Generic;

namespace NameCheck.WebApi
{
    public static class ConfigHelper
    {
        public static IList<string> ParseCommaSeparatedList(string value)
        {
            if (String.IsNullOrWhiteSpace(value))
            {
                return new List<string>();
            }

            List<string> result = new List<string>();
            string[] elements = value.Split(new string[] { "," }, StringSplitOptions.RemoveEmptyEntries);
            if (elements == null || elements.Length == 0)
            {
                return result;
            }
            elements.Each(x =>
            {
                result.Add(x.Trim());
            });
            return result;
        }
    }
}