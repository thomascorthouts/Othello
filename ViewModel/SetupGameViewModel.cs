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
            initScreen(mainWindowViewModel);

            this.StartGame = new StartGameCommand(this);
        }

        public SetupGameViewModel(MainWindowViewModel mainWindowViewModel, string playerName1, string playerName2)
        {
            initScreen(mainWindowViewModel);

            this.Player1 = playerName1;
            this.Player2 = playerName2;

            this.StartGame = new StartGameCommand(this);

        }

        private void initScreen(MainWindowViewModel mainWindowViewModel)
        {
            this.MainWindowViewModel = mainWindowViewModel;

            this.Width = 8;
            this.Height = 8;
        }

        // Implementation interface 
        public event PropertyChangedEventHandler PropertyChanged;

        // Board Sizes
        public static int[] PossibleWidths => new int[] {4, 6, 8, 10, 12, 14, 16, 18};
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

        public static int[] PossibleHeights => new int[] { 4, 6, 8, 10, 12, 14, 16, 18 };
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
                if (_viewmodel.Player1 == "") _viewmodel.Player1 = "Black";
                if (_viewmodel.Player2 == "") _viewmodel.Player2 = "White";
                _viewmodel.startGame();
            }

            private SetupGameViewModel _viewmodel;

            public event EventHandler CanExecuteChanged;
        }
        protected void startGame() => MainWindowViewModel.startGame(Width, Height, Player1, Player2);
    }
}
