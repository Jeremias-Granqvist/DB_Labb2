using DB_Labb2.Model;
using System.Diagnostics;
using System.Globalization;
using System.Windows.Data;

namespace DB_Labb2.Converters;

public class AuthorNameConverter : IValueConverter
{
    public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
    {
        var authors = value as IEnumerable<Author>;
        if (authors == null)
        {
            return string.Empty;
        }
        else 
        {
            var authorNames = string.Join(", ", authors.Select(a => a.FullName));

            Debug.WriteLine(authorNames); // Debugging output
            return authorNames;

        }
        return string.Empty;
    }

    public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
    {
        return null;
    }
}
