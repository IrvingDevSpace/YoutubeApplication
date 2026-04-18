using System.Globalization;
using System.Windows.Data;

namespace YoutubeApplication.Converters
{
    public class BigNumberConverter : IValueConverter
    {
        public object Convert(object? value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value == null || !double.TryParse(value.ToString(), out double number))
                return "0";

            if (number >= 100000000) // 億
            {
                return (number / 100000000.0).ToString("0.#") + "億";
            }
            if (number >= 10000) // 萬
            {
                return (number / 10000.0).ToString("0.#") + "萬";
            }

            return number.ToString("#,##0"); // 小於萬則顯示千分位，例如 9,453
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
