using SuperMassive;
using SuperMassive.Helpers;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text.RegularExpressions;

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

        public static string Sanitize(string value)
        {
            if (String.IsNullOrEmpty(value)) { return value; }
            // http://msdn.microsoft.com/en-us/library/20bw873z(v=vs.110).aspx
            return StringHelper.CollapseWhiteSpaces(Regex.Replace(value, @"[^\p{L}0-9 ]", ""));
        }

        public static string FormatName(string value)
        {
            if (String.IsNullOrEmpty(value)) { return value; }
            return Sanitize(value).Trim();
        }

        public static string FormatQuery(string value)
        {
            if (String.IsNullOrEmpty(value))
            {
                return value;
            }
            string result = RemoveExtension(value).ToLower(CultureInfo.CurrentUICulture);
            result = XmlHelper.SanitizeXmlString(Sanitize(result).Replace(" ", "")); // Needed for gandi api request
            return result;
        }

        public static string FormatKey(string value)
        {
            if (String.IsNullOrEmpty(value)) { return value; }
            return
                StringHelper.Dasherize(
                StringHelper.RemoveDiacritics(
                Sanitize(value)
                .Replace(" ", "")))
                .Trim('-');
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