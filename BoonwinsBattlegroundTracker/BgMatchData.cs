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
using static System.Windows.Visibility;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.IO;

namespace BoonwinsBattlegroundTracker
{


    public class BgMatchData
    {
        private static int _rating;
        private static int _ratingStart;
        private static bool _isStart = true;
        private static int _roundCounter = 0;
        private static int _lastRoundNr = 0;
        private static GameRecord _record;
        private static int lastRank;
        private static Config _config;
        private static Ranks _ranks;
        private static string _avgRank;
        private static int _tribeImgSize;

        public static BgMatchOverlay _overlay;
        public static View _view;
        public static TribesOverlay _tribes;

        public static OverlayManager _input;
        public static TriverOverlayManager _tribeInput;
      

        internal static void TurnStart(ActivePlayer player)
        {
            if (!InBgMode()) return;
            int turn = Core.Game.GetTurnNumber();
            if (turn == 1) SetMissingRace();
            
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
            _tribeImgSize = _config.tribeSize;
            _view.SetAvgRank(_avgRank);
            _view.SetMMR(_rating);
            _view.SetisBannedGameStart();
        }



        internal static void GameEnd()
        {
            _lastRoundNr++;

            if (Core.OverlayCanvas.Children.Contains(_tribes))
            {
               
                    Log.Info($"I Guess I remove it here then.");
                    Core.OverlayCanvas.Children.Remove(_tribes);
              
            }

            int playerId = Core.Game.Player.Id;
            Entity hero = Core.Game.Entities.Values
                .Where(x => x.IsHero && x.GetTag(GameTag.PLAYER_ID) == playerId)
                .First();

            _record.Position = hero.GetTag(GameTag.PLAYER_LEADERBOARD_PLACE);
            lastRank = hero.GetTag(GameTag.PLAYER_LEADERBOARD_PLACE);

            if (lastRank > 0)
            {
                SetRank(lastRank);
                CalcAvgRank(_ranks);
                _overlay.SetTextBoxValue(_ranks, _avgRank);
            }

            Log.Info($"Game ended Player Position is: { _record.Position }");
        }

        internal static void SetMissingRace()
        {
            
            var gameID = Core.Game.CurrentGameStats.GameId;            
            GetMissingRaceString(gameID);

        }

