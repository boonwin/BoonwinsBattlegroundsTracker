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
            switch (Core.Game.CurrentRegion)
            {
                case Region.ASIA:
                    {
                        _recordList = GameRecord.LoadGameRecordFromFile(_config._gameRecordPath + "_AP");
                        break;
                    }
                case (Region.US):
                    {
                        _recordList = GameRecord.LoadGameRecordFromFile(_config._gameRecordPath + "_US");
                        break;
                    }
                case (Region.EU):
                    {

                        _recordList = GameRecord.LoadGameRecordFromFile(_config._gameRecordPath);
                        break;
                    }
            }
        }




    }
}
