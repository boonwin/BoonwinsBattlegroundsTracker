using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BoonwinsBattlegroundTracker
{
    public class Config
    {
          
        public static readonly string _configLocation = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData) + @"\BoonwinsBattlegroundTracker\data\BoonwinsBattlegroundTracker.config"; 
        public readonly string _gameRecordPath = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData) + @"\BoonwinsBattlegroundTracker\data\GameRecords.json";
        public readonly string _tribeAttributesPath = Hearthstone_Deck_Tracker.Config.AppDataPath + @"\Plugins\BoonwinsBattlegroundTracker\data\TribeAttributes.json";

        public string _themeLocation = Hearthstone_Deck_Tracker.Config.AppDataPath + @"\Plugins\BoonwinsBattlegroundTracker\Img\";
        public readonly string _soundLocation = Hearthstone_Deck_Tracker.Config.AppDataPath + @"\Plugins\BoonwinsBattlegroundTracker\Sounds\";
        public static readonly string _tribesImageLocation = Hearthstone_Deck_Tracker.Config.AppDataPath + @"\Plugins\BoonwinsBattlegroundTracker\Img\";
        public static readonly string _statsBestHeroBackgroundPath = _tribesImageLocation + @"stats\lichking.png";
        public static readonly string _statsBackgroundPath = _tribesImageLocation + @"stats\background.png";
        public int TurnToStartTrackingAllBoards = 1;
        public bool showStatsOverlay = true;    
        public string backgroundImage = @"baseTheme.png";
        public int tribeSize = 0;
        public bool menuOverlayEnabled = true;
        public bool ingameOverlayEnabled = false;
        public bool isSoundChecked = false;
        public bool isLeaderboardActivated = false;
        public bool showSimpleOverlay = true;
        public string leaderboardName;


        public string TrackerFontColor;
        public string MmrPlus;
        public string MmrMinus;
        public double posLeft = 20;
        public double posTop = 20;
        public double tribePosLeft = 320;
        public double tribePosTop = 20;
        public bool showTribeColors;
        public bool showTribeImages;

        public bool showConsole;
        internal bool showTurns = true;
        public bool IsMeanBobChecked = false;
        internal bool DisconectedThisGame = false;
        internal bool UseDisconect = false;
        public string GamePath;
        internal bool IsAdmin = false;
        internal bool SkipAll = false;
        internal bool DisconectWindowOpen;
        internal bool waitForRank;
        internal string player;

        public void save()
        {
            File.WriteAllText(_configLocation, JsonConvert.SerializeObject(this, Formatting.Indented));
            
        }

        public Config load()
        {
            if (File.Exists(_configLocation))
            {
                // load config from file, if available
                var config = JsonConvert.DeserializeObject<Config>(File.ReadAllText(_configLocation));

                return config;
            } return null;
        }

    }
}
