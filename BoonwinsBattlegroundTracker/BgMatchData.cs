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
        private static string _avgRank;
        private static SettingsControl _settings;
        private static bool isInBattle = false;
        private static int lastBattleTurn = 0;


        public static BgMatchOverlay Overlay;
        public static View View;

        internal static bool InBgMenu(string currentMethod)
        {
            if (Core.Game.CurrentMode != Hearthstone_Deck_Tracker.Enums.Hearthstone.Mode.BACON)
            {
             
                return false;
            }

            return true;
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
           
            Core.OverlayCanvas.Children.Remove(Overlay);
            View.SetAvgRank(_avgRank);
            View.SetMMR(_rating);
            View.SetisBannedGameStart();
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
                View.SetisBanned("Murlocs");
            }
            else if (total == 15)
            {
                View.SetisBanned("Demons");
            }
            else if (total == 17)
            {
                View.SetisBanned("Mechs");
            }
            else if (total == 20)
            {
                View.SetisBanned("Beasts");
            }
            else if (total == 23)
            {
                View.SetisBanned("Pirates");
            }
            else if (total == 24)
            {
                View.SetisBanned("Dragons");
            }
            else
            {
                View.SetisBanned("N/A");
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

            if (totalAmount != 0)
            {
                _avgRank = Math.Round((weightedAmount / totalAmount), MidpointRounding.AwayFromZero).ToString();
            }

        }


        internal static void InMenu()
        {
            Core.OverlayCanvas.Children.Add(Overlay);
            if (lastRank > 0)
            {
                SetRank(lastRank);
                CalcAvgRank(_ranks);
                Overlay.SetTextBoxValue(_ranks, _avgRank);
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
                Overlay.UpdateMmrChangeValue(mmrChange);
            }


            _rating = latestRating;
            _record.Rating = _rating;
            Overlay.UpdateMMR(latestRating);


        }


    }

}
