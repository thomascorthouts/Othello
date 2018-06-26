using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace View
{
    public partial class MusicPanel : UserControl
    {
        private MediaPlayer mediaPlayer = new MediaPlayer();
        private string Sound = "Mario";

        public MusicPanel()
        {
            InitializeComponent();
            mediaPlayer.Volume = (double)volumeSlider.Value;
            OpenSong();
        }

        private void OpenSong()
        {
            var uri = $"../../Songs/{Sound}.mp3";
            mediaPlayer.Open(new Uri(uri, UriKind.Relative));
        }
        private void ChooseSong(object sender, RoutedEventArgs eventArgs)
        {
            ComboBox cmb = sender as ComboBox;
            Sound = cmb.SelectedItem.ToString().Split(new string[] { ": " }, StringSplitOptions.None).Last();
            OpenSong();
        }

        private void Play(object sender, RoutedEventArgs eventArgs)
        {
            mediaPlayer.Play();
        }

        private void Pause(object sender, RoutedEventArgs eventArgs)
        {
            mediaPlayer.Pause();
        }

        private void ChangeMediaVolume(object sender, RoutedEventArgs eventArgs)
        {
            mediaPlayer.Volume = (double)volumeSlider.Value;
        }
    }
}