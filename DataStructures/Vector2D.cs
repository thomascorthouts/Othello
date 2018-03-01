using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Diagnostics;

namespace DataStructures
{
    [DebuggerDisplay("({X}, {Y})")]
    public class Vector2D
    {
        public Vector2D(int x, int y)
        {
            this.X = x;
            this.Y = y;
        }

        public int X { get; }

        public int Y { get; }

        public static Vector2D operator +(Vector2D u, Vector2D v)
        {
            var x = u.X + v.X;
            var y = u.Y + v.Y;

            return new Vector2D(x, y);
        }

        public static Vector2D operator *(Vector2D v, int f)
        {
            var x = v.X * f;
            var y = v.Y * f;

            return new Vector2D(x, y);
        }

        public static Vector2D operator *(int f, Vector2D v)
        {
            return v * f;
        }

        public static IEnumerable<Vector2D> Directions
        {
            get
            {
                yield return new Vector2D(1, 0);
                yield return new Vector2D(1, 1);
                yield return new Vector2D(0, 1);
                yield return new Vector2D(-1, 1);
                yield return new Vector2D(-1, 0);
                yield return new Vector2D(-1, -1);
                yield return new Vector2D(0, -1);
                yield return new Vector2D(1, -1);
            }
        }

        public bool IsDirection
        {
            get
            {
                return -1 <= this.X && this.X <= 1 && -1 <= this.Y && this.Y <= 1 && (this.X != 0 || this.Y != 0);
            }
        }

        public override string ToString()
        {
            return $"({this.X}, {this.Y})";
        }

        public override int GetHashCode()
        {
            return this.X.GetHashCode() ^ this.Y.GetHashCode();
        }

        public override bool Equals(object obj)
        {
            return Equals(obj as Vector2D);
        }

        public bool Equals(Vector2D v)
        {
            return v != null && this.X == v.X && this.Y == v.Y;
        }
    }
}
