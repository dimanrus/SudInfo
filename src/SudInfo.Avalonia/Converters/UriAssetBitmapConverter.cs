namespace SudInfo.Avalonia.Converters;

public class UriAssetToBitmapConverter : IValueConverter
{
    public object Convert(object value, Type targetType, object parameter, CultureInfo culture) {
        return new Bitmap(AssetLoader.Open(new(value.ToString())));
    }

    public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture) {
        return string.Empty;
    }
}