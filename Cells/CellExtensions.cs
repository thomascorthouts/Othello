using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cells
{
    public static class CellExtensions
    {
        public static Cell<R> Map<T, R>(this Cell<T> cell, Func<T, R> func)
        {
            return Cell.Derived( cell, func );
        }

        public static Cell<bool> Negate(Cell<bool> cell)
        {
            return cell.Map(x => !x);
        }

        public static Cell<bool> And(this Cell<bool> left, Cell<bool> right)
        {
            return Cell.Derived(left, right, (x, y) => x && y);
        }

        public static Cell<bool> Or(this Cell<bool> left, Cell<bool> right)
        {
            return Cell.Derived(left, right, (x, y) => x || y);
        }
    }
}
