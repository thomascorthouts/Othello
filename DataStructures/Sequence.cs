using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataStructures
{
    public interface ISequence<T> : IEnumerable<T>
    {
        int Length { get; }

        T this[int index] { get; }
    }

    public static class ISequenceExtensions
    {
        public static bool IsValidIndex<T>(this ISequence<T> sequence, int index)
        {
            Debug.Assert(sequence != null);

            return 0 <= index && index < sequence.Length;
        }

        public static IEnumerable<int> Indices<T>(this ISequence<T> sequence)
        {
            return Enumerable.Range(0, sequence.Length);
        }

        public static int? FindFirstIndex<T>(this ISequence<T> sequence, Func<T, bool> predicate)
        {
            for ( var i = 0; i != sequence.Length; ++i )
            {
                var item = sequence[i];

                if ( predicate(item) )
                {
                    return i;
                }
            }

            return null;
        }

        public static ISequence<T> Slice<T>(this ISequence<T> sequence, int start, int count, int direction = 1)
        {
            Debug.Assert(sequence != null);
            Debug.Assert(sequence.IsValidIndex(start));
            Debug.Assert(direction == -1 || direction == 1);
            Debug.Assert(count >= 0);
            Debug.Assert(count == 0 || sequence.IsValidIndex(start + direction * (count - 1)));

            return new VirtualSequence<T>(count, i => sequence[start + i * direction]);
        }
    }

    public abstract class SequenceBase<T> : ISequence<T>
    {
        public abstract int Length { get; }

        public abstract T this[int index] { get; }

        public virtual IEnumerator<T> GetEnumerator()
        {
            return this.Indices().Select(i => this[i]).GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return this.GetEnumerator();
        }
    }

    public class Sequence<T> : SequenceBase<T>
    {
        private readonly T[] _items;

        public Sequence(int length, Func<int, T> initializer)
        {
            this._items = Enumerable.Range(0, length).Select(initializer).ToArray();
        }

        public override int Length => this._items.Length;

        public override T this[int index] => this._items[index];

        public override IEnumerator<T> GetEnumerator()
        {
            return ((IEnumerable<T>)_items).GetEnumerator();
        }
    }

    public class VirtualSequence<T> : SequenceBase<T>
    {
        private readonly int _length;

        private readonly Func<int, T> _function;

        public VirtualSequence(int length, Func<int, T> function)
        {
            Debug.Assert(length >= 0);
            Debug.Assert(function != null);

            this._length = length;
            this._function = function;
        }

        public override int Length => this._length;

        public override T this[int index]
        {
            get
            {
                Debug.Assert(this.IsValidIndex(index));

                return _function(index);
            }
        }        
    }
}
