using SuperMassive;
using System;
using System.Collections.Generic;

namespace NameCheck.WebApi
{
    public static class NameCheckHelper
    {
        public static string RemoveExtension(string value)
        {
            if (String.IsNullOrWhiteSpace(value))
            {
                return value;
            }

            int indexOf = value.IndexOf('.');
            if (indexOf == -1)
            {
                return value;
            }

            return value.Substring(0, indexOf);
        }

        public static string FormatQuery(string value)
        {
            if (String.IsNullOrWhiteSpace(value))
            {
                return value;
            }
            return RemoveExtension(value.ToLowerInvariant()).Replace(" ", "");
        }

        public static string FormatKey(string value)
        {
            if (String.IsNullOrWhiteSpace(value)) { return value; }
            value = StringHelper.RemoveDiacritics(value.Replace(" ", ""));
            return StringHelper.Dasherize(value).Trim('-');
        }

        public static IList<string> ParseBatch(string batch, string separator)
        {
            if (String.IsNullOrWhiteSpace(batch))
            {
                return null;
            }

            return new List<string>(batch.Trim().Split(
                new string[] { separator },
                StringSplitOptions.RemoveEmptyEntries));
        }
    }
}