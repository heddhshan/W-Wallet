using Nethereum.Hex.HexConvertors.Extensions;
using System;
using System.Globalization;
using System.Windows.Data;
using System.Windows.Media;

namespace BlockChain.Share
{
    public class RRColorConvert : IValueConverter
    {
        public object Convert(object value, Type typeTarget, object param, CultureInfo culture)
        {
            string SV = value.ToString();    //形如： FFAA98
            byte r = SV.Substring(0, 2).HexToByteArray()[0];
            byte g = SV.Substring(2, 2).HexToByteArray()[0];
            byte b = SV.Substring(4, 2).HexToByteArray()[0];

            return new SolidColorBrush(Color.FromRgb(r, g, b));
        }

        public object ConvertBack(object value, Type typeTarget, object param, CultureInfo culture)
        {
            return "";
        }

    }

}
