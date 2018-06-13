using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace View
{
    public partial class Stone : UserControl
    {

        public static readonly DependencyProperty EllipseBackgroundProperty = DependencyProperty.RegisterAttached("OwnerBrush",
            typeof(Brush), typeof(Stone));

        public Brush OwnerBrush
        {
            get { return GetValue(EllipseBackgroundProperty) as Brush; }
            set { SetValue(EllipseBackgroundProperty, value); }
        }

        public Stone()
        {
            InitializeComponent();
        }
    }
}