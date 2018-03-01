using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataStructures
{
    public interface IGrid<out T>
    {
        int Width { get; }

        int Height { get; }

        T this[Vector2D position] { get; }
    }

    public static class IGridExtensions
    {
        public static IEnumerable<Vector2D> Positions<T>(this IGrid<T> grid)
        {
            Debug.Assert(grid != null);

            return from x in Enumerable.Range(0, grid.Width)
                   from y in Enumerable.Range(0, grid.Height)
                   select new Vector2D(x, y);
        }

        public static bool IsValidPosition<T>(this IGrid<T> grid, Vector2D position)
        {
            Debug.Assert(grid != null);
            Debug.Assert(position != null);

            return 0 <= position.X && position.X < grid.Width && 0 <= position.Y && position.Y < grid.Height;
        }

        public static ISequence<T> Slice<T>(this IGrid<T> grid, Vector2D initialPosition, Vector2D direction)
        {
            Debug.Assert(grid != null);
            Debug.Assert(initialPosition != null);
            Debug.Assert(direction != null);
            Debug.Assert(direction.IsDirection);

            int horizontalLimit, verticalLimit;

            if ( direction.X == -1 )
            {
                horizontalLimit = initialPosition.X + 1;
            }
            else if ( direction.X == 0 )
            {
                horizontalLimit = int.MaxValue;
            }
            else
            {
                Debug.Assert(direction.X == 1);

                horizontalLimit = grid.Width - initialPosition.X;
            }

            if (direction.Y == -1)
            {
                verticalLimit = initialPosition.Y + 1;
            }
            else if (direction.Y == 0)
            {
                verticalLimit = int.MaxValue;
            }
            else
            {
                Debug.Assert(direction.Y == 1);

                verticalLimit = grid.Height - initialPosition.Y;
            }

            var length = Math.Min(horizontalLimit, verticalLimit);

            return new VirtualSequence<T>(length, i => grid[initialPosition + i * direction]);
        }

        public static Grid<T> Copy<T>(this IGrid<T> grid, Func<T, T> elementCopier = null)
        {
            if (elementCopier == null)
            {
                elementCopier = x => x;
            }

            Debug.Assert(grid != null);
            Debug.Assert(elementCopier != null);

            return new Grid<T>(grid.Width, grid.Height, p => elementCopier(grid[p]));
        }

        public static bool IsValidRowIndex<T>(this IGrid<T> grid, int rowIndex)
        {
            return 0 <= rowIndex && rowIndex < grid.Height;
        }

        public static bool IsValidColumnIndex<T>(this IGrid<T> grid, int columnIndex)
        {
            return 0 <= columnIndex && columnIndex < grid.Width;
        }

        public static ISequence<T> Row<T>(this IGrid<T> grid, int rowIndex)
        {
            Debug.Assert(grid != null);
            Debug.Assert(grid.IsValidRowIndex(rowIndex));

            return new VirtualSequence<T>(grid.Width, x => grid[new Vector2D(x, rowIndex)]);
        }

        public static ISequence<T> Column<T>(this IGrid<T> grid, int columnIndex)
        {
            Debug.Assert(grid != null);
            Debug.Assert(grid.IsValidColumnIndex(columnIndex));

            return new VirtualSequence<T>(grid.Height, y => grid[new Vector2D(columnIndex, y)]);
        }

        public static ISequence<ISequence<T>> Rows<T>(this IGrid<T> grid)
        {
            Debug.Assert(grid != null);

            return new VirtualSequence<ISequence<T>>(grid.Height, y => grid.Row(y));
        }

        public static ISequence<ISequence<T>> Columns<T>(this IGrid<T> grid)
        {
            Debug.Assert(grid != null);

            return new VirtualSequence<ISequence<T>>(grid.Width, x => grid.Column(x));
        }

        public static string Format<T>(this IGrid<T> grid, Func<T, string> formatter = null, string rowSeparator = "\n", string columnSeparator = " ")
        {
            if ( formatter == null )
            {
                formatter = x => x.ToString();
            }

            Debug.Assert(grid != null);
            Debug.Assert(formatter != null);
            Debug.Assert(columnSeparator != null);
            Debug.Assert(rowSeparator != null);

            return string.Join(rowSeparator, grid.Rows().Select(row => string.Join(columnSeparator, row.Select(x => formatter(x)))));
        }

        public static Vector2D FindPosition<T>(this IGrid<T> grid, Func<T, bool> predicate)
        {
            Debug.Assert(grid != null);
            Debug.Assert(predicate != null);

            return grid.Positions().FirstOrDefault(p => predicate(grid[p]));
        }

        public static IGrid<U> Map<T, U>(this IGrid<T> grid, Func<T, U> function)
        {
            return new Grid<U>(grid.Width, grid.Height, p => function(grid[p]));
        }
    }

    public class Grid<T> : IGrid<T>
    {
        private T[,] _contents;

        public Grid(int width, int height, Func<Vector2D, T> initializer)
        {
            Debug.Assert(width >= 0);
            Debug.Assert(height >= 0);
            Debug.Assert(initializer != null);

            this._contents = new T[width, height];

            for ( var i = 0; i != width; ++i )
            {
                for ( var j = 0; j != height; ++j )
                {
                    var position = new Vector2D(i, j);
                    this._contents[i, j] = initializer(position);
                }
            }
        }

        public int Width => this._contents.GetLength(0);

        public int Height => this._contents.GetLength(1);

        public T this[Vector2D position] => this._contents[position.X, position.Y];        
    }
}
