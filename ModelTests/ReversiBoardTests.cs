using System;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Model.Reversi;
using DataStructures;

namespace ReversiModelTests
{
    [TestClass]
    public class ReversiBoardTests
    {
        private IGrid<char> ParseGrid(string str)
        {
            var lines = str.Trim().Split('\n').Select(line => line.Trim()).ToList();

            var lineLengths = lines.Select(line => line.Length);
            if ( lineLengths.Distinct().Count() != 1 )
            {
                throw new ArgumentException("Lines don't have equal lengths");
            }

            var width = lines[0].Length;
            var height = lines.Count;
            
            return new Grid<char>(width, height, p => lines[p.Y][p.X]);
        }

        private ReversiBoard ParseReversiBoard(IGrid<char> grid)
        {
            return new ReversiBoard(grid.Width, grid.Height, p =>
            {
                char c = grid[p];

                switch (c)
                {
                    case '.':
                        return null;

                    case 'w':
                        return Player.WHITE;

                    case 'b':
                        return Player.BLACK;

                    default:
                        throw new ArgumentException($"Invalid character {c}");
                }
            });
        }

        private ReversiBoard ParseReversiBoard(string str)
        {
            var grid = ParseGrid(str);

            return ParseReversiBoard(grid);
        }

        private Tuple<Vector2D, Player> FindMove(IGrid<char> grid)
        {
            var position = grid.FindPosition(c => Char.IsUpper(c));

            if (position == null)
            {
                throw new Exception("Test bug: could not find move position");
            }
            else
            {
                switch (grid[position])
                {
                    case 'W':
                        return Tuple.Create(position, Player.WHITE);

                    case 'B':
                        return Tuple.Create(position, Player.BLACK);

                    default:
                        throw new Exception("Test bug: invalid character in string");
                }
            }
        }

        private void CheckPutStone(string initialString, string expectedString)
        {
            var initialGrid = ParseGrid(initialString);
            var initialBoard = ParseReversiBoard(initialGrid.Map(c => Char.IsUpper(c) ? '.' : c));
            var tuple = FindMove(initialGrid);
            var position = tuple.Item1;
            var player = tuple.Item2;
            var actualBoard = initialBoard.AddStone(position, player);
            var expectedBoard = ParseReversiBoard(expectedString);

            var temp = initialBoard.ToString();

            foreach (var p in actualBoard.Positions)
            {
                Assert.AreSame(expectedBoard[p], actualBoard[p], $"At position {p}, expected {expectedBoard[p]}, got {actualBoard[p]}");
            }
        }

        private void CheckIsValidMove(string str, Player player)
        {
            var grid = ParseGrid(str);
            var board = ParseReversiBoard(grid.Map(c => c == '*' ? '.' : c));
            var validPositions = grid.Positions().Where(p => grid[p] == '*');
            var invalidPositions = grid.Positions().Where(p => grid[p] != '*');

            foreach ( var validPosition in validPositions )
            {
                Assert.IsTrue(board.IsValidMove(validPosition, player));
            }

            foreach (var invalidPosition in invalidPositions)
            {
                Assert.IsFalse(board.IsValidMove(invalidPosition, player));
            }
        }

        [TestMethod]
        public void PutStone1()
        {
            var initial = @"
            .Wbw.
            ";

            var expected = @"
            .www.
            ";

            CheckPutStone(initial, expected);
        }

        [TestMethod]
        public void PutStone2()
        {
            var initial = @"
            .Bwb
            ";

            var expected = @"
            .bbb
            ";

            CheckPutStone(initial, expected);
        }

        [TestMethod]
        public void PutStone3()
        {
            var initial = @"
            .bW
            .bb
            .bw
            ";

            var expected = @"
            .bw
            .bw
            .bw
            ";

            CheckPutStone(initial, expected);
        }

        [TestMethod]
        public void PutStoneCapturingEastToWest()
        {
            var initial = @"
            .bwwB..
            ";

            var expected = @"
            .bbbb..
            ";

            CheckPutStone(initial, expected);
        }

