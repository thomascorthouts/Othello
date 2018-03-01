using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Model.ArtificialIntelligence
{
    public interface IGameTree<MOVE>
    {
        IList<MOVE> ValidMoves { get; }

        IGameTree<MOVE> MakeMove(MOVE move);

        bool IsAITurn { get; }

        double Score { get; }
    }
}
