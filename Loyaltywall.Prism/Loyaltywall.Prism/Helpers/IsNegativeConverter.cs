using System;
using System.Collections.Generic;
using System.Text;
using Xamarin.Forms;

namespace Loyaltywall.Prism.Helpers
{
    public class IsNegativeConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            if (value is double numericValue)
            {
                return numericValue < 0;
            }

            return false; // Si el valor no es numérico o no es un número, se considera no negativo.
        }

        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}