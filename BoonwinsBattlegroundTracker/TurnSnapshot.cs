using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BoonwinsBattlegroundTracker
{
    public class TurnSnapshot
    {
        public string Minions;
        public string Hero;
        public int Turn;
        public DateTimeOffset dateTime;
        public string isSelf = "Yes";
        public string result = "Draw";
        public string GameID;
        public string player;

        public List<object> ToList(bool useDateTimeString)
        {
            return (List<object>)ToArgList().Select((key, index) =>
            {
                if (useDateTimeString && key.Item1 == "dateTime") return DateTimeToString();
                else return key.Item2;
            }).ToList();
        }

        public List<string> Headers()
        {
            return (List<string>)ToArgList().Select((key, val) => key.Item1).ToList();
        }

        public List<(string, object)> ToArgList()
        {
            return new List<(string, object)>
            {
                ("dateTime",   dateTime),
                ("hero", Hero),
                ("minions", Minions),
                ("turn", Turn),
                ("isSelf", isSelf),
                ("combatResult", result),
                ("gameId", GameID),
                ("player", player)
            };
        }

        public string DateTimeToString()
        {
            return dateTime.ToString("yyyy-MM-dd HHmm");
        }

    }
}
