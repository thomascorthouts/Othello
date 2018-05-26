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


            this.Width = 8;
            this.Height = 8;

            this.StartGame = new StartGameCommand(this);
        }

        // Implementation interface 
        public event PropertyChangedEventHandler PropertyChanged;

        // Board Sizes
        public static int[] PossibleWidths => new int[] {2, 4, 6, 8, 10, 12, 14, 16, 18, 20};
        private int _width { get; set; }
        public int Width
        {
            get
            {
                return _width;
            }
            set
            {
                _width = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(Width)));
            }
        }

        public static int[] PossibleHeights => new int[] { 2, 4, 6, 8, 10, 12, 14, 16, 18, 20 };
        private int _height { get; set; }
        public int Height
        {
            get
            {
                return _height;
            }
            set
            {
                _height = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(Height)));
            }
        }

        // Players
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

        // Start Game
        public ICommand StartGame { get; set; }
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
        protected void startGame() => this.MainWindowViewModel.startGame(Width, Height, Player1, Player2);
    }
}
