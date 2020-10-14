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
        private static string _avgRank;
        private static int _tribeImgSize;
        public static bool IsMissingTribeRetrieved;
        public static bool IsMissingHeroesRetrieved;

        public static BgMatchOverlay _overlay;
        public static View _view;
        public static TribesOverlay _tribes;
        public static ConsoleOverlay _console;
        public static GameHistoryOverlay _history;

        public static OverlayManager _input;
        public static TriverOverlayManager _tribeInput;

        public static HashSet<Race> _avaiableTribes;
        public static string[] _avaiableHeroes;
        internal static int _lastknownTurn = 0;

        internal static void TurnStart(ActivePlayer player)
        {
            if (!InBgMode()) return;

            Entity hero = Core.Game.Entities.Values
                .Where(x => x.IsHero && x.GetTag(GameTag.PLAYER_ID) == Core.Game.Player.Id)
                .First();

            if (_lastknownTurn != Core.Game.GetTurnNumber() && _config.showTurns)
            {
                _console.SetConsoleText("Turn #" + Core.Game.GetTurnNumber() + " started. You have " + " " + hero.Health + " HP left.");
                _lastknownTurn = Core.Game.GetTurnNumber();
            }

        }

        public static void OnLoad(Config config)
        {
            Log.Info($"onLoad - reading config, ceating ranks and gamerecord object");
            _config = config;
            _ranks = new Ranks();
            _recordList = GameRecord.LoadGameRecordFromFile(_config._gameRecordPath);
            lastRank = 0;

        }

        internal static async void GameStart()
        {
            _console.SetConsoleText("Game with the ID " + Core.Game.CurrentGameStats.GameId + " started.");

            IsMissingTribeRetrieved = false;
            _tribeImgSize = _config.tribeSize;
            _view.SetAvgRank(_avgRank);
            _view.SetMMR(_rating);

            int waitTime = 30000;

            while (!IsMissingTribeRetrieved && waitTime > 0)
            {
                Thread.Sleep(3000);
                waitTime -= 3000;
                IsMissingTribeRetrieved = SetMissingRace();

            }

            var avaiableHeroes = await SetPersonalHeroRating();

            _console.SetConsoleText("Getting Game Informations");

            GameRecord.GetHeroWinRating(_recordList, avaiableHeroes, _avaiableTribes, _console);
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


        internal static void GameEnd()
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
            GameRecord.WriteGameRecordToFile(_config._gameRecordPath, _record);
            lastRank = hero.GetTag(GameTag.PLAYER_LEADERBOARD_PLACE);

            if (lastRank > 0)
            {
                SetRank(lastRank);
                CalcAvgRank(_ranks);
                _overlay.SetTextBoxValue(_ranks, _avgRank);
            }
            _console.SetConsoleText("Game with the ID " + Core.Game.CurrentGameStats.GameId + " ended.");
        }

        private static void GetGameRecordData(Entity hero)
        {
            _record = new GameRecord();
            _record.DateTime = DateTime.Now;
            _record.Position = hero.GetTag(GameTag.PLAYER_LEADERBOARD_PLACE);
            _record.Hero = hero.Card.LocalizedName;
            _record.Tribes = _avaiableTribes;
            _record.GameID = Core.Game.CurrentGameStats.GameId;
            _record.Player = Core.Game.Player.Name;
            _recordList.Add(_record);

        }

        //Most of this should be in another class KEKL
        internal static bool SetMissingRace()
        {

            var gameID = Core.Game.CurrentGameStats.GameId;

            _avaiableTribes = Tribes.GetTribes(gameID, _view, _config, _tribes);

            if (_avaiableTribes != null)
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

        internal static void AddRemoveGameHistory()
        {
            if (_config.showHistory)
            {
                if (!Core.OverlayCanvas.Children.Contains(_history))
                {
                    Core.OverlayCanvas.Children.Add(_history);
                }
            }
            else
            {
                if (Core.OverlayCanvas.Children.Contains(_history))
                {
                    Core.OverlayCanvas.Children.Remove(_history);
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
            // rating is only updated after we have passed the menu
            AddOrRemoveOverlay();
            SetTribeImgSize();
            AddRemoveConsole();
            AddRemoveGameHistory();



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
            _overlay.UpdateMMR(latestRating);

        }


    }

}
