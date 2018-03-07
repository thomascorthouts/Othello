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
        /// <summary>
        /// Creates a new game with the given board dimensions.
        /// </summary>
        /// <param name="boardWidth">Board width.</param>
        /// <param name="boardHeight">Board height.</param>
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

        /// <summary>
        /// Board.
        /// </summary>
        public ReversiBoard Board { get; }

        /// <summary>
        /// Player whose turn it is.
        /// </summary>
        public Player CurrentPlayer { get; }

        /// <summary>
        /// True is the game is over, false otherwise.
        /// </summary>
        public bool IsGameOver => CurrentPlayer == null;

        /// <summary>
        /// Returns <code>true</code> if the current player is allowed to place
        /// a stone at the given position, <code>false</code> otherwise.
        /// </summary>
        /// <param name="position">Position of the move to be validated.</param>
        /// <returns>True if the move is valid, false otherwise.</returns>
        public bool IsValidMove(Vector2D position)
        {
            return !this.IsGameOver && this.Board.IsValidMove(position, CurrentPlayer);
        }

        /// <summary>
        /// Puts a stone at the specified position. BE CAREFUL: this method
        /// does not change the game object in itself, but returns a new one!
        /// </summary>
        /// <param name="position">Position at which to place a stone.</param>
        /// <returns>A new game object representing the new state of the game.</returns>
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
