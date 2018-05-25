using Model.Reversi;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using ViewModel;

namespace View
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        ReversiGame reversiGame;
        public App()
        {
            int size = 6;
            reversiGame = new ReversiGame(size, size);

            MainWindow main = new MainWindow();
            BoardViewModel boardViewModel = new BoardViewModel(reversiGame);
            main.DataContext = boardViewModel;
            main.Show();
        }


    }
}
