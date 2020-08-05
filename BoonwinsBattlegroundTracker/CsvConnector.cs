using System;
using System.Linq;
using System.IO;

namespace BoonwinsBattlegroundTracker
{
    public class CsvConnector
    {
        private static Config _config;

        internal static string AddQuotes(string str)
        {
            return "\"" + str + "\"";
        }
        public static void Initialize(Config config)
        {
            _config = config;

        }

      

       
    }
}
