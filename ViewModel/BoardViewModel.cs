using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Cell;
using Model.Reversi;

namespace ViewModel
{
    public class BoardViewModel
    { 
        private List<BoardRowViewModel> rows;
        private ReversiGame reversiGame;

        public BoardViewModel(ReversiGame game, string playername1, string playername2)
        {
            gameCell = Cell.Cell.Create<ReversiGame>(game);
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
        }

        public Cell<bool> FirstPlayersTurn { get; set; }

        public Cell<bool> SecondPlayersTurn { get; set; }
        public Cell<ReversiGame> gameCell { get; set; }
        
        public Cell<int> scorePlayer1 { get; set; }

        public Cell<int> scorePlayer2 { get; set; }

        public string PlayerName1 { get; set; }

        public string PlayerName2 { get; set; }

        public List<BoardRowViewModel> Rows
        {
            get
            {
                return rows;
            }
            set
            {
            }
        }
    }
}
