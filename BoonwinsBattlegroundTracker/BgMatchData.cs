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
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.IO;
using System.Threading;
using System.Windows.Controls;
using Hearthstone_Deck_Tracker.LogReader.Interfaces;
using MahApps.Metro.Controls;
using System.Media;
using System.Security.Principal;
using Hearthstone_Deck_Tracker.Windows;
using System.Reflection;
using BoonConector.Service;
using BoonwinsBattlegroundTracker.Overlays;
using Newtonsoft.Json;

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
        private static TribeAttributesList _tribeAttributesList;


        private static BoonLeague _boon;

        private static int lastRank;
        private static Config _config;
        private static Ranks _ranks;
        private static string _peak;
        private static string _avgRank;
        private static int _tribeImgSize;
        public static bool IsMissingTribeReceived;
        public static bool IsMissingHeroesRetrieved;

        public static BgMatchOverlay _overlay;
        public static SimpleOverlay _simpleOverlay;
        public static InBattleMmrScore _mmrView;
        public static TribesOverlay _tribes;
        //public static ConsoleOverlay _console;
        public static InGameDisconectorOverlay _cheatButtonForNoobs;


        public static OverlayManager _input;
        public static TriverOverlayManager _tribeInput;


        public static string[] _avaiableHeroes;
        internal static int _lastknownTurn = 0;
        private static int _leaderboardRatingNextRank = 0;
        private static string _leaderBoardRank = "none";
        private static HashSet<Race> _avaiableTribes;
        private static string _unavailableTribes = null;



        public static void OnLoad(Config config)
        {
            Log.Info($"onLoad - reading config, creating ranks and gamerecord object");
            _config = config;
            _ranks = new Ranks();
            LoadGameRecordFromFile();
            LoadTribeAttributesFromFile();
            lastRank = 0;
            IsAdmin();
            boonApiConnector.InitializeClient();
        }

        private static void LoadTribeAttributesFromFile()
        {
            _tribeAttributesList = JsonConvert.DeserializeObject<TribeAttributesList>(File.ReadAllText(_config._tribeAttributesPath));
        }

        internal static void IsAdmin()
        {

            using (WindowsIdentity identity = WindowsIdentity.GetCurrent())
            {
                WindowsPrincipal principal = new WindowsPrincipal(identity);
                _config.IsAdmin = principal.IsInRole(WindowsBuiltInRole.Administrator);
                if (!_config.IsAdmin)
                {
                    _config.UseDisconect = false;
                }

            }

        }

        internal static void InMenu()
        {
        }

        internal static void GameStart()
        {
            if (!InBgMode()) return;
            if (!Core.Game.Spectator)
            {

                
                //_console.SetConsoleText("Game with the ID " + Core.Game.CurrentGameStats.GameId + " started.");
                if (String.IsNullOrEmpty(_config.player))
                {
                    _config.player = Core.Game.Player.Name;
                }
              
                try
                {
                    _peak = GameRecord.GetPeak(_recordList, Core.Game.CurrentRegion);
                    //GameRecord.GetHeroWinRating(_recordList, avaiableHeroes, _avaiableTribes, _console);
                }
                catch { }

                _mmrView.SetPeak(_peak);

                if (_rating > 0) _mmrView.SetMMR(_rating);
                else _mmrView.SetMMR(_ratingStart);
                bool letsGo = false;
                int waitTime = 6000;
               while (!letsGo && waitTime > 0) {
                    Thread.Sleep(100);
                    waitTime -= 100;
                    GetUnavaiableTribes();
                    if (_unavailableTribes != null) { 
                    string[] bannedtribes = _unavailableTribes.Split(',');
                    if (bannedtribes.Length == 4) {
                            letsGo = true;
                    _mmrView.ShowBannedTribes(bannedtribes);
                    GetAndSetBannedTribes();
                    }
                       
                    }
                }
            }
        }
        
        private static void GetAndSetBannedTribes()
        {
            if (!InBgMode()) return;
         
         
            if (!Core.Game.Spectator)
            {
                //if (!InBgMode()) return;
                //if (!InGameplayMode()) return;
                //var _db = new Lazy<BattlegroundsDb>();
                //_avaiableTribes = BattlegroundsUtils.GetAvailableRaces(Core.Game.CurrentGameStats?.GameId) ?? _db.Value.Races;
                //_unavailableRaces = string.Join(", ", _db.Value.Races.Where(x => !_avaiableTribes.Contains(x) && x != Race.INVALID && x != Race.ALL).Select(x => HearthDbConverter.RaceConverter(x)));

                if (_avaiableTribes.Count > 0)
                {


                    //IsMissingTribeReceived = false;
                    ////_tribeImgSize = _config.tribeSize;

                    //int waitTime = 6000;

                    try
                    {
                        //while (!IsMissingTribeReceived && waitTime > 0)
                        //{
                        //    Thread.Sleep(100);
                        //    waitTime -= 100;
                        //    IsMissingTribeReceived = SetMissingTribe();


                            //_tribes = new TribesOverlay();
                            var missingTribes = GetMissingTribeForInBattleUi();
                            if (missingTribes.Count() == 4)
                            {

                                if (_config.showTribeImages == true)
                                {
                                    if (!Core.OverlayCanvas.Children.Contains(_tribes))
                                    {
                                        Core.OverlayCanvas.Children.Add(_tribes);
                                        _tribes.ShowBannedTribes(missingTribes);
                                    }
                                }
                                else
                                {
                                    if (Core.OverlayCanvas.Children.Contains(_tribes))
                                    {
                                        Core.OverlayCanvas.Children.Remove(_tribes);
                                    }
                                }
                                //var avaiableHeroes = await SetPersonalHeroRating();
                                //_console.SetConsoleText("Getting Game Informations");
                            }
                        //}
                    }
                    catch { }
                }
            }
        }

        private static bool GetUnavaiableTribes()
        {
            var _db = new Lazy<BattlegroundsDb>();
            _avaiableTribes = BattlegroundsUtils.GetAvailableRaces(Core.Game.CurrentGameStats?.GameId) ?? _db.Value.Races;
            _unavailableTribes = string.Join(", ", _db.Value.Races.Where(x => !_avaiableTribes.Contains(x) && x != Race.INVALID && x != Race.ALL).Select(x => HearthDbConverter.RaceConverter(x)));
            if (_unavailableTribes != null) { return true; } else return false; ;
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
                        //_console.SetConsoleText("Turn #" + Core.Game.GetTurnNumber() + " started. You have " + " " + hero.Health + " HP left.");
                        _lastknownTurn = Core.Game.GetTurnNumber();
                    }
                }
                catch { }

                //if (_config.SkipAll)
                //{
                //    Task.Delay(3000);
                //    ToggleDisconect();
                //    Task.Delay(3000);
                //    ToggleDisconect();

                //}
            }
        }

        internal static void GameEnd()
        {

            if (!Core.Game.Spectator)
            {

                _lastRoundNr++;
                _mmrView.RemoveBannedTribes();
                if (Core.OverlayCanvas.Children.Contains(_tribes))
                {
                    Core.OverlayCanvas.Children.Remove(_tribes);
                    _tribes.RemoveBannedTribes();
                }

                int playerId = Core.Game.Player.Id;
                Entity hero = Core.Game.Entities.Values
                    .Where(x => x.IsHero && x.GetTag(GameTag.PLAYER_ID) == playerId)
                    .First();

                GetGameRecordData(hero);
                // es muss nur der gamerecord an die api geschickt werden der rest passiert automatisch, boonleage kann mich sich via HTTP GET holen.
                // eigentlich brauche ich hier keine gamerecords, die sind nur für die stat seite interesant und müssen hier nicht jedes mal geladen werden.
                //wenn man will kann man dann den platz der boonleague im overlay ausgeben

                if (!_config.DisconectedThisGame)
                {
                    lastRank = hero.GetTag(GameTag.PLAYER_LEADERBOARD_PLACE);
                    SetOverlayRanksAndStuff();
                }
                else
                {
                    OpenAddRankPrompt();
                }

                //_console.SetConsoleText("Game with the ID " + Core.Game.CurrentGameStats.GameId + " ended.");
            }

        }

        private static void SetOverlayRanksAndStuff()
        {
            if (lastRank > 0)
            {
                Ranks.SetRank(lastRank, _ranks);
                CalcAvgRank(_ranks);
                _overlay.SetRanksForOverlay(_ranks, _avgRank);
                _simpleOverlay.SetLastRank(lastRank);
                UpdateLeaderboardData();
                meanbob.meanBobLines(lastRank, _config);
                if (lastRank == 8) RankEight();
            }
        }


        public static void AddRankManualy(int rank)
        {
            lastRank = rank;
            SetOverlayRanksAndStuff();
        }


        internal static void RemoveRankManualy(int rank)
        {
            Ranks.RemoveRank(rank, _ranks);
            CalcAvgRank(_ranks);
            _overlay.SetRanksForOverlay(_ranks, _avgRank);
            _simpleOverlay.RemoveLastRank(lastRank);
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
        internal static List<BannedTribes> GetMissingTribeForInBattleUi()
        {
            if (_config.showTribeColors == true || _config.showTribeImages == true) { 
                if (BattlegroundsUtils.GetAvailableRaces(Core.Game.CurrentGameStats.GameId) != null)
                {
                    BannedTribes bannedTribes = new BannedTribes();
                    return bannedTribes.GetBannedTribesList(_unavailableTribes, _tribeAttributesList.tribeAttributes, _config);
                } 
            }
            return null;
        }

        //internal static bool SetMissingTribe()
        //{
        //    if (_unavailableTribes != null)
        //    {
        //        return true;
        //    }
        //    return false;
        //}


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
        //internal static void AddRemoveConsole()
        //{
        //    if (_config.showConsole)
        //    {
        //        if (!Core.OverlayCanvas.Children.Contains(_console))
        //        {
        //            Core.OverlayCanvas.Children.Add(_console);
        //        }
        //    }
        //    else
        //    {
        //        if (Core.OverlayCanvas.Children.Contains(_console))
        //        {
        //            Core.OverlayCanvas.Children.Remove(_console);
        //        }
        //    }
        //}



        internal static void AddRemoveDisconectButton()
        {
            if (_config.UseDisconect)
            {


                Window DisconectButtonWindow = new Window()
                {
                    Title = "Boomer Button",
                    Content = new InGameDisconectorOverlay(),
                    Height = 113.861,
                    Width = 211.646,

                    ResizeMode = ResizeMode.NoResize
                };
                if (!_config.DisconectWindowOpen)
                {
                    _config.DisconectWindowOpen = true;
                    _config.save();
                    InGameDisconectorOverlay.GetWindowName(DisconectButtonWindow);
                    DisconectButtonWindow.Show();
                }


            }

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
                //SetTribeImgSize();
                AddRemoveDisconectButton();

                //AddRemoveConsole();
                AddRemoveSimpleOverlay();
                AddRemoveLeaderboard();

                if (!InBgMenu()) return;
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
            return Disconnector.RuleSwitcher(_config);
        }

        internal static async void UpdateLeaderboardData()
        {
            if (_config.isLeaderboardActivated)
            {
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
                        if (_config.showSimpleOverlayBg) { 
                        _simpleOverlay.tbRanks.Background = (SolidColorBrush)new BrushConverter().ConvertFrom("#242424");
                        } else _simpleOverlay.tbRanks.Background = Brushes.Transparent;

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

        //internal static void SetTribeImgSize()
        //{
        //    if (_config.showTribeImages && _config.tribeSize != _tribeImgSize)
        //    {
        //        _tribes.SetTribeImageSize(_config.tribeSize);
        //        _tribeImgSize = _config.tribeSize;
        //    }
        //}

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

        internal static void OpenAddRankPrompt()
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
                    AddRankWindow.Topmost = true;
                    AddRankWindow.Show();
                    AddRankPrompt.GetWindowName(AddRankWindow);
                }
            }

        }

    }

}
