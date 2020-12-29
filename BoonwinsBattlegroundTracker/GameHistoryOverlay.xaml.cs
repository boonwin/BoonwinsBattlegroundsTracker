using Hearthstone_Deck_Tracker;
using Hearthstone_Deck_Tracker.Enums;
using System;
using System.Collections.Generic;
using System.ComponentModel;
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
            makeHeroList();
            imgBg.ImageSource = new BitmapImage(new Uri(Config._statsBackgroundPath));
            imgStatsFG.Source = new BitmapImage(new Uri(Config._statsBestHeroForegroundPath));
            imgStatsBG.Source = new BitmapImage(new Uri(Config._statsBestHeroBackgroundPath));
            mostTop3Hero();

        }

        

            public void showAvgRanks()
        {
            int ranks = 0;
            int counter = 0;

            foreach (var game in _recordList)
            {
                ranks = ranks + game.Position;
                counter++;
            }
            lbTotalGames.Content = "Total Games: " + counter;
            if (counter > 0) lbAvgRanks.Content = "Average Rank: " + (ranks / counter).ToString();
            else lbAvgRanks.Content = "Average Rank: first Game";


        }

        public void makeHeroList()
        {
            var viewList = _recordList.GroupBy(t => new { Hero = t.Hero })
             .Select(g => new {
                 //HeroImage = g.Select(l => l.HeroID),
                 Hero = g.Key.Hero,
                 Count = g.Count(),
                 Average = Math.Round(g.Average(p => p.Position),1)
                 
                 
             })
             .OrderByDescending(o => o.Count)
             ;

            dgGameHistory.ItemsSource = viewList;
        }

        public void mostTop3Hero()
        {

            var topThreePositionPerHero = _recordList
                .Where(x => x.Position <= 3)
                .GroupBy(t => new {Hero = t.Hero})
                .Select(g => new {
                    Hero = g.Key.Hero,
                    Count = g.Count()
                })
                .ToList();


            var bottomFivePositionPerHero = _recordList
                .Where(x => x.Position > 3)
                .GroupBy(t => new { Hero = t.Hero })
                .Select(g => new {
                    Hero = g.Key.Hero,
                    Count = g.Count()
                })
                .ToList();



            var query =
                from heroTop in topThreePositionPerHero
                join heroBottom in bottomFivePositionPerHero on heroTop.Hero equals heroBottom.Hero
                select new { Hero = heroTop.Hero, Amount = heroTop.Count - heroBottom.Count}
                ;


            lbBestHero.Content = "Best Hero: " + query.Where(x => x.Amount == query.Max(y => y.Amount)).Select(g => g.Hero).First();


        }

    }

}


//GridViewColumnHeader _lastHeaderClicked = null;
//ListSortDirection _lastDirection = ListSortDirection.Ascending;

//void GridViewColumnHeaderClickedHandler(object sender, RoutedEventArgs e)
//{
//    GridViewColumnHeader headerClicked = e.OriginalSource as GridViewColumnHeader;
//    ListSortDirection direction;

//    if (headerClicked != null)
//    {
//        if (headerClicked.Role != GridViewColumnHeaderRole.Padding)
//        {
//            if (headerClicked != _lastHeaderClicked)
//            {
//                direction = ListSortDirection.Ascending;
//            }
//            else
//            {
//                if (_lastDirection == ListSortDirection.Ascending)
//                {
//                    direction = ListSortDirection.Descending;
//                }
//                else
//                {
//                    direction = ListSortDirection.Ascending;
//                }
//            }

//            string header = headerClicked.Column.Header as string;
//            Sort(header, direction);

//            _lastHeaderClicked = headerClicked;
//            _lastDirection = direction;
//        }
//    }
//}

//private void Sort(string sortBy, ListSortDirection direction)
//{
//    ICollectionView dataView =
//      CollectionViewSource.GetDefaultView(lbtHeros.ItemsSource);

//    dataView.SortDescriptions.Clear();
//    SortDescription sd = new SortDescription(sortBy, direction);
//    dataView.SortDescriptions.Add(sd);
//    dataView.Refresh();
//}