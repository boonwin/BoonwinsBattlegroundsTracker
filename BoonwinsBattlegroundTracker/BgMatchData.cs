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
        private static GameRecord _record;
        public static int lastRank;
        private static Config _config;
        private static Ranks _ranks;
        private static string _avgRank;


        public static BgMatchOverlay _overlay;
        public static View _view;
        public static TribesOverlay _tribes;

        public static OverlayManager _input;
        public static TriverOverlayManager _tribeInput;


        internal static bool InBgMenu(string currentMethod)
        {
            if (Core.Game.CurrentMode != Hearthstone_Deck_Tracker.Enums.Hearthstone.Mode.BACON)
            {
        
                if (_config.menuOverlayEnabled == true)
                {
                    if (!Core.OverlayCanvas.Children.Contains(_overlay))
                    {
                        Core.OverlayCanvas.Children.Add(_overlay); 
                    }
                    return false;
                }
                else
                {
                    if (Core.OverlayCanvas.Children.Contains(_overlay) == true)
                    {
                        Core.OverlayCanvas.Children.Remove(_overlay);
                    }
                    return false;
                }
            } else {
               
                //if (Core.OverlayCanvas.Children.Contains(Tribes))
                //{
                   
                //        Log.Info($"{currentMethod} - IM A FREAKING BUG YOU TARD.");
                //        Core.OverlayCanvas.Children.Remove(Tribes);
                   
                //}

                if (_config.menuOverlayEnabled == true)
                {
                    if (Core.OverlayCanvas.Children.Contains(_overlay) == false)
                    {
                        Core.OverlayCanvas.Children.Add(_overlay);                        
                    }
                    return true;
                } else
                {
                    if (Core.OverlayCanvas.Children.Contains(_overlay) == true)
                    {
                        Core.OverlayCanvas.Children.Remove(_overlay);
                    }
                    return false;
                }
            }

        }
        internal static bool InBgMode(string currentMethod)
        {
            if (Core.Game.CurrentGameMode != GameMode.Battlegrounds)
            {
                Log.Info($"{currentMethod} - Not in Battlegrounds Mode.");
                return false;
            }
            return true;
        }

        internal static bool MulliganDone(string currentMethod)
        {
           if(Core.Game.IsMulliganDone != true) {
                Log.Info($"{currentMethod} - Shits not ready yet.");
                return false;
            } return true;
        }

        internal static void TurnStart(ActivePlayer player)
        {
            if (!InBgMode("Turn Start")) return;
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
          
            if (!_config.ingameOverlayEnabled) {
                
                Core.OverlayCanvas.Children.Remove(_overlay);
            }

            _view.SetAvgRank(_avgRank);
            _view.SetMMR(_rating);
            _view.SetisBannedGameStart();
        }



        internal static void GameEnd()
        {

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

            Log.Info($"Game ended Player Position is: { _record.Position }");
        }

        internal static void SetMissingRace()
        {
            
            var gameID = Core.Game.CurrentGameStats.GameId;            
            GetMissingRaceString(gameID);

        }

        internal static void GetMissingRaceString(Guid? gameID)
        {
            SetTribeImageSize();

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
                Core.OverlayCanvas.Children.Add(_tribes);
            }else {
                Log.Info($" KEKL.");
                Core.OverlayCanvas.Children.Remove(_tribes); }
        }

        private static void SetTribeImageSize()
        {
            switch (_config.tribeSize)
            {
                case 0:
                    _tribes.imgTribes.Width = 150;
                    _tribes.imgTribes.Height = 150;
                   
                    break;
                case 1:
                    _tribes.imgTribes.Width = 200;
                    _tribes.imgTribes.Height = 200;
                   

                    break;
                case 2:
                    _tribes.imgTribes.Width = 250;
                    _tribes.imgTribes.Height = 250;
                   
                    break;
                case 3:
                    _tribes.imgTribes.Width = 300;
                    _tribes.imgTribes.Height = 300;
                   
                    break;
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


        internal static void InMenu()
        {

            if (_config.menuOverlayEnabled == true)
            {
                if (!Core.OverlayCanvas.Children.Contains(_overlay))
                {
                    Core.OverlayCanvas.Children.Add(_overlay);
                }
            }
            else 
            {
                if (Core.OverlayCanvas.Children.Contains(_overlay)) { 
                Core.OverlayCanvas.Children.Remove(_overlay);
                }

            }

            if (lastRank > 0)
            {
                SetRank(lastRank);
                CalcAvgRank(_ranks);
                _overlay.SetTextBoxValue(_ranks, _avgRank);
            }

        }

        internal static void Update()
        {

            // rating is only updated after we have passed the menu

            if (!InBgMenu("Update")) return;

            
            int latestRating = Core.Game.BattlegroundsRatingInfo.Rating;

            if (_isStart)
            {
                _ratingStart = latestRating;
                _isStart = false;
            }
            else
            {
                int mmrChange = latestRating - _ratingStart;
                _overlay.UpdateMmrChangeValue(mmrChange);
            }


            _rating = latestRating;
            _record.Rating = _rating;
            _overlay.UpdateMMR(latestRating);



        }


    }

}
