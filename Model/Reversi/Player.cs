using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Model.Reversi
{
    /// <summary>
    /// Objects of this class represent players.
    /// There are only two objects: <code>Player.BLACK</code> and <code>Player.WHITE</code>.
    /// </summary>
    public abstract class Player
    {
        /// <summary>
        /// Object representing the black player.
        /// </summary>
        public static readonly Player BLACK = new Black();

        /// <summary>
        /// Object representing the white player.
        /// </summary>
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
