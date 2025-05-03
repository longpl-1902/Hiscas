using System;
using System.Globalization;
using System.IO;
using System.Reflection;
using System.Security.Policy;
using System.Windows.Data;
using System.Windows.Media.Imaging;

namespace Hicas.WPF.View.Converter
{
    class EyeIconConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            bool isIsolate = value is bool b && b;
            //string path = Path.Combine(Assembly.GetExecutingAssembly().Location, "..", "Resource", "Icon");
            string path = @"D:\work\project\Outcome\Debug\Resource\Icon";
            var imgPath = isIsolate ? Path.Combine(path, "hide.png") : Path.Combine(path, "view.png");
            return new BitmapImage(new Uri(imgPath));
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
