using System;

namespace NameCheck.WebApi
{
    public static class NameHelpers
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
    }
}