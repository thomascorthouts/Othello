using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Model.Reversi
{
    public abstract class Player
    {
        public static readonly Player BLACK = new Black();

        public static readonly Player WHITE = new White();

        public abstract Player OtherPlayer { get; }

        private class Black : Player
        {
            public override Player OtherPlayer => WHITE;

            public override string ToString()
            {
                return "B";
            }
        }

        private class White : Player
        {
            public override Player OtherPlayer => BLACK;

            public override string ToString()
            {
                return "W";
            }
        }
    }
}
