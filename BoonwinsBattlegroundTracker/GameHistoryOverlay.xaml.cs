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
        public string[] Labels { get; set; }
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

  //      new CartesianMapper<double>()
  //.X((value, index) => value) //use the value as X
  //.Y((value, index) => index) //use the index as Y
        //var mapper = Mappers.Xy<MyClass>().X(v => v.XProp).Y(v => v.YProp);
        //var seriesCollection = new SeriesCollection(mapper);
        //myChart.SeriesCollection = seriesCollection;

        public void MakeSeries()
        {
            SeriesCollection = new SeriesCollection();
            var MmrValues = new ChartValues<int>();

            _recordList.ForEach(x => { if (x.Mmr != 0) MmrValues.Add(x.Mmr); });
                     

            Labels = new[] {"Game"};


            //modifying the series collection will animate and update the chart
            SeriesCollection.Add(new LineSeries
            {
                Title = "MMR",
                LineSmoothness = 0, //0: straight lines, 1: really smooth lines
                Values = MmrValues
           
            }) ;



            DataContext = this;

        }

 
    }

}