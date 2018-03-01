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

        public static ReversiBoard CreateInitialState(int width, int height)
        {
            if (width < 2)
            {
                throw new ArgumentException($"Width (${width}) should be at least 2");
            }
            else if (height < 2)
            {
                throw new ArgumentException($"Height (${height}) should be at least 2");
            }
            else if (width % 2 != 0)
            {
                throw new ArgumentException($"Width (${width}) should be an even number");
            }
            else if (height % 2 != 0)
            {
                throw new ArgumentException($"Height (${height}) should be an even number");
            }
            else
            {
                var x = width / 2 - 1;
                var y = height / 2 - 1;
                var black = new HashSet<Vector2D>() { new Vector2D(x, y + 1), new Vector2D(x + 1, y) };
                var white = new HashSet<Vector2D>() { new Vector2D(x, y), new Vector2D(x + 1, y + 1) };

                return new ReversiBoard(width, height, position =>
                {
                    if (black.Contains(position))
                    {
                        return Player.BLACK;
                    }
                    else if (white.Contains(position))
                    {
                        return Player.WHITE;
                    }
                    else
                    {
                        return null;
                    }
                });
            }
        }

        public ReversiBoard(int width, int height, Func<Vector2D, Player> initializer)
        {
            Debug.Assert(width > 0);
            Debug.Assert(height > 0);
            Debug.Assert(initializer != null);

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
            if (player == null)
            {
                throw new ArgumentNullException(nameof(player));
            }
            else if (_contents[position].Value != null)
            {
                throw new InvalidOperationException("Can only add stones on free squares");
            }
            else
            {
                var copy = MakeCopyOfContents();
                copy[position].Value = player;

                foreach (var direction in Vector2D.Directions)
                {
                    var slice = copy.Slice(position + direction, direction);
                    var maybeIndex = slice.FindFirstIndex(x => x.Value != player.OtherPlayer);

                    if (maybeIndex.HasValue)
                    {
                        var index = maybeIndex.Value;

                        if (slice[index].Value == player)
                        {
                            foreach (var x in slice.Slice(0, index))
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

            if (player == Player.BLACK)
            {
                return "1";
            }
            else if (player == Player.WHITE)
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

                    if (maybeIndex.HasValue && maybeIndex.Value > 0 && slice[maybeIndex.Value].Value == player)
                    {
                        return true;
                    }
                }

                return false;
            }
        }

        public bool HasValidMoves(Player player)
        {
            return this.Positions.Any(p => IsValidMove(p, player));
        }

        public int CountStones(Player player)
        {
            return this.Positions.Count(p => this[p] == player);
        }
    }
}
