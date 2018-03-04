using DataStructures;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Model.Reversi
{
    public class ReversiGame
    {
        public ReversiGame(int boardWidth, int boardHeight)
            : this(ReversiBoard.CreateInitialState(boardWidth, boardHeight), Player.BLACK)
        {
            // NOP
        }

        private ReversiGame(ReversiBoard board, Player currentPlayer)
        {
            this.Board = board;
            this.CurrentPlayer = currentPlayer;
        }

        public ReversiBoard Board { get; }

        public Player CurrentPlayer { get; }

        public bool IsGameOver => CurrentPlayer == null;

        public bool IsValidMove(Vector2D position)
        {
            return !this.IsGameOver && this.Board.IsValidMove(position, CurrentPlayer);
        }

        public ReversiGame PutStone(Vector2D position)
        {
            if ( position == null )
            {
                throw new ArgumentNullException(nameof(position));
            }
            else if (IsGameOver)
            {
                throw new InvalidOperationException("Game is over");
            }
            else if ( !Board.IsValidMove(position, CurrentPlayer))
            {
                throw new InvalidOperationException("Invalid move for current player");
            }
            else
            {
                var updatedBoard = Board.AddStone(position, CurrentPlayer);

                if ( updatedBoard.HasValidMoves(CurrentPlayer.OtherPlayer) )
                {
                    return new ReversiGame(updatedBoard, CurrentPlayer.OtherPlayer);
                }
                else if ( updatedBoard.HasValidMoves(CurrentPlayer))
                {
                    return new ReversiGame(updatedBoard, CurrentPlayer);
                }
                else
                {
                    return new ReversiGame(updatedBoard, null);
                }
            }
        }
    }
}
