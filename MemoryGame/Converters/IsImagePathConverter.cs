using System;
using System.Globalization;
using System.IO;
using System.Windows;
using System.Windows.Data;

namespace MemoryGame.Converters
{
    public class IsImagePathConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is string path)
            {
                try
                {
                    if (File.Exists(path))
                    {
                        string extension = Path.GetExtension(path).ToLower();
                        return extension == ".jpg" || extension == ".jpeg" || extension == ".png" || extension == ".gif" || extension == ".bmp";
                    }

                    if (path.StartsWith("Value_") || !path.Contains("."))
                    {
                        return false;
                    }

                    string fileName = Path.GetFileName(path);
                    if (fileName.EndsWith(".jpg") || fileName.EndsWith(".jpeg") ||
                        fileName.EndsWith(".png") || fileName.EndsWith(".gif") ||
                        fileName.EndsWith(".bmp"))
                    {
                        return true;
                    }
                }
                catch
                {
                    return false;
                }
            }
            return false;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }

    }
}
