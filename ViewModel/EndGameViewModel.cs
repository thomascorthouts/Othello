using System;
using System.ComponentModel;
using System.Windows.Input;

namespace ViewModel
{
    public class EndGameViewModel : INotifyPropertyChanged
    {
        private bool player1Won;
        private string playerName1;
        private string playerName2;
        private int scorePlayer1;
        private int scorePlayer2;
        private MainWindowViewModel mainWindowViewModel;
        

        public EndGameViewModel(MainWindowViewModel mainWindowViewModel, string playerName1, string playerName2, int scorePlayer1, int scorePlayer2)
        {

            this.playerName1 = playerName1;
            this.playerName2 = playerName2;
            this.scorePlayer1 = scorePlayer1;
            this.scorePlayer2 = scorePlayer2;
            this.player1Won = (scorePlayer1 > scorePlayer2);
            this.mainWindowViewModel = mainWindowViewModel;
            this.NewGame = new NewGameCommand(this);
            this.Stop = new ExitCommand(this);
        }

        public string Winner
        {
            get
            {
                return player1Won ? playerName1 : playerName2;
            }
            set
            {
            }
        }

        public string Loser
        {
            get
            {
                return player1Won ? playerName2 : playerName1;
            }
            set
            {
            }
        }

        public int ScoreWinner
        {
            get
            {
                return player1Won ? scorePlayer1 : scorePlayer2; ;
            }
            set
            {
            }
        }

        public int ScoreLoser
        {
            get
            {
                return player1Won ? scorePlayer2 : scorePlayer1; ;
            }
            set
            {
            }
        }

        private void newGame()
        {
            this.mainWindowViewModel.newGame(playerName1, playerName2);
        }

        public ICommand Stop { get; set; }

        public ICommand NewGame { get; set; }

        private class NewGameCommand : ICommand
        {
            public event EventHandler CanExecuteChanged;

            public NewGameCommand(EndGameViewModel endGameViewModel)
            {
                this._endGameViewModel = endGameViewModel;
            }
            public bool CanExecute(object parameter)
            {
                return true;
            }

            public void Execute(object parameter)
            {
                this._endGameViewModel.newGame();
            }
            private EndGameViewModel _endGameViewModel;
        }


        private class ExitCommand : ICommand
        {
            public ExitCommand(EndGameViewModel endGameViewModel)
            {
                _vm = endGameViewModel;
            }

            public event EventHandler CanExecuteChanged;

            public bool CanExecute(object parameter)
            {
                return true;
            }

            public void Execute(object parameter)
            {
                _vm.closeScreen();
            }

            private EndGameViewModel _vm;
        }

        public event PropertyChangedEventHandler PropertyChanged;
        private void closeScreen()
        {
            mainWindowViewModel.ApplicationClose?.Invoke();
        }
    }
}