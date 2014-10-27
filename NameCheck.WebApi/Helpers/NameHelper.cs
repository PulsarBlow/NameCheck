using System;

namespace NameCheck.WebApi
{
    public static class NameHelper
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

        public static string Format(string value)
        {
            if (String.IsNullOrWhiteSpace(value))
            {
                return value;
            }

            return RemoveExtension(value.ToLowerInvariant());
        }
    }
}