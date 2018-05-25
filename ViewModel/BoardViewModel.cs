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
        private List<BoardRowViewModel> rows;
        private ReversiGame reversiGame;

        public BoardViewModel(ReversiGame game)
        {
            gameCell = Cell.Create<ReversiGame>(game);
            reversiGame = game;
            rows = new List<BoardRowViewModel>();
            foreach (var i in Enumerable.Range(0, game.Board.Height))
            {
                rows.Add(new BoardRowViewModel(gameCell, i));
            }

            scorePlayer1 = gameCell.Derive(g => g.Board.CountStones(Player.BLACK));
            scorePlayer2 = gameCell.Derive(g => g.Board.CountStones(Player.WHITE));
            FirstPlayersTurn = gameCell.Derive(g => g.CurrentPlayer == Player.BLACK);
            SecondPlayersTurn = gameCell.Derive(g => g.CurrentPlayer == Player.WHITE);
        }

        public Cell<bool> FirstPlayersTurn { get; set; }

        public Cell<bool> SecondPlayersTurn { get; set; }
        public Cell<ReversiGame> gameCell { get; set; }
        
        public Cell<int> scorePlayer1 { get; set; }

        public Cell<int> scorePlayer2 { get; set; }

        public string PlayerName1 {
            get
            {
                return "Dirk";
            }
            set
            {
            }
        }

        public string PlayerName2 {
            get
            {
                return "Jos";
            }
            set
            {
            }
        }

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
