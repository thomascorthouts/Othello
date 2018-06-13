using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Cells;
using Model.Reversi;

namespace ViewModel
{
    public class BoardViewModel
    {
        private MainWindowViewModel mainWindowViewModel;
        private List<BoardRowViewModel> rows;
        private ReversiGame reversiGame;
        public Player Player1 => Player.BLACK;
        public Player Player2 => Player.WHITE;
        private string playerName1;
        private string playerName2;

        public BoardViewModel(MainWindowViewModel parentViewModel, ReversiGame game, string playername1, string playername2)
        {
            mainWindowViewModel = parentViewModel;
            gameCell = Cell.Create<ReversiGame>(game);
            reversiGame = game;
            rows = new List<BoardRowViewModel>();
            foreach (var i in Enumerable.Range(0, game.Board.Height))
            {
                rows.Add(new BoardRowViewModel(gameCell, i));
            }

            // Setup first player data, always Player.BLACK for the model
            scorePlayer1 = gameCell.Derive(g => g.Board.CountStones(Player.BLACK));
            FirstPlayersTurn = gameCell.Derive(g => g.CurrentPlayer == Player.BLACK);
            PlayerName1 = playername1;

            // Setup second player data, always Player.WHITE for the model
            scorePlayer2 = gameCell.Derive(g => g.Board.CountStones(Player.WHITE));
            SecondPlayersTurn = gameCell.Derive(g => g.CurrentPlayer == Player.WHITE);
            PlayerName2 = playername2;

            // Setup gameOver listener

            _gameOver = gameCell.Derive(g => g.IsGameOver);
            _gameOver.ValueChanged += () => GameOver();


        }

        public Cell<bool> FirstPlayersTurn { get; set; }

        public Cell<bool> SecondPlayersTurn { get; set; }
        public Cell<ReversiGame> gameCell { get; set; }
        
        public Cell<int> scorePlayer1 { get; set; }

        public Cell<int> scorePlayer2 { get; set; }

        public string PlayerName1 {
            get
            {
                return playerName1;
            }
            set
            {
                if (value == "" || value == null)
                {
                    value = "Player1";
                }
                playerName1 = value;
            }
        }

        public string PlayerName2 {
            get
            {
                return playerName2;
            }
            set
            {
                if (value == "" || value == null)
                {
                    value = "Player 2";
                }
                playerName2 = value;
            }
        }

        private Cell<bool> _gameOver { get; set; }

        private void GameOver()
        {
            mainWindowViewModel.endGame(PlayerName1, PlayerName2, scorePlayer1.Value, scorePlayer2.Value);
        }

        public List<BoardRowViewModel> Rows
        {
            get
            {
                return rows;
            }
            private set
            {
            }
        }
    }
}
