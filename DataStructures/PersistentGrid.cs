using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataStructures
{
    public interface IPersistentGrid<T>
    {
        int Width { get; }

        int Height { get; }

        T this[Vector2D position] { get; }

        IPersistentGrid<T> Set(Vector2D position, T value);
    }

    public static class IPersistentGridExtensions
    {
        public static bool IsValidPosition<T>(this IPersistentGrid<T> grid, Vector2D position)
        {
            return 0 <= position.X && position.X < grid.Width && 0 <= position.Y && position.Y < grid.Height;
        }

        public static IPersistentGrid<T> Update<T>(this IPersistentGrid<T> grid, Vector2D position, Func<T, T> updater)
        {
            return grid.Set(position, updater(grid[position]));
        }

        public static IPersistentGrid<U> Map<T, U>(this IPersistentGrid<T> grid, Func<T, U> mapper)
        {
            return new PersistentGrid<U>(grid.Width, grid.Height, p => mapper(grid[p]));
        }

        public static IEnumerable<Vector2D> Positions<T>(this IPersistentGrid<T> grid)
        {
            foreach (var x in Enumerable.Range(0, grid.Width))
            {
                foreach (var y in Enumerable.Range(0, grid.Height))
                {
                    yield return new Vector2D(x, y);
                }
            }
        }

        public static Vector2D FindPosition<T>(this IPersistentGrid<T> grid, Func<T, bool> predicate)
        {
            foreach ( var position in grid.Positions() )
            {
                if ( predicate(grid[position]))
                {
                    return position;
                }
            }

            return null;
        }
    }

    public class PersistentGrid<T> : IPersistentGrid<T>
    {
        private readonly IPersistentArray<IPersistentArray<T>> _items;

        public PersistentGrid(int width, int height, Func<Vector2D, T> initializer)
        {
            Debug.Assert(width >= 0);
            Debug.Assert(height >= 0);
            Debug.Assert(initializer != null);

            this.Width = width;
            this.Height = height;
            this._items = new PersistentArray<IPersistentArray<T>>(width, x =>
            {
                return new PersistentArray<T>(height, y => initializer(new Vector2D(x, y)));
            });
        }

        public PersistentGrid(int width, int height, T initialValue = default(T)) : this(width, height, _ => initialValue)
        {
            // NOP
        }

        private PersistentGrid(int width, int height, IPersistentArray<IPersistentArray<T>> items)
        {
            Debug.Assert(width >= 0);
            Debug.Assert(height >= 0);
            Debug.Assert(items.Length == width);
            Debug.Assert(width == 0 || items.All(col => col.Length == height));

            this._items = items;
            this.Width = width;
            this.Height = height;
        }

        public int Width { get; }

        public int Height { get; }

        public T this[Vector2D position] => this._items[position.X][position.Y];

        public IPersistentGrid<T> Set(Vector2D position, T value)
        {
            Debug.Assert(position != null);
            Debug.Assert(this.IsValidPosition(position));

            var updatedItems = this._items.Update(position.X, column => column.Set(position.Y, value));

            return new PersistentGrid<T>(Width, Height, updatedItems);
        }
    }
}
