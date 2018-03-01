using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Model.Reversi
{
    public abstract class Player
    {
        public static readonly Player ONE = new PlayerOne();

        public static readonly Player TWO = new PlayerTwo();

        public abstract Player OtherPlayer { get; }

        private class PlayerOne : Player
        {
            public override Player OtherPlayer => TWO;
        }

        private class PlayerTwo : Player
        {
            public override Player OtherPlayer => ONE;
        }
    }
}
