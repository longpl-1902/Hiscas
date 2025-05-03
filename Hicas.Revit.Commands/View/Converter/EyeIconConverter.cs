using System;
using System.Globalization;
using System.Windows.Data;

namespace Hicas.WPF.View.Converter
{
    class EyeIconConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            bool isIsolate = value is bool b && b;
            return isIsolate ? @"D:\work\Hicas.WPF\bin\Debug\Resource\Icon\hide.png" : @"D:\work\Hicas.WPF\bin\Debug\Resource\Icon\view.png";
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
