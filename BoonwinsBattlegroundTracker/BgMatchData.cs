using System;
using System.Linq;
using System.Threading.Tasks;
using System.Collections.Generic;

using HearthDb.Enums;

using Hearthstone_Deck_Tracker.Enums;
using Hearthstone_Deck_Tracker.Hearthstone;
using Hearthstone_Deck_Tracker.API;
using Hearthstone_Deck_Tracker.Hearthstone.Entities;
using Hearthstone_Deck_Tracker.Utility;
using Hearthstone_Deck_Tracker.Utility.Logging;
using System.Windows;

namespace BoonwinsBattlegroundTracker
{


    public class BgMatchData
    {
        private static int _rating;
        private static int _ratingStart;
        private static bool _isStart = true;
        private static GameRecord _record;
        public static int lastRank;
        private static Config _config;
        private static Ranks _ranks;
        private static SettingsControl _settings;


        public static BgMatchOverlay Overlay;

        internal static bool InBgMode(string currentMethod)
        {
            if (Core.Game.CurrentMode != Hearthstone_Deck_Tracker.Enums.Hearthstone.Mode.BACON)
            {
                return false;
            }

            return true;
        }
 


        public static void OnLoad(Config config)
        {
            Log.Info($"onLoad - reading config, ceating ranks and gamerecord object");
            _config = config;
            _ranks = new Ranks();
            _record = new GameRecord();
            lastRank = 0;
        }
      
        internal static void GameStart()
        {
            //if (!InBgMode("Game Start")) return;
          
        }

        internal static void GameEnd()
        {
                    
            int playerId = Core.Game.Player.Id;
            Entity hero = Core.Game.Entities.Values
                .Where(x => x.IsHero && x.GetTag(GameTag.PLAYER_ID) == playerId)
                .First();
          
            _record.Position = hero.GetTag(GameTag.PLAYER_LEADERBOARD_PLACE);
            lastRank = hero.GetTag(GameTag.PLAYER_LEADERBOARD_PLACE);

            Log.Info($"Game ended Player Position is: { _record.Position }");
        }

        internal static void SetRank(int rank)
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
         internal static void InMenu()
        {
            Log.Info($"FUCKER I AM IN MENU ");
            if (lastRank > 0)
            {       
            SetRank(lastRank);
            Overlay.SetTextBoxValue(_ranks);
            }

        }
    
        internal static void Update()
        {
          
            // rating is only updated after we have passed the menu
            
            if (!InBgMode("Update")) return;

            int latestRating = Core.Game.BattlegroundsRatingInfo.Rating;

            if (_isStart) {
                _ratingStart = latestRating;
                _isStart = false;
            }
            else{
                int mmrChange =  latestRating - _ratingStart;  
                Overlay.UpdateMmrChangeValue(mmrChange);
            }
            

            _rating = latestRating;
            _record.Rating = _rating;
            Overlay.UpdateMMR(latestRating);


        }

     
    }

}
