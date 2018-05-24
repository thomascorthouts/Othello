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

        }

        public Cell<ReversiGame> gameCell { get; set; }


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
