using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Model.ArtificialIntelligence
{
    public class MinimaxWithAlphaBetaPruning<MOVE> : IArtificialIntelligence<MOVE>
    {
        private readonly int _ply;

        public MinimaxWithAlphaBetaPruning(int ply)
        {
            Debug.Assert(ply > 0);

            this._ply = ply;
        }

        public MOVE GetBestMove(IGameTree<MOVE> state)
        {
            if (!state.IsAITurn)
            {
                throw new InvalidOperationException("It is not the AI's turn");
            }
            else if (state.ValidMoves.Count == 0)
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
                    var successorStateScore = ScoreState(successorState, this._ply - 1, bestScore, double.PositiveInfinity);

                    if (successorStateScore > bestScore)
                    {
                        best = move;
                        bestScore = successorStateScore;
                    }
                }

                return best;
            }
        }

        private double ScoreState(IGameTree<MOVE> state, int ply, double minimum, double maximum)
        {
            if (state.IsAITurn)
            {
                return ScoreAIState(state, ply, minimum, maximum);
            }
            else
            {
                return ScoreOpponentState(state, ply, minimum, maximum);
            }
        }

        private double ScoreAIState(IGameTree<MOVE> state, int ply, double minimum, double maximum)
        {
            Debug.Assert(ply >= 0);
            Debug.Assert(state.IsAITurn);

            if (state.ValidMoves.Count == 0)
            {
                return state.Score;
            }
            else
            {
                var best = double.NegativeInfinity;

                foreach ( var validMove in state.ValidMoves )
                {
                    var successorState = state.MakeMove(validMove);
                    var successorStateScore = ScoreState(successorState, ply - 1, best, maximum);

                    if ( successorStateScore > best )
                    {
                        best = successorStateScore;

                        if ( best >= maximum )
                        {
                            return best;
                        }
                    }
                }

                return best;
            }
        }

        private double ScoreOpponentState(IGameTree<MOVE> state, int ply, double minimum, double maximum)
        {
            Debug.Assert(ply >= 0);
            Debug.Assert(!state.IsAITurn);

            if (ply == 0 || state.ValidMoves.Count == 0)
            {
                return state.Score;
            }
            else
            {
                var worst = double.PositiveInfinity;

                foreach ( var validMove in state.ValidMoves )
                {
                    var successorState = state.MakeMove(validMove);
                    var successorStateScore = ScoreState(successorState, ply - 1, minimum, worst);

                    if ( successorStateScore < worst )
                    {
                        worst = successorStateScore;

                        if ( worst <= minimum)
                        {
                            return worst;
                        }
                    }
                }

                return worst;
            }
        }
    }
}
