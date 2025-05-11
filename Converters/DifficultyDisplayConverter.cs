using System.Globalization;
using System.Windows.Data;
using TurboTyper.MVVM.ViewModel;

namespace TurboTyper.Converters;

public class DifficultyDisplayConverter : IMultiValueConverter
{
    public object Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
    {
        if (values[0] is int number && values[1] is GameSettingsViewModel.GameMode mode)
        {
            return mode == GameSettingsViewModel.GameMode.Time ? $"{number}s" : number.ToString();
        }
        return Binding.DoNothing;
    }

    public object[] ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture)
    {
        throw new NotImplementedException();
    }
}