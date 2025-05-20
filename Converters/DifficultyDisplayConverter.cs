using System.Globalization;
using System.Windows.Data;
using TurboTyper.MVVM.ViewModel;
using TurboTyper.MVVM.ViewModel.Enums;

namespace TurboTyper.Converters;

public class DifficultyDisplayConverter : IMultiValueConverter
{
    public object Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
    {
        if (values[0] is DifficultyOption option && values[1] is GameMode mode)
        {
            return mode == GameMode.Time ? $"{option.Value}s" : option.Value.ToString();
        }

        return Binding.DoNothing;
    }

    public object[] ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture)
    {
        throw new NotImplementedException();
    }
}