using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DataStructures;

namespace Model.ArtificialIntelligence
{
    public class Minimax<MOVE> : IArtificialIntelligence<MOVE>
    {
        private readonly int _ply;

        public Minimax(int ply)
        {
            Debug.Assert(ply > 0);

            this._ply = ply;
        }

        public MOVE GetBestMove(IGameTree<MOVE> state)
        {
            if ( !state.IsAITurn)
            {
                throw new InvalidOperationException("It is not the AI's turn");
            }
            else if ( state.ValidMoves.Count == 0)
            {
                throw new InvalidOperationException("No valid moves");
            }
            else
            {
                var best = default(MOVE);
                var bestScore = double.NegativeInfinity;

                foreach (var move in state.ValidMoves)
                {
                    var successorState = state.MakeMove(move);
                    var successorStateScore = ScoreState(successorState, this._ply - 1);

                    if ( successorStateScore > bestScore )
                    {
                        best = move;
                        bestScore = successorStateScore;
                    }
                }

                return best;
            }
        }

        private double ScoreState(IGameTree<MOVE> state, int ply)
        {
            if ( state.IsAITurn )
            {
                return ScoreAIState(state, ply);
            }
            else
            {
                return ScoreOpponentState(state, ply);
            }
        }

        private double ScoreAIState(IGameTree<MOVE> state, int ply)
        {
            Debug.Assert(ply >= 0);
            Debug.Assert(state.IsAITurn);

            if (state.ValidMoves.Count == 0)
            {
                return state.Score;
            }
            else
            {
                return state.ValidMoves.Max(move => ScoreState(state.MakeMove(move), ply - 1));
            }
        }

        private double ScoreOpponentState(IGameTree<MOVE> state, int ply)
        {
            Debug.Assert(ply >= 0);
            Debug.Assert(!state.IsAITurn);

            if ( ply == 0 || state.ValidMoves.Count == 0 )
            {
                return state.Score;
            }
            else
            {
                return state.ValidMoves.Min(move => ScoreState(state.MakeMove(move), ply - 1));
            }
        }
    }
}
