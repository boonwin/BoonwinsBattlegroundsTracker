using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BoonwinsBattlegroundTracker
{
    public class Mmr
    {
        public int mmrChange(int mmrActual, int mmrLastRound)
        {
            return mmrLastRound - mmrActual;
        }

    }
}
