using System;
using System.Net;
using System.Text.RegularExpressions;
using System.Windows.Data;

namespace Kudu.Converters
{
    public class HtmlSanitizer : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            return HtmlSanitizer.Convert(value as string);
        }

        public static string Convert(string input)
        {
            // Short-circut and return on empty string
            if (string.IsNullOrEmpty(input))
                return "";

            // Remove HTML tags and empty newlines, also replace paragraph
            // and line breaks with a single line.
            var returnString = input.Replace("</p>", "\n\n");
            returnString = returnString.Replace("<br/>", "\n\n");
            returnString = Regex.Replace(returnString, "<[^>]*>", "");

            // Decode HTML entities
            returnString = HttpUtility.HtmlDecode(returnString);

            return returnString;
        }

        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
