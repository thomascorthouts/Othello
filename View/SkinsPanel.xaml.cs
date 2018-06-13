using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;

namespace View
{
    public partial class SkinsPanel : UserControl
    {
        private String BoardColor;

        public SkinsPanel()
        {
            InitializeComponent();
        }

        private void ChooseSkin(object sender, SelectionChangedEventArgs args)
        {
            ComboBox cmb = sender as ComboBox;
            BoardColor = cmb.SelectedItem.ToString().Split(new string[] { ": " }, StringSplitOptions.None).Last();
            SetSkin(BoardColor.ToLower());
        }

        private void SetSkin(string name)
        {
            var resourceDictionary = new ResourceDictionary();
            var uri = $"Skins/{name}.xaml";
            resourceDictionary.MergedDictionaries.Add((ResourceDictionary)Application.LoadComponent(new Uri(uri, UriKind.Relative)));
            Application.Current.Resources = resourceDictionary;
        }
    }
}