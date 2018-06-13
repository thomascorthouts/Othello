using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace View
{
    /// <summary>
    /// Interaction logic for ScoreCard.xaml
    /// </summary>
    public partial class ScoreCard : UserControl
	{
        public static readonly DependencyProperty PlayerNameProperty = DependencyProperty.RegisterAttached("PlayerName",
            typeof(string), typeof(ScoreCard));
        public static readonly DependencyProperty PlayerBrushProperty = DependencyProperty.RegisterAttached("PlayerBrush",
            typeof(Brush), typeof(ScoreCard));
        public static readonly DependencyProperty ScorePlayerProperty = DependencyProperty.RegisterAttached("ScorePlayer",
            typeof(int), typeof(ScoreCard));
        public static readonly DependencyProperty CardBackgroundProperty = DependencyProperty.RegisterAttached("CardBackground",
            typeof(Brush), typeof(ScoreCard));

        public string PlayerName {
            get { return GetValue(PlayerNameProperty) as string; }
            set { SetValue(PlayerNameProperty, value); }
        }
        public Brush PlayerBrush
        {
            get { return GetValue(PlayerBrushProperty) as Brush; }
            set { SetValue(PlayerBrushProperty, value); }
        }
        public int ScorePlayer
        {
            get { return (int) GetValue(ScorePlayerProperty); }
            set { SetValue(ScorePlayerProperty, value); }
        }
        public Brush CardBackground
        {
            get { return GetValue(CardBackgroundProperty) as Brush; }
            set { SetValue(CardBackgroundProperty, value); }
        }

        public ScoreCard ()
		{
			InitializeComponent();
		}
	}
}