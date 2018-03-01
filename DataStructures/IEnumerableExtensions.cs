using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataStructures
{
    public static class IEnumerableExtensions
    {
        public static T MaximumByKey<T>(this IEnumerable<T> xs, Func<T, double> key, T whenEmpty = default(T))
        {
            var best = whenEmpty;
            var bestScore = double.NegativeInfinity;

            foreach ( var x in xs )
            {
                var score = key(x);

                if ( score > bestScore )
                {
                    best = x;
                    bestScore = score;
                }
            }

            return best;
        }

        public static T MinimumByKey<T>(this IEnumerable<T> xs, Func<T, double> key, T whenEmpty = default(T))
        {
            return xs.MaximumByKey(x => -key(x), whenEmpty);
        }
    }
}
