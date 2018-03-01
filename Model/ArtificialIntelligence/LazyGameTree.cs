using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Model.ArtificialIntelligence
{
    public class LazyGameTree<MOVE> : IGameTree<MOVE>
    {
        private readonly IGameTree<MOVE> _originalTree;

        private Dictionary<MOVE, IGameTree<MOVE>> _children;

        private double? _score;

        public LazyGameTree(IGameTree<MOVE> gameTree)
        {
            Debug.Assert(gameTree != null);

            this._originalTree = gameTree;
        }

        public IList<MOVE> ValidMoves
        {
            get
            {
                ComputeChildren();

                return new List<MOVE>(this._children.Keys);
            }
        }

        public bool IsAITurn
        {
            get
            {
                return this._originalTree.IsAITurn;
            }
        }

        public double Score
        {
            get
            {
                ComputeScore();

                return this._score.Value;
            }
        }

        public IGameTree<MOVE> MakeMove(MOVE move)
        {
            ComputeChildren();

            return this._children[move];
        }

        private void ComputeScore()
        {
            if ( !this._score.HasValue )
            {
                this._score = this._originalTree.Score;
            }
        }

        private void ComputeChildren()
        {
            if ( this._children != null )
            {
                this._children = new Dictionary<MOVE, IGameTree<MOVE>>();

                foreach ( var move in this._originalTree.ValidMoves )
                {
                    var successor = this._originalTree.MakeMove(move);
                    var lazySuccessor = new LazyGameTree<MOVE>(successor);

                    this._children.Add(move, lazySuccessor);
                }
            }
        }
    }
}
