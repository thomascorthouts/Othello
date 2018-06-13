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
            this.Closing = new ClosingCommand(this);
        }

        public Action ApplicationClose { get; set; }

        internal void startGame(int width, int height, string playername1, string playername2)
        {
            ReversiGame game = new ReversiGame(width, height);
            this.ActiveScreen = new BoardViewModel(this, game, playername1, playername2);
        }

        internal void endGame(String PlayerName1, String PlayerName2, int ScorePlayer1, int ScorePlayer2)
        {
            this.ActiveScreen = new EndGameViewModel(this, PlayerName1, PlayerName2, ScorePlayer1, ScorePlayer2);
        }

        internal void newGame(string playerName1, string PlayerName2)
        {
            this.ActiveScreen = new SetupGameViewModel(this, playerName1, PlayerName2);
        }

        private object activeScreen;

        public event PropertyChangedEventHandler PropertyChanged;

        public object ActiveScreen
        {
            get
            {
                return activeScreen;
            }
            private set
            {
                activeScreen = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(ActiveScreen)));
            }
        }

        public ICommand Closing { get; set; }

        private class ClosingCommand : ICommand
        {
            public ClosingCommand(MainWindowViewModel mainWindowViewModel)
            {
                _vm = mainWindowViewModel;
            }

            public event EventHandler CanExecuteChanged;

            public bool CanExecute(object parameter)
            {
                return true;
            }

            public void Execute(object parameter)
            {
                _vm.ApplicationClose?.Invoke();
            }

            private MainWindowViewModel _vm;
        }
    }

}

