using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BoonwinsBattlegroundTracker
{
    public class GameRecord
    {
        public List<int> TavernTierTimings = new List<int>();
        public int CurrentTavernTier = 1;
        public TurnSnapshot Snapshot = new TurnSnapshot();
        public List<TurnSnapshot> Histories = new List<TurnSnapshot>();
        public int Position;

        public DateTimeOffset DateTime { get => Snapshot.dateTime; set => Snapshot.dateTime = value; }
        public string player { get => Snapshot.player; set => Snapshot.player = value; }
        public int Rating;

        public static string ListToString(List<(string, object)> list)
        {
            for (int i = 0; i < list.Count; i++)
            {
                var (key, val) = list[i];

                if (val is DateTimeOffset) val = val.ToString();

                if (val is string) list[i] = (key, $"\"{val}\"");

            }

            string result = String.Join(", ", list.Select(item => $"{item.Item1}:{item.Item2}"));
            return result;

        }
        public static void WriteGameRecord(GameRecord record)
        {

            List<(string, object)> recordList = new List<(string, object)>
            {
                ("tavernTimings", String.Join(",", record.PaddedTavernTimings())),
                ("position", record.Position),
                ("endTurn", record.Snapshot.Turn),
                ("mmr", record.Rating),
                ("hero", record.Snapshot.Hero),
                ("gameId", record.Snapshot.GameID),
                ("dateTime", record.DateTime),
                ("player", record.player),
            };
            string recordString = ListToString(recordList);



        }


        public List<string> Headers = new List<string> {
                "Date & Time","Hero","Position","MMR","Ending Minions", "Turns taken to reach tavern tier 2","3","4","5","6", "Ending Turn", "Game ID", "Player"
            };

        public List<string> PaddedTavernTimings()
        {
            List<string> list = TavernTierTimings.ConvertAll(x => x.ToString());
            while (list.Count < 5)
            {
                list.Add("");
            }
            return list;
        }

        public List<object> ToList(bool useDateTimeString)
        {

            object dt = DateTime;
            if (useDateTimeString) dt = Snapshot.DateTimeToString();

            List<object> l = new List<object>
            {
                dt, Snapshot.Hero, Position, Rating, Snapshot.Minions
            };

            foreach (string turn in PaddedTavernTimings())
            {
                l.Add(turn);
            }

            l.Add(Snapshot.Turn);
            l.Add(Snapshot.GameID);
            l.Add(Snapshot.player);

            return l;
        }

    }
}
