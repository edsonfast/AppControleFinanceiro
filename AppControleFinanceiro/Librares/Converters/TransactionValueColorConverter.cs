using AppControleFinanceiro.Models;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AppControleFinanceiro.Librares.Converters
{
    public class TransactionValueColorConverter : IValueConverter {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture) {
            Transaction transaction = (Transaction)value;
            if (transaction == null) { return Colors.Black; }
            if (transaction.Type == TransactionType.Income) {
                return Color.FromArgb("#7B8D1D");
            } else {
                return Color.FromArgb("#CB5353");
            }
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture) {
            throw new NotImplementedException();
        }
    }
}
