using System.Globalization;
using System.Windows.Data;
using TurboTyper.MVVM.ViewModel.Enums;

namespace TurboTyper.Converters;

public class CharStateToColorConverter : IValueConverter
{
    public object? Convert(object? value, Type targetType, object? parameter, CultureInfo culture)
    {
        return value switch
        {
            CharacterState.Correct => "Green",
            CharacterState.Incorrect => "Red",
            _ => "Gray"
        };
    }

    public object? ConvertBack(object? value, Type targetType, object? parameter, CultureInfo culture)
    {
        throw new NotImplementedException();
    }
}

