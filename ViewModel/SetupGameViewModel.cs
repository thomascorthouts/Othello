using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace ViewModel
{
    public class SetupGameViewModel : INotifyPropertyChanged
    {
        private MainWindowViewModel MainWindowViewModel;
        public SetupGameViewModel(MainWindowViewModel mainWindowViewModel)
        {
            this.MainWindowViewModel = mainWindowViewModel;
            this.StartGame = new StartGameCommand(this);
        }

        protected void startGame()
        {
            this.MainWindowViewModel.startGame(6, 6, Player1, Player2);
        }


        private string _player1 { get; set; }
        public string Player1
        {
            get
            {
                return _player1;
            }
            set
            {
                _player1 = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(Player1)));
            }
        }


        private string _player2 { get; set; }
        public string Player2
        {
            get
            {
                return _player2;
            }
            set
            {
                _player2 = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(Player2)));
            }
        }
        public ICommand StartGame { get; set; }

        public event PropertyChangedEventHandler PropertyChanged;

        private class StartGameCommand : ICommand
        {

            public StartGameCommand(SetupGameViewModel vm)
            {
                _viewmodel = vm;
            }

            public bool CanExecute(object parameter)
            {
                return true;
            }

            public void Execute(object parameter)
            {
                _viewmodel.startGame();
            }

            private SetupGameViewModel _viewmodel;

            public event EventHandler CanExecuteChanged;
        }
    }
}
