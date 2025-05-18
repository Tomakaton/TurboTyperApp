using System.Globalization;
using System.Windows.Data;
using TurboTyper.MVVM.ViewModel;

namespace TurboTyper.Converters;

public class ObservableCharToStringConverter : IValueConverter
{
    public object Convert(object? value, Type targetType, object parameter, CultureInfo culture)
    {
        if (value is ObservableChar observableChar)
        {
            return observableChar.Letter.ToString();
        }
        return string.Empty;
    }

    public object? ConvertBack(object? value, Type targetType, object? parameter, CultureInfo culture)
    {
        throw new NotImplementedException();
    }
}