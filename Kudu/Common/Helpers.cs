using System;
using System.Collections.Generic;
using System.IO;
using System.IO.IsolatedStorage;
using System.Runtime.Serialization;
using System.Windows;

namespace Kudu.Common
{
    public static class Helpers
    {
        public static void DeleteFile(string fileName)
        {
            using (var store = IsolatedStorageFile.GetUserStoreForApplication())
            {
                if (store.FileExists(fileName))
                    store.DeleteFile(fileName);
            }
        }

        public static DateTime ParseDateTime(string date)
        {
            var month = date.Substring(4, 3).Trim();
            var dayInMonth = date.Substring(8, 2).Trim();
            var time = date.Substring(11, 9).Trim();
            var year = date.Substring(25, 5).Trim();
            var dateTime = string.Format("{0}-{1}-{2} {3}", dayInMonth, month, year, time);
            var ret = DateTime.Parse(dateTime).ToLocalTime();

            return ret;
        }

        public static void ShowMessage(string message)
        {
            Deployment.Current.Dispatcher.BeginInvoke(() => MessageBox.Show(message));
        }

        /// <summary>
        /// Parses a query string into a dictionary collection
        /// </summary>
        /// <param name="queryString">the query string to parse</param>
        /// <returns>a dictionary collection of querystring items</returns>
        public static Dictionary<string, string> ParseQueryString(string queryString)
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