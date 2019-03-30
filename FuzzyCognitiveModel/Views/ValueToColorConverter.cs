using System;
using System.Data;
using System.Globalization;
using System.Windows.Data;
using System.Windows.Media;

namespace FuzzyCognitiveModel.Views
{
    public class ValueToColorConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if ((double) value > 0)
            {
                return new SolidColorBrush(Colors.LightGreen);
            }
            else if((double)value < 0)
            {
                return new SolidColorBrush(Colors.LightBlue);
            }

            return Binding.DoNothing;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}