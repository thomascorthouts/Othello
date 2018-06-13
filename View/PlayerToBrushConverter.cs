using Model.Reversi;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;
using System.Windows.Media;

namespace View
{
    class PlayerToBrushConverter : IValueConverter
    {
        public Brush brushPlayer1 { get; set; }
        public Brush brushPlayer2 { get; set; }

        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            Player player = value as Player;

            if (player == null)
            {
                return Brushes.Transparent;
            }
            else if (player == Player.BLACK)
            {
                return (brushPlayer1==null) ? Brushes.Red : brushPlayer1;
            }
            else
            {
                return (brushPlayer2 == null) ? Brushes.Green : brushPlayer2;
            }
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}