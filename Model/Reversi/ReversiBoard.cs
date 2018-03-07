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
        private const int MINIMUM_WIDTH = 2;
        private const int MAXIMUM_WIDTH = 20;
        private const int MINIMUM_HEIGHT = 2;
        private const int MAXIMUM_HEIGHT = 20;

        private readonly IGrid<Var<Player>> _contents;

        /// <summary>
        /// Checks whether the given width is valid.
        /// </summary>
        /// <param name="width">Width to be checked.</param>
        /// <returns>True if the given width is valid, false otherwise.</returns>
        public static bool IsValidWidth(int width)
        {
            return MINIMUM_WIDTH <= width && width < MAXIMUM_WIDTH && IsEven(width);
        }

        /// <summary>
        /// Checks whether the given height is valid.
        /// </summary>
        /// <param name="height">Height to be validated.</param>
        /// <returns>True if the given height is valid, false otherwise.</returns>
        public static bool IsValidHeight(int height)
        {
            return MINIMUM_HEIGHT <= height && height <= MAXIMUM_HEIGHT && IsEven(height);
        }

        private static bool IsEven(int n)
        {
            return n % 2 == 0;
        }

        /// <summary>
        /// Creates a Reversi board in its initial state. The given width and height must be valid.
        /// </summary>
        /// <param name="width">Width of the board.</param>
        /// <param name="height">Height of the board.</param>
        /// <returns>A board object in the initial state.</returns>
        public static ReversiBoard CreateInitialState(int width, int height)
        {
            if (!IsValidWidth(width))
            {
                throw new ArgumentException($"Width (${width}) should be between ${MINIMUM_WIDTH} and ${MAXIMUM_WIDTH}, and even");
            }
            else if (!IsValidHeight(height))
            {
                throw new ArgumentException($"Height (${height}) should be between ${MINIMUM_HEIGHT} and ${MAXIMUM_HEIGHT}, and even");
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

        internal ReversiBoard(int width, int height, Func<Vector2D, Player> initializer)
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

        /// <summary>
        /// Width of the board.
        /// </summary>
        public int Width => this._contents.Width;

        /// <summary>
        /// Height of the board.
        /// </summary>
        public int Height => this._contents.Height;

        /// <summary>
        /// Stone at the given position, or <code>null</code> if unowned.
        /// </summary>
        /// <param name="position">Position on the board.</param>
        /// <returns>A <code>Player</code> object representing the owner of the stone at the given position, or <code>null</code> if no stone is placed there.</returns>
        public Player this[Vector2D position] => this._contents[position].Value;

        /// <summary>
        /// List of all positions on the board.
        /// </summary>
        public IEnumerable<Vector2D> Positions => this._contents.Positions();

        internal ReversiBoard AddStone(Vector2D position, Player player)
        {
            if (!IsValidMove(position, player))
            {
                throw new InvalidOperationException("Invalid move");
            }
            else
            {
                var copy = MakeCopyOfContents();
                copy[position].Value = player;
                var captured = CapturedBy(position, player).ToList();

                foreach (var p in captured)
                {
                    copy[p].Value = player;
                }

                return new ReversiBoard(copy);
            }
        }

        /// <summary>
        /// Computes a list of board positions that would be captured
        /// if the given player would put a stone on the given position.
        /// </summary>
        /// <param name="position">Position.</param>
        /// <param name="player">Player.</param>
        /// <returns>A list of captured board positions.</returns>
        public IEnumerable<Vector2D> CapturedBy(Vector2D position, Player player)
        {
            if (IsValidMove(position, player))
            {
                foreach (var direction in Vector2D.Directions)
                {
                    var slice = this._contents.Slice(position + direction, direction);
                    var maybeIndex = slice.FindFirstIndex(x => x.Value != player.OtherPlayer);

                    if (maybeIndex.HasValue)
                    {
                        var index = maybeIndex.Value;

                        if (slice[index].Value == player)
                        {
                            for (var i = 1; i <= index; ++i)
                            {
                                yield return position + i * direction;
                            }
                        }
                    }
                }
            }
            else
            {
                throw new InvalidOperationException("Invalid move");
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
                return "B";
            }
            else if (player == Player.WHITE)
            {
                return "W";
            }
            else
            {
                Debug.Assert(player == null);

                return ".";
            }
        }

        /// <summary>
        /// Checks if the given board position is a valid move for the given player.
        /// </summary>
        /// <param name="position">Position.</param>
        /// <param name="player">Player.</param>
        /// <returns>True if the move is valid, false otherwise.</returns>
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

        internal bool HasValidMoves(Player player)
        {
            return this.Positions.Any(p => IsValidMove(p, player));
        }

        /// <summary>
        /// Counts the number of stones of the given player.
        /// </summary>
        /// <param name="player">Player whose stones to count.</param>
        /// <returns>Number of stones owner by the given player.</returns>
        public int CountStones(Player player)
        {
            return this.Positions.Count(p => this[p] == player);
        }
    }
}
