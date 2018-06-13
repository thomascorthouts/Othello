using System;
using System.Collections.Generic;
using System.Linq;
using System.Media;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Cells;
using DataStructures;
using Model.Reversi;

namespace ViewModel
{
    public class BoardSquareViewModel
    {

        private Vector2D position;
        private Cell<ReversiGame> gameCell;

        public BoardSquareViewModel(Cell<ReversiGame> reversiGameCell, int indexVertical, int indexHorizontal)
        {
            this.gameCell = reversiGameCell;
            this.position = new Vector2D(indexHorizontal, indexVertical);
            
            this.Player = gameCell.Derive(p => p.Board[position]);
            this.isValidMove = gameCell.Derive(p => p.IsValidMove(position));

            this.SelectSquare = new SelectSquareCommand(this, isValidMove);
        }

        public Cell<Player> Player { get; set;}

        public ICommand SelectSquare
        {
            get; set;
        }


        public Cell<bool> isValidMove { get; set; }
        private class SelectSquareCommand : ICommand
        {

            public SelectSquareCommand(BoardSquareViewModel vm, Cell<bool> isValidMove)
            {
                _viewmodel = vm;
                isValidMove.ValueChanged += () => CanExecuteChanged(this, EventArgs.Empty);
            }

            public bool CanExecute(object parameter)
            {
                //return true;
                return _viewmodel.gameCell.Value.IsValidMove(_viewmodel.position);
            }

            public void Execute(object parameter)
            {
                SystemSounds.Asterisk.Play();
                var newGame = _viewmodel.gameCell.Value.PutStone(_viewmodel.position);
                _viewmodel.gameCell.Value = newGame;
            }

            private BoardSquareViewModel _viewmodel;

            public event EventHandler CanExecuteChanged;
        }
    }
}