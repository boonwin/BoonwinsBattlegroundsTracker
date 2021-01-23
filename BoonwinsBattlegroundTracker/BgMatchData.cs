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
using System.Threading;
using System.Windows.Controls;
using Hearthstone_Deck_Tracker.LogReader.Interfaces;
using MahApps.Metro.Controls;
using System.Media;
using Hearthstone_Deck_Tracker.Windows;
using System.Reflection;

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
        private static List<GameRecord> _recordList;

        private static int lastRank;
        private static Config _config;
        private static Ranks _ranks;
        private static string _peak;
        private static string _avgRank;
        private static int _tribeImgSize;
        public static bool IsMissingTribeRetrieved;
        public static bool IsMissingHeroesRetrieved;

        public static BgMatchOverlay _overlay;
        public static SimpleOverlay _simpleOverlay;
        public static View _view;
        public static TribesOverlay _tribes;
        public static ConsoleOverlay _console;
        public static InGameDisconectorOverlay _cheatButtonForNoobs;

        public static OverlayManager _input;

        

        public static TriverOverlayManager _tribeInput;

        public static HashSet<Race> _avaiableTribes;
        public static string[] _avaiableHeroes;
        internal static int _lastknownTurn = 0;
        private static int _leaderboardRatingNextRank = 0;
        private static string _leaderBoardRank = "none";

        internal List<FrameworkElement> frameworkElements = new List<FrameworkElement>();

        public static void OnLoad(Config config)
        {
            Log.Info($"onLoad - reading config, ceating ranks and gamerecord object");
            _config = config;
            _ranks = new Ranks();
            LoadGameRecordFromFile();
            AddRemoveDisconectButton();
            lastRank = 0;

        }

        internal static void InMenu()
        {
        }

        internal static async void GameStart()
        {
            if (!Core.Game.Spectator)
            {
                _console.SetConsoleText("Game with the ID " + Core.Game.CurrentGameStats.GameId + " started.");


                IsMissingTribeRetrieved = false;
                _tribeImgSize = _config.tribeSize;
                

                int waitTime = 6000;

                while (!IsMissingTribeRetrieved && waitTime > 0)
                {
                    Thread.Sleep(3000);
                    waitTime -= 3000;
                    IsMissingTribeRetrieved = SetMissingRace();

                }

              

                var avaiableHeroes = await SetPersonalHeroRating();

                _console.SetConsoleText("Getting Game Informations");

                try { 
                    _peak = GameRecord.GetPeak(_recordList, Core.Game.CurrentRegion);
                    GameRecord.GetHeroWinRating(_recordList, avaiableHeroes, _avaiableTribes, _console);
                }
                catch { }

                _view.SetPeak(_peak);

                if (_rating > 0) _view.SetMMR(_rating);
                else _view.SetMMR(_ratingStart);
            
            }
        }
        internal static void TurnStart(ActivePlayer player)
        {
            if (!Core.Game.Spectator)
            {
                if (!InBgMode()) return;
                try
                {
                    Entity hero = Core.Game.Entities.Values
                        .Where(x => x.IsHero && x.GetTag(GameTag.PLAYER_ID) == Core.Game.Player.Id)
                        .First();

                    if (_lastknownTurn != Core.Game.GetTurnNumber() && _config.showTurns)
                    {
                        _console.SetConsoleText("Turn #" + Core.Game.GetTurnNumber() + " started. You have " + " " + hero.Health + " HP left.");
                        _lastknownTurn = Core.Game.GetTurnNumber();
                    }
                }
                catch { }
            }
        }

        internal static void GameEnd()
        {
            
            if (!Core.Game.Spectator)
            {
                
                _lastRoundNr++;
                _view.SetisBannedGameStart();

                if (Core.OverlayCanvas.Children.Contains(_tribes))
                {
                    Core.OverlayCanvas.Children.Remove(_tribes);

                }

                int playerId = Core.Game.Player.Id;
                Entity hero = Core.Game.Entities.Values
                    .Where(x => x.IsHero && x.GetTag(GameTag.PLAYER_ID) == playerId)
                    .First();

                GetGameRecordData(hero);
               
                lastRank = hero.GetTag(GameTag.PLAYER_LEADERBOARD_PLACE);
               
                if (lastRank > 0)
                {
                    Ranks.SetRank(lastRank,_ranks);
                    CalcAvgRank(_ranks);
                    _overlay.SetRanksForOverlay(_ranks, _avgRank);
                    _simpleOverlay.SetLastRank(lastRank);
                    UpdateLeaderboardData();
                    meanbob.meanBobLines(lastRank,_config);                   
                    if(lastRank == 8) RankEight();
                }
                
           
                
                _console.SetConsoleText("Game with the ID " + Core.Game.CurrentGameStats.GameId + " ended.");
            }

        }

        internal static async Task<string[]> SetPersonalHeroRating()
        {
            string[] avaiableHeroes = null;

            for (var i = 0; i < 10; i++)
            {
                await Task.Delay(500);
                var heroes = Core.Game.Player.PlayerEntities.Where(x => x.IsHero && x.HasTag(GameTag.BACON_HERO_CAN_BE_DRAFTED));
                if (heroes.Count() < 2)
                    continue;
                await Task.Delay(500);
                avaiableHeroes = heroes.Select(x => x.Card.LocalizedName).ToArray();

            }

            return avaiableHeroes;
        }

        private static void LoadGameRecordFromFile()
        {
            _recordList = GameRecord.LoadGameRecordFromFile(_config._gameRecordPath);            
        }

        private static void WriteGameRecordToFile(GameRecord record)
        {
           GameRecord.WriteGameRecordToFile(_config._gameRecordPath, record);
        }

        private static void GetGameRecordData(Entity hero)
        {
            _record = new GameRecord();
            _record.DateTime = DateTime.Now;
            _record.Position = hero.GetTag(GameTag.PLAYER_LEADERBOARD_PLACE);
            _record.Hero = hero.Card.LocalizedName;
            _record.HeroID = hero.Card.Id;
            _record.Tribes = _avaiableTribes;
            _record.GameID = Core.Game.CurrentGameStats.GameId;
            _record.Player = Core.Game.Player.Name;
            _record.Region = Core.Game.CurrentRegion;
        }    

        private static void GetMmrRecordData(GameRecord record)
        {
            record.Mmr = Core.Game.BattlegroundsRatingInfo.Rating;
          
        }


        //Most of this should be in another class KEKL
        internal static bool SetMissingRace()
        {

            var gameID = Core.Game.CurrentGameStats.GameId;

            _avaiableTribes = Tribes.GetTribes(gameID, _view, _config, _tribes);


            if (_avaiableTribes != null && _avaiableTribes.Count > 0)
            {
                if (!_avaiableTribes.Contains(Race.INVALID))
                {
                    Tribes.SetBannedTribes(gameID, _view, _config, _tribes);

                    if (_config.showTribeImages == true)
                    {
                        if (!Core.OverlayCanvas.Children.Contains(_tribes))
                        {
                            Core.OverlayCanvas.Children.Add(_tribes);
                        }
                    }
                    else
                    {
                        Log.Info($" KEKL.");
                        if (Core.OverlayCanvas.Children.Contains(_tribes))
                        {
                            Core.OverlayCanvas.Children.Remove(_tribes);
                        }
                    }
                    return true;
                }
                return false;
            }
            return false;
        }


       

        public static void RankEight()
        {
            if (_config.isSoundChecked)
            {
                if (File.Exists(_config._soundLocation + "top8.wav"))
                {               
                    SoundPlayer player = new SoundPlayer(_config._soundLocation + "top8.wav");
                    player.Play();
                }
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
            if (Core.Game.CurrentGameMode != GameMode.Battlegrounds) return false;
            else return true;
        }
        internal static bool InGameplayMode()
        {
            if (Core.Game.CurrentMode != Hearthstone_Deck_Tracker.Enums.Hearthstone.Mode.GAMEPLAY) return false;
            else return true;
        }
        internal static bool InGameHub()
        {
            if (Core.Game.CurrentMode != Hearthstone_Deck_Tracker.Enums.Hearthstone.Mode.HUB) return false;
            else return true;
        }
        internal static void AddRemoveConsole()
        {
            if (_config.showConsole)
            {
                if (!Core.OverlayCanvas.Children.Contains(_console))
                {
                    Core.OverlayCanvas.Children.Add(_console);
                }
            }
            else
            {
                if (Core.OverlayCanvas.Children.Contains(_console))
                {
                    Core.OverlayCanvas.Children.Remove(_console);
                }
            }
        }



        internal static void AddRemoveDisconectButton()
        {
            Window DisconectButtonWindow = new Window()
            {
                Title = "Boomer Button",
                Content = new InGameDisconectorOverlay(),
                Height = 113.861,
                Width = 211.646,

                ResizeMode = ResizeMode.NoResize
            };

            DisconectButtonWindow.Show();
        }

        internal static void AddOrRemoveOverlay()
        {
            if (InBgMenu())
            {
                if (_config.menuOverlayEnabled)
                {
                    if (!Core.OverlayCanvas.Children.Contains(_overlay))
                    {
                        Core.OverlayCanvas.Children.Add(_overlay);
                    }
                }
                else
                {
                    if (Core.OverlayCanvas.Children.Contains(_overlay))
                    {
                        Core.OverlayCanvas.Children.Remove(_overlay);
                    }
                }

            }
            else
            {
                if (InGameplayMode())
                {
                    if (InBgMode())
                    {
                        if (!_config.ingameOverlayEnabled)
                        {
                            if (Core.OverlayCanvas.Children.Contains(_overlay))
                            {
                                Core.OverlayCanvas.Children.Remove(_overlay);
                            }
                        }
                        else
                        {
                            if (!Core.OverlayCanvas.Children.Contains(_overlay))
                            {
                                Core.OverlayCanvas.Children.Add(_overlay);
                            }
                        }
                    }
                }
                if (InGameHub())
                {
                    if (Core.OverlayCanvas.Children.Contains(_overlay))
                    {
                        Core.OverlayCanvas.Children.Remove(_overlay);
                    }
                }

            }

        }

      
        internal static void Update()
        {
            if (!Core.Game.Spectator)
            {
               
                // rating is only updated after we have passed the menu
                AddOrRemoveOverlay();
                SetTribeImgSize();
                AddRemoveConsole();
                AddRemoveSimpleOverlay();
                AddRemoveLeaderboard();
               


                if (!InBgMenu()) return;
                OpenAddRankPrompt();
                if (_isStart) SetLatestRating();
                if (_lastRoundNr > _roundCounter)
                {
                    SetMMRChange();
                    GetMmrRecordData(_record);
                    _recordList.Add(_record);
                    WriteGameRecordToFile(_record);
                }

            }

        }
        public static int ToggleDisconect()
        {
           return  Disconnector.RuleSwitcher(_config);
        }

        internal static async void UpdateLeaderboardData()
        {
            if (_config.isLeaderboardActivated) { 
            if (InBgMenu())
            {

                if (!String.IsNullOrEmpty(_config.leaderboardName))
                {
                    switch (Core.Game.CurrentRegion)
                    {
                        case Region.ASIA:
                            {
                                _overlay.tbLeaderboard.Content = "AS-Leaderboard:";
                                (_leaderBoardRank, _leaderboardRatingNextRank) = await Leaderboard.GetLeaderboard("AP", _config.leaderboardName);
                                break;
                            }
                        case (Region.US):
                            {
                                _overlay.tbLeaderboard.Content = "NA-Leaderboard:";
                                (_leaderBoardRank, _leaderboardRatingNextRank) = await Leaderboard.GetLeaderboard("US", _config.leaderboardName);
                                break;
                            }
                        case (Region.EU):
                            {
                                _overlay.tbLeaderboard.Content = "EU-Leaderboard:";
                                (_leaderBoardRank, _leaderboardRatingNextRank) = await Leaderboard.GetLeaderboard("EU", _config.leaderboardName);
                                break;
                            }
                    }
                    if (!String.IsNullOrEmpty(_rating.ToString()) && _rating == 0)
                    {
                        var neededMMRForLeaderboard = _leaderboardRatingNextRank - _ratingStart;
                        _overlay.tbLeaderboardRatingDifference.Content = neededMMRForLeaderboard;

                    }
                    _overlay.tbLeaderboardRank.Content = _leaderBoardRank;

                }
            }
            if (InGameplayMode() && InBgMode())
            {
                switch (Core.Game.CurrentRegion)
                {
                    case Region.ASIA:
                        {
                            _overlay.tbLeaderboard.Content = "AS-Leaderboard:";
                            (_leaderBoardRank, _leaderboardRatingNextRank) = await Leaderboard.GetLeaderboard("AP", _config.leaderboardName);
                            break;
                        }
                    case (Region.US):
                        {
                            _overlay.tbLeaderboard.Content = "NA-Leaderboard:";
                            (_leaderBoardRank, _leaderboardRatingNextRank) = await Leaderboard.GetLeaderboard("US", _config.leaderboardName);
                            break;
                        }
                    case (Region.EU):
                        {
                            _overlay.tbLeaderboard.Content = "EU-Leaderboard:";
                            (_leaderBoardRank, _leaderboardRatingNextRank) = await Leaderboard.GetLeaderboard("EU", _config.leaderboardName);
                            break;
                        }
                }

                if (!String.IsNullOrEmpty(_rating.ToString()) && _rating > 0)
                {
                    var neededMMRForLeaderboard = _leaderboardRatingNextRank - _rating;
                    _overlay.tbLeaderboardRatingDifference.Content = neededMMRForLeaderboard;
                }
            }

        }

        }



        internal static void AddRemoveLeaderboard()
        {
            if (_config.isLeaderboardActivated && !String.IsNullOrEmpty(_config.leaderboardName))
            {
                _overlay.tbLeaderboard.Visibility = Visibility.Visible;
                _overlay.tbLeaderboardRank.Visibility = Visibility.Visible;
                _overlay.tbLeaderboardRating.Visibility = Visibility.Visible;
                _overlay.tbLeaderboardRatingDifference.Visibility = Visibility.Visible;

            }
            else
            {
                _overlay.tbLeaderboard.Visibility = Visibility.Hidden;
                _overlay.tbLeaderboardRank.Visibility = Visibility.Hidden;
                _overlay.tbLeaderboardRating.Visibility = Visibility.Hidden;
                _overlay.tbLeaderboardRatingDifference.Visibility = Visibility.Hidden;
            }
        }

        internal static void AddRemoveSimpleOverlay()
        {
            if (InBgMenu())
            {
                if (_config.showSimpleOverlay)
                {
                    if (!Core.OverlayCanvas.Children.Contains(_simpleOverlay))
                    {
                        Core.OverlayCanvas.Children.Add(_simpleOverlay);
                    }
                }
                else
                {
                    if (Core.OverlayCanvas.Children.Contains(_simpleOverlay))
                    {
                        Core.OverlayCanvas.Children.Remove(_simpleOverlay);
                    }
                }

            }
            else
            {
                if (InGameplayMode())
                {
                    if (InBgMode())
                    {
                        if (!_config.showSimpleOverlay)
                        {
                            if (Core.OverlayCanvas.Children.Contains(_simpleOverlay))
                            {
                                Core.OverlayCanvas.Children.Remove(_simpleOverlay);
                            }
                        }
                        else
                        {
                            if (!Core.OverlayCanvas.Children.Contains(_simpleOverlay))
                            {
                                Core.OverlayCanvas.Children.Add(_simpleOverlay);
                            }
                        }
                    }
                }
                if (InGameHub())
                {
                    if (Core.OverlayCanvas.Children.Contains(_simpleOverlay))
                    {
                        Core.OverlayCanvas.Children.Remove(_simpleOverlay);
                    }
                }

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

            UpdateLeaderboardData();
            _isStart = false;
        }

        internal static void SetMMRChange()
        {
            _roundCounter = _lastRoundNr;
            int latestRating = Core.Game.BattlegroundsRatingInfo.Rating;
            int mmrChange = latestRating - _ratingStart;
            _overlay.UpdateMmrChangeValue(mmrChange);
            _rating = latestRating;
            _overlay.UpdateMMR(latestRating);

        }

        public static void AddRankManualy(int rank)
        {
            Ranks.AddManual(rank, _ranks);
        }

        internal static void OpenAddRankPrompt()
        {
            if (InBgMenu())
            {
                if (_config.UseDisconect)
                {
                    if (_config.DisconectedThisGame)
                    {
                        //add new wpf for using disconect in game and handling the manual rank
                        _config.DisconectedThisGame = false;
                        Window AddRankWindow = new Window()
                        {
                            Title = "You used the disconect feature, chame on you! Now you have to enter your rank manually. Boomer",
                            Content = new AddRankPrompt(),

                            ResizeMode = ResizeMode.NoResize
                        };

                        AddRankWindow.Show();
                        AddRankPrompt.GetWindowName(AddRankWindow);
                    }
                }
            }
        }

    }

}
