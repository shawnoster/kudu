using System;
using System.Collections.Generic;
using System.Globalization;

namespace Librarian.Common
{
    public static class Parse
    {
        private static CultureInfo _enUS = new CultureInfo("en-US");

        public static DateTime? GoodreadsDateTime(string s)
        {
            DateTime parsedDateTime;
            if (DateTime.TryParseExact(s, "ddd MMM dd HH:mm:ss zzz yyyy", _enUS, DateTimeStyles.None, out parsedDateTime))
                return parsedDateTime;
            else
                return null;            
        }

        public static Uri GoodreadsUri(string s)
        {
            return String.IsNullOrWhiteSpace(s) ? null : new Uri(s, UriKind.Absolute);
        }

        /// <summary>
        /// Parses a query string into a dictionary collection
        /// </summary>
        /// <param name="queryString">the query string to parse</param>
        /// <returns>a dictionary collection of querystring items</returns>
        public static Dictionary<string, string> QueryString(string queryString)
        {
            Dictionary<string, string> nameValueCollection = new Dictionary<string, string>();
            string[] items = queryString.Split('&');

            foreach (string item in items)
            {
                if (item.Contains("="))
                {
                    string[] nameValue = item.Split('=');
                    if (nameValue[0].Contains("?"))
                        nameValue[0] = nameValue[0].Replace("?", "");
                    nameValueCollection.Add(nameValue[0], System.Net.HttpUtility.UrlDecode(nameValue[1]));
                }
            }

            return nameValueCollection;
        }
    }
}