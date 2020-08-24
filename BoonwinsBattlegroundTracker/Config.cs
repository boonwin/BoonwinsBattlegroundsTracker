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
        public static readonly string _configLocation = Hearthstone_Deck_Tracker.Config.AppDataPath + @"\Plugins\BoonwinsBattlegroundTracker\BoonwinsBattlegroundTracker.config";
        public readonly string _themeLocation  = Hearthstone_Deck_Tracker.Config.AppDataPath + @"\Plugins\BoonwinsBattlegroundTracker\Img\";
        public static readonly string _tribesImageLocation = Hearthstone_Deck_Tracker.Config.AppDataPath + @"\Plugins\BoonwinsBattlegroundTracker\Img\";
        public int TurnToStartTrackingAllBoards = 1;
        public bool showStatsOverlay = true;    
        public string backgroundImage = @"baseTheme.png";
        public int tribeSize = 0;
        public bool menuOverlayEnabled = true;
        public bool ingameOverlayEnabled = false;

        public string TrackerFontColor;
        public string MmrPlus;
        public string MmrMinus;
        public double posLeft = 20;
        public double posTop = 20;
        public double tribePosLeft = 320;
        public double tribePosTop = 20;
        public bool showTribeColors;
        public bool showTribeImages;
      
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
