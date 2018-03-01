using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataStructures
{
    public interface IPersistentArray<T> : IEnumerable<T>
    {
        int Length { get; }

        T this[int index] { get; }

        IPersistentArray<T> Set(int index, T value);
    }

    public static class IPersistentArrayExtensions
    {
        public static bool IsValidIndex<T>(this IPersistentArray<T> array, int index)
        {
            return 0 <= index && index < array.Length;
        }

        public static IPersistentArray<T> Update<T>(this IPersistentArray<T> array, int index, Func<T, T> updater)
        {
            return array.Set(index, updater(array[index]));
        }

        public static IPersistentArray<U> Map<T, U>(this IPersistentArray<T> array, Func<T, U> mapper)
        {
            Debug.Assert(array != null);
            Debug.Assert(mapper != null);

            return new PersistentArray<U>(array.Length, i => mapper(array[i]));
        }

        public static int FindIndex<T>(this IPersistentArray<T> array, Func<T, bool> predicate)
        {
            Debug.Assert(array != null);
            Debug.Assert(predicate != null);

            return Enumerable.Range(0, array.Length).First(i => predicate(array[i]));
        }
    }

    public class PersistentArray<T> : IPersistentArray<T>
    {
        private readonly T[] _items;

        public PersistentArray(int length, T initialValue = default(T)) : this(length, _ => initialValue)
        {
            Debug.Assert(length >= 0);
        }

        public PersistentArray(int length, Func<int, T> initializer)
        {
            Debug.Assert(length >= 0);
            Debug.Assert(initializer != null);

            this._items = new T[length];

            for ( var i = 0; i != length; ++i )
            {
                this._items[i] = initializer(i);
            }
        }

        private PersistentArray(T[] items)
        {
            Debug.Assert(items != null);

            this._items = items;
        }

        public int Length => this._items.Length;

        public T this[int index] => this._items[index];

        public IPersistentArray<T> Set(int index, T value)
        {
            Debug.Assert(this.IsValidIndex(index));

            // Make copy
            var copy = new T[this._items.Length];
            Array.Copy(this._items, copy, this._items.Length);

            // Set item
            copy[index] = value;

            return new PersistentArray<T>(copy);
        }

        public IEnumerator<T> GetEnumerator()
        {
            return ((IEnumerable<T>)this._items).GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return this._items.GetEnumerator();
        }
    }
}
