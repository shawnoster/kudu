using System;
using System.Windows.Data;

namespace Kudu.Converters
{
    public class StringCaseConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            if (value == null)
                return value;

            string textToConvert = value.ToString();
            bool isToUpper = true;
            if (parameter != null) isToUpper = System.Convert.ToBoolean(parameter.ToString());

            return isToUpper ? textToConvert.ToUpper() : textToConvert.ToLower();
        }

        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}


