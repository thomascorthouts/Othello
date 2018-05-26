using Model.Reversi;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace ViewModel
{
    public class MainWindowViewModel : INotifyPropertyChanged
    {
        public MainWindowViewModel()
        {
            this.ActiveScreen = new SetupGameViewModel(this);
        }

        internal void startGame(int width, int height, string playername1, string playername2)
        {
            ReversiGame game = new ReversiGame(width, height);
            this.ActiveScreen = new BoardViewModel(game, playername1, playername2);
        }

        private object activeScreen;

        public event PropertyChangedEventHandler PropertyChanged;

        public object ActiveScreen
        {
            get
            {
                return activeScreen;
            }
            set
            {
                activeScreen = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(ActiveScreen)));
            }
        }


    }
}