        internal static void GetMissingRaceString(Guid? gameID)
        {
  
            var races = BattlegroundsUtils.GetAvailableRaces(gameID);
            var total = 113;

            foreach (var race in races)
            {
                total -= (int)race;
            }

            if (total == 14)
            {
                _view.SetisBanned("Murlocs");
                if (_config.showTribeColors == true)
                {
                    _view.spBanned.Background = (Brush)new BrushConverter().ConvertFrom("#048519");
                }
                if (_config.showTribeImages == true)
                {
                    if(File.Exists(Config._tribesImageLocation + @"murloc.png")) { 
                    _tribes.imgTribes.Source = new BitmapImage(new Uri(Config._tribesImageLocation + @"murloc.png"));
                    }
                }

            }
            else if (total == 15)
            {
                _view.SetisBanned("Demons");
                if (_config.showTribeColors == true)
                {
                    _view.spBanned.Background = (Brush)new BrushConverter().ConvertFrom("#340065");
                }
                if (_config.showTribeImages == true)
                {
                    if (File.Exists(Config._tribesImageLocation + @"demon.png"))
                    {
                        _tribes.imgTribes.Source = new BitmapImage(new Uri(Config._tribesImageLocation + @"demon.png"));
                    }
                }
            }
            else if (total == 17)
            {
                _view.SetisBanned("Mechs");
                if (_config.showTribeColors == true)
                {
                    _view.spBanned.Background = (Brush)new BrushConverter().ConvertFrom("#008b89");
                }
                if (_config.showTribeImages == true)
                {
                    if (File.Exists(Config._tribesImageLocation + @"mech.png"))
                    {
                        _tribes.imgTribes.Source = new BitmapImage(new Uri(Config._tribesImageLocation + @"mech.png"));
                    }
                }
            }

            else if (total == 18)
            {
                _view.SetisBanned("Elementals");
                if (_config.showTribeColors == true)
                {
                    _view.spBanned.Background = (Brush)new BrushConverter().ConvertFrom("#dacd01");
                }
                if (_config.showTribeImages == true)
                {
                    if (File.Exists(Config._tribesImageLocation + @"elementals.png"))
                    {
                        _tribes.imgTribes.Source = new BitmapImage(new Uri(Config._tribesImageLocation + @"elementals.png"));
                    }
                }
            }


            else if (total == 20)
            {
                _view.SetisBanned("Beasts");
                if (_config.showTribeColors == true)
                {
                    _view.spBanned.Background = (Brush)new BrushConverter().ConvertFrom("#714800");
                }
                if (_config.showTribeImages == true)
                {
                    if (File.Exists(Config._tribesImageLocation + @"beast.png"))
                    {
                        _tribes.imgTribes.Source = new BitmapImage(new Uri(Config._tribesImageLocation + @"beast.png"));
                    }
                }
            }
            else if (total == 23)
            {
                _view.SetisBanned("Pirates");
                if (_config.showTribeColors == true)
                {
                    _view.spBanned.Background = (Brush)new BrushConverter().ConvertFrom("#590000");
                }
                if (_config.showTribeImages == true)
                {
                    if (File.Exists(Config._tribesImageLocation + @"pirate.png"))
                    {
                        _tribes.imgTribes.Source = new BitmapImage(new Uri(Config._tribesImageLocation + @"pirate.png"));
                    }
                }
            }
            else if (total == 24)
            {
                _view.SetisBanned("Dragons");
                if (_config.showTribeColors == true)
                {
                    _view.spBanned.Background = (Brush)new BrushConverter().ConvertFrom("#042e85");
                }
                if (_config.showTribeImages == true)
                {
                    if (File.Exists(Config._tribesImageLocation + @"dragon.png"))
                    {
                        _tribes.imgTribes.Source = new BitmapImage(new Uri(Config._tribesImageLocation + @"dragon.png"));
                    }
                }
            }
            else
            {
                _view.SetisBanned("N/A");
            }

            if (_config.showTribeImages == true)
            {
                if (!Core.OverlayCanvas.Children.Contains(_tribes)){
                    Core.OverlayCanvas.Children.Add(_tribes);
                }
            }else {
                Log.Info($" KEKL.");
                if (Core.OverlayCanvas.Children.Contains(_tribes))
                {
                    Core.OverlayCanvas.Children.Remove(_tribes);
                } 
            }
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

        public static void CalcAvgRank(Ranks rank)
        {
            double totalAmount = rank.rank1Amount + rank.rank2Amount + rank.rank3Amount + rank.rank4Amount + rank.rank5Amount + rank.rank6Amount + rank.rank7Amount + rank.rank8Amount;
            double weightedAmount = (1 * rank.rank1Amount) + (2 * rank.rank2Amount) + (3 * rank.rank3Amount) + (4 * rank.rank4Amount) + (5 * rank.rank5Amount) + (6 * rank.rank6Amount) + (7 * rank.rank7Amount) + (8 * rank.rank8Amount);

            if (_overlay.tbTotalGames.Visibility == Visibility.Visible)
            {
                _overlay.tbTotalGames.Content = "Games: " + totalAmount.ToString();
            }
            if (totalAmount != 0)
            {
                _avgRank = Math.Round((weightedAmount / totalAmount), MidpointRounding.AwayFromZero).ToString();
            }
        }

        internal static bool InBgMenu()
        {
            if (Core.Game.CurrentMode != Hearthstone_Deck_Tracker.Enums.Hearthstone.Mode.BACON) return false;           
            else return true;
        }
        internal static bool InBgMode()
        {
            if (Core.Game.CurrentGameMode != GameMode.Battlegrounds)return false;
            else return true;
        }

        internal static void AddOrRemoveOverlay()
        {
            if (InBgMenu()) {
                if (!Core.OverlayCanvas.Children.Contains(_overlay))
                {
                    Core.OverlayCanvas.Children.Add(_overlay);
                }            
            } else {
                if (Core.OverlayCanvas.Children.Contains(_overlay))
                {
                    if (InBgMode() && !_config.ingameOverlayEnabled) { 
                    Core.OverlayCanvas.Children.Remove(_overlay);
                    }
                    if (Core.Game.CurrentMode == Hearthstone_Deck_Tracker.Enums.Hearthstone.Mode.HUB)
                    {
                        Core.OverlayCanvas.Children.Remove(_overlay);
                    }                 
                 }       
            }
      
        }

   
        internal static void Update()
        {
            // rating is only updated after we have passed the menu
            AddOrRemoveOverlay();
            SetTribeImgSize();

            if (!InBgMenu()) return;
            if (_isStart) SetLatestRating();
            if (_lastRoundNr > _roundCounter) 
            {
                SetMMRChange();
            }

        }
        internal static void SetTribeImgSize()
        {
            if (_config.showTribeImages && _config.tribeSize != _tribeImgSize)
            {
                _tribes.SetTribeImageSize(_config.tribeSize);
                _tribeImgSize = _config.tribeSize;
            }
        }

        internal static void SetLatestRating()
        {
            _ratingStart = Core.Game.BattlegroundsRatingInfo.Rating;
            _overlay.UpdateMMR(_ratingStart);
            _isStart = false;
        }

        internal static void SetMMRChange()
        {
            _roundCounter = _lastRoundNr;
            int latestRating = Core.Game.BattlegroundsRatingInfo.Rating;
            int mmrChange = latestRating - _ratingStart;
            _overlay.UpdateMmrChangeValue(mmrChange);
            _rating = latestRating;
            _record.Rating = _rating;
            _overlay.UpdateMMR(latestRating);

        }


    }

}
