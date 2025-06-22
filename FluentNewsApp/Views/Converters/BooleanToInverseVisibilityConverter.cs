using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;

namespace FluentNewsApp.Views.Converters
{
    [ValueConversion(typeof(bool), typeof(Visibility))]
    public class BooleanToInverseVisibilityConverter : IValueConverter
    {
        #region IValueConverter Members

        public object Convert(object value, Type targetType, object parameter,
            System.Globalization.CultureInfo culture)
        {
            if (targetType != typeof(Visibility))
                throw new InvalidOperationException("The target must be a boolean");

            var converter = new BooleanToVisibilityConverter();
            return converter.Convert(!(bool)value, targetType, parameter, culture);
        }

        public object ConvertBack(object value, Type targetType, object parameter,
            System.Globalization.CultureInfo culture)
        {
            throw new NotSupportedException();
        }

        #endregion
    }
}