        [TestMethod]
        public void PutStoneCapturingWestToEast()
        {
            var initial = @"
            .Wbbbw..
            ";

            var expected = @"
            .wwwww..
            ";

            CheckPutStone(initial, expected);
        }

        [TestMethod]
        public void PutStoneCapturingNorthToSouth()
        {
            var initial = @"
            w
            b
            w
            B
            .
            ";

            var expected = @"
            w
            b
            b
            b
            .
            ";

            CheckPutStone(initial, expected);
        }

        [TestMethod]
        public void PutStoneCapturingSouthToNorth()
        {
            var initial = @"
            wwW
            ..b
            bbb
            ..b
            ..w
            ";

            var expected = @"
            www
            ..w
            bbw
            ..w
            ..w
            ";

            CheckPutStone(initial, expected);
        }

        [TestMethod]
        public void PutStoneCapturingNW2SE()
        {
            var initial = @"
            .b..
            ..w.
            wwwB
            ....
            ";

            var expected = @"
            .b..
            ..b.
            wwwb
            ....
            ";

            CheckPutStone(initial, expected);
        }

        [TestMethod]
        public void PutStoneCapturingInAllDirections()
        {
            var initial = @"
            .w.......
            ..b...w..
            ...b.b...
            .wbbWbw..
            ...bbb...
            ..b.b.w..
            .w..b....
            ....b....
            ....b..b.
            ....w...b
            ";

            var expected = @"
            .w.......
            ..w...w..
            ...w.w...
            .wwwwww..
            ...www...
            ..w.w.w..
            .w..w....
            ....w....
            ....w..b.
            ....w...b
            ";

            CheckPutStone(initial, expected);
        }

        [TestMethod]
        public void IsValidMove1()
        {
            var grid = @"
            *wb
            ";

            CheckIsValidMove(grid, Player.BLACK);
        }

        [TestMethod]
        public void IsValidMove2()
        {
            var grid = @"
            *bw
            ";

            CheckIsValidMove(grid, Player.WHITE);
        }

        [TestMethod]
        public void IsValidMove3()
        {
            var grid = @"
            .....
            *b...
            .w...
            *wb..
            .*...
            ";

            CheckIsValidMove(grid, Player.BLACK);
        }

        [TestMethod]
        public void IsValidMove4()
        {
            var grid = @"
            .wb
            ";

            CheckIsValidMove(grid, Player.WHITE);
        }

        [TestMethod]
        public void InitialState2x2()
        {
            var board = ReversiBoard.CreateInitialState(2, 2);

            Assert.AreSame(Player.WHITE, board[new Vector2D(0, 0)]);
            Assert.AreSame(Player.WHITE, board[new Vector2D(1, 1)]);
            Assert.AreSame(Player.BLACK, board[new Vector2D(0, 1)]);
            Assert.AreSame(Player.BLACK, board[new Vector2D(1, 0)]);
        }

        [TestMethod]
        public void InitialState4x4()
        {
            var board = ReversiBoard.CreateInitialState(4, 4);

            Assert.AreSame(Player.WHITE, board[new Vector2D(1, 1)]);
            Assert.AreSame(Player.WHITE, board[new Vector2D(2, 2)]);
            Assert.AreSame(Player.BLACK, board[new Vector2D(1, 2)]);
            Assert.AreSame(Player.BLACK, board[new Vector2D(2, 1)]);

            Assert.IsNull(board[new Vector2D(0, 0)]);
            Assert.IsNull(board[new Vector2D(1, 0)]);
            Assert.IsNull(board[new Vector2D(2, 0)]);
            Assert.IsNull(board[new Vector2D(3, 0)]);

            Assert.IsNull(board[new Vector2D(0, 3)]);
            Assert.IsNull(board[new Vector2D(1, 3)]);
            Assert.IsNull(board[new Vector2D(2, 3)]);
            Assert.IsNull(board[new Vector2D(3, 3)]);

            Assert.IsNull(board[new Vector2D(0, 1)]);
            Assert.IsNull(board[new Vector2D(0, 2)]);

            Assert.IsNull(board[new Vector2D(3, 1)]);
            Assert.IsNull(board[new Vector2D(3, 2)]);
        }
    }
}
