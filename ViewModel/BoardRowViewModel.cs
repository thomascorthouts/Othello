using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Cell;
using Model.Reversi;

namespace ViewModel
{
    public class BoardRowViewModel
    {
        private List<BoardSquareViewModel> squares;
        private int indexVertical;

        public BoardRowViewModel(Cell<ReversiGame> reversiGameCell, int indexVertical)
        {
            this.indexVertical = indexVertical;
            squares = new List<BoardSquareViewModel>();
            foreach (var i in Enumerable.Range(0, reversiGameCell.Value.Board.Width))
            {
                squares.Add(new BoardSquareViewModel(reversiGameCell, indexVertical, i));
            }
        }

        public List<BoardSquareViewModel> Squares {
            get
            {
                return squares;
            }
            set
            {
                
            }
        }

    }
}
