using Hearthstone_Deck_Tracker;
using Hearthstone_Deck_Tracker.Enums;
using LiveCharts;
using LiveCharts.Wpf;
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
    /// 
    public partial class GameHistoryOverlay : UserControl
    {

        Config _config = new Config();
        List<GameRecord> _recordList = new List<GameRecord>();
        public SeriesCollection SeriesCollection { get; set; }
        public string[] Labels1 { get; set; }
        public string[] Labels2 { get; set; }
        public string[] Labels3 { get; set; }
        public Func<double, string> YFormatter { get; set; }

        public GameHistoryOverlay()
        {

            InitializeComponent();

           

            try
            {
                _recordList = GameRecord.LoadGameRecordFromFile(_config._gameRecordPath);
                MakeSeries();
            }
            catch
            {
                MessageBox.Show("You cant show Stats yet, maybe you didnt play anygame so far.", "Warning", MessageBoxButton.OK, MessageBoxImage.Warning);
            };

        }

        public void MakeSeries()
        {
            
            SeriesCollection = new SeriesCollection();         
            var regionGroupedList = _recordList.Where(x => x.Region != 0 && x.Mmr > 0).GroupBy(g => g.Region);

            foreach (var region in regionGroupedList)
            {
                var MmrValues = new ChartValues<int>();
                List<string> DateLabels = new List<string>();
                foreach (var record in region.Where(x => x.Mmr > 0))
                {
                    MmrValues.Add(record.Mmr);
                    DateLabels.Add(record.Hero);
                }

                SeriesCollection.Add(
                    new LineSeries{

                            Title = (region.Key).ToString(),
                            LineSmoothness = 0, //0: straight lines, 1: really smooth lines
                            Values = MmrValues,
                            
                    }
                    
                );
              
                //switch (region.Key)
                //{
                //    case Region.US:
                //        Labels1 = DateLabels.ToArray();
                //        break;
                //    case Region.EU:
                //        Labels2 = DateLabels.ToArray();
                //        break;
                //    case Region.ASIA:
                //        Labels3 = DateLabels.ToArray();
                //        break;
                //    case Region.CHINA:
                //        break;
                //}
                
                DataContext = this;
            }
        }

 
    }

}