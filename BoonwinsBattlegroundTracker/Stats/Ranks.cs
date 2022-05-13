using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace BoonwinsBattlegroundTracker
{
    public class Ranks
    {
       
        public int rank1Amount { get; set; }
        public int rank2Amount { get; set; }
        public int rank3Amount { get; set; }
        public int rank4Amount { get; set; }
        public int rank5Amount { get; set; }
        public int rank6Amount { get; set; }
        public int rank7Amount { get; set; }
        public int rank8Amount { get; set; }
        public int? LastRank { get; set; }

        public static void AddManual(int rank, Ranks ranks)
        {

            SetRank(rank, ranks);
        }

        internal static void SetRank(int rank, Ranks _ranks)
        {

            switch (rank)
            {
                case 1:
                    _ranks.rank1Amount = _ranks.rank1Amount + 1;
                    break;
                case 2:
                    _ranks.rank2Amount = _ranks.rank2Amount + 1;
                    break;
                case 3:
                    _ranks.rank3Amount = _ranks.rank3Amount + 1;
                    break;
                case 4:
                    _ranks.rank4Amount = _ranks.rank4Amount + 1;
                    break;
                case 5:
                    _ranks.rank5Amount = _ranks.rank5Amount + 1;
                    break;
                case 6:
                    _ranks.rank6Amount = _ranks.rank6Amount + 1;
                    break;
                case 7:
                    _ranks.rank7Amount = _ranks.rank7Amount + 1;
                    break;
                case 8:
                    _ranks.rank8Amount = _ranks.rank8Amount + 1;

                    break;
                default: break;
            }

        }
        internal static void RemoveRank(int rank, Ranks _ranks)
        {

            switch (rank)
            {
                case 1:
                    _ranks.rank1Amount = _ranks.rank1Amount - 1;
                    break;
                case 2:
                    _ranks.rank2Amount = _ranks.rank2Amount - 1;
                    break;
                case 3:
                    _ranks.rank3Amount = _ranks.rank3Amount - 1;
                    break;
                case 4:
                    _ranks.rank4Amount = _ranks.rank4Amount - 1;
                    break;
                case 5:
                    _ranks.rank5Amount = _ranks.rank5Amount - 1;
                    break;
                case 6:
                    _ranks.rank6Amount = _ranks.rank6Amount - 1;
                    break;
                case 7:
                    _ranks.rank7Amount = _ranks.rank7Amount - 1;
                    break;
                case 8:
                    _ranks.rank8Amount = _ranks.rank8Amount - 1;

                    break;
                default: break;
            }

        }
    }

   

}
