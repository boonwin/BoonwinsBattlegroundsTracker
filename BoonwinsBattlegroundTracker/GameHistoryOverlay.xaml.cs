using Hearthstone_Deck_Tracker;
using Hearthstone_Deck_Tracker.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
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
    /// Interaktionslogik für GameHistoryOverlay.xaml
    /// </summary>
    public partial class GameHistoryOverlay : UserControl
    {

        Config _config = new Config();
        List<GameRecord> _recordList = new List<GameRecord>();
    
        public GameHistoryOverlay()
        {
            InitializeComponent();
            _recordList = GameRecord.LoadGameRecordFromFile(_config._gameRecordPath);
            showAvgRanks();

        }

        public void showAvgRanks()
        {
            int ranks = 0;
            int counter = 0;
            int bestHero = 0;
            string heroName;



            foreach (var game in _recordList)
            {
                ranks = ranks + game.Position;
                counter++;
            }
            lbTotalGames.Content = "Total Games: " + counter;
            lbAvgRanks.Content = "Average Rank: " + (ranks / counter).ToString();

            var query = _recordList.GroupBy(x => x.Hero, (y, z) => new { Hero = y, Count = z.Count() })
                .OrderByDescending(o => o.Count);

            lbtHeros.ItemsSource = query;
        }


    }

}
