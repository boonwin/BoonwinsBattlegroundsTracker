using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using Hearthstone_Deck_Tracker.API;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace BoonwinsBattlegroundTracker
{
    /// <summary>
    /// Interaktionslogik für View.xaml
    /// </summary>
    public partial class View : UserControl
    {
        BgMatchOverlay _overlay = new BgMatchOverlay();
        public View()
        {
            InitializeComponent();
        }

        internal void SetAvgRank(string avgRank)
        {
            Ranks rank = new Ranks();
            lbAvgRank.Content = "Avg Rank: " + avgRank;
        }

        internal void SetMMR(int rating)
        {
            lbrMMR.Content = "MMR: " + rating;
        }

        internal void SetisBannedGameStart()
        {
            lbbannedMinion.Content = "Waiting...";
            lbbannedMinion2.Content = "";

        }

        internal void SetisBanned(string type1, string type2)
        {
            lbbannedMinion.Content = type1;
            lbbannedMinion2.Content = type2;
        }
    }


    


}
