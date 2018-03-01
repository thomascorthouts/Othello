using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Model.Reversi
{
    public class ReversiGame
    {
        private readonly ReversiBoard _board;

        public ReversiGame(int boardWidth, int boardHeight)
        {
            this._board = ReversiBoard.CreateInitialState(boardWidth, boardHeight);
        }
    }
}
