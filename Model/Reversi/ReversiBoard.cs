using Cells;
using DataStructures;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Model.Reversi
{
    [DebuggerDisplay(nameof(ReversiBoard) + "[{Width} x {Height}]")]
    public class ReversiBoard
    {
        private readonly IGrid<Var<Player>> _contents;

        public ReversiBoard(int width, int height, Func<Vector2D, Player> initializer)
        {
            this._contents = new Grid<Var<Player>>(width, height, p => new Var<Player>(initializer(p)));
        }

        private ReversiBoard(IGrid<Var<Player>> contents)
        {
            this._contents = contents;
        }

        public int Width => this._contents.Width;

        public int Height => this._contents.Height;

        public Player this[Vector2D position] => this._contents[position].Value;

        public IEnumerable<Vector2D> Positions => this._contents.Positions();

        public ReversiBoard AddStone(Vector2D position, Player player)
        {
            if ( player == null )
            {
                throw new ArgumentNullException(nameof(player));
            }
            else if ( _contents[position].Value != null )
            {
                throw new InvalidOperationException("Can only add stones on free squares");
            }
            else
            {
                var copy = MakeCopyOfContents();
                copy[position].Value = player;

                foreach ( var direction in Vector2D.Directions )
                {
                    var slice = copy.Slice(position + direction, direction);
                    var maybeIndex = slice.FindFirstIndex(x => x.Value != player.OtherPlayer);
                    
                    if ( maybeIndex.HasValue )
                    {
                        var index = maybeIndex.Value;

                        if ( slice[index].Value == player )
                        {
                            foreach ( var x in slice.Slice(0, index) )
                            {
                                x.Value = player;
                            }
                        }
                    }
                }

                return new ReversiBoard(copy);
            }
        }

        private IGrid<Var<Player>> MakeCopyOfContents()
        {
            return this._contents.Copy(elementCopier: x => new Var<Player>(x.Value));
        }

        public override string ToString()
        {
            return this._contents.Format(formatter: FormatElement, columnSeparator: "");
        }

        private static string FormatElement(Var<Player> x)
        {
            Debug.Assert(x != null);

            var player = x.Value;

            if ( player == Player.ONE )
            {
                return "1";
            }
            else if ( player == Player.TWO )
            {
                return "2";
            }
            else
            {
                Debug.Assert(player == null);

                return ".";
            }
        }

        public bool IsValidMove(Vector2D position, Player player)
        {
            Debug.Assert(position != null);
            Debug.Assert(this._contents.IsValidPosition(position));

            if (this._contents[position].Value != null)
            {
                return false;
            }
            else
            {
                foreach (var direction in Vector2D.Directions)
                {
                    var slice = this._contents.Slice(position + direction, direction);
                    var maybeIndex = slice.FindFirstIndex(x => x.Value != player.OtherPlayer);

                    if ( maybeIndex.HasValue && slice[maybeIndex.Value].Value == player)
                    {
                        return true;
                    }
                }

                return false;
            }
        }        
    }

    public abstract class Player
    {
        public static readonly Player ONE = new PlayerOne();

        public static readonly Player TWO = new PlayerTwo();

        public abstract Player OtherPlayer { get; }

        private class PlayerOne : Player
        {
            public override Player OtherPlayer => TWO;
        }

        private class PlayerTwo : Player
        {
            public override Player OtherPlayer => ONE;
        }
    }
}
