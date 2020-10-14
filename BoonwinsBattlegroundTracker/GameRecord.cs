using HearthDb.Enums;
using Hearthstone_Deck_Tracker.Hearthstone.Entities;
using MahApps.Metro.Controls;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Remoting.Messaging;
using System.Text;
using System.Threading.Tasks;

namespace BoonwinsBattlegroundTracker
{
    public class GameRecord
    {
        public DateTime DateTime { get; set; }
        public int Position { get; set; }
        public string Hero { get; set; }
        public HashSet<Race> Tribes { get; set; }
        public Guid GameID { get; set; }
        public string Player { get; set; }

  

        //public List<string> Headers = new List<string> {
        //        "Date & Time","Position","MMR" ,"Hero", "Banned Tribes", "Ending Minions", "Ending Turn", "Game ID", "Player"
        //    };


        public static List<GameRecord> LoadGameRecordFromFile(string gameHistoryFile)
        {
            List<GameRecord> gameRecords = new List<GameRecord>();
            if (File.Exists(gameHistoryFile))
            {
                using (StreamReader streamReader = File.OpenText(gameHistoryFile))
                {
                    string dataLine;
                    while ((dataLine = streamReader.ReadLine()) != null)
                    {
                        var singleGameRecord = new GameRecord();
                        singleGameRecord = JsonConvert.DeserializeObject<GameRecord>(dataLine);
                        gameRecords.Add(singleGameRecord);
                        
                    }

                }
            }

            return gameRecords;

        }


        private static List<string> GetDataFromString(string dataLine)
        {
            List<string> outputList = new List<string>();
            var data = dataLine.Split(',');
            foreach (var type in data)
            {
                outputList.Add(type);
            }

            return outputList;
        }


        public static void WriteGameRecordToFile(string gameHistoryFile, GameRecord record)
        {
            if (!File.Exists(gameHistoryFile))
            {
                File.Create(gameHistoryFile);
            }

            string output = JsonConvert.SerializeObject(record);

            using (StreamWriter sw = File.AppendText(gameHistoryFile))
            {
                sw.WriteLine(output);
            }

        }


        public static void GetHeroWinRating(List<GameRecord> recordList, string[] avaiableHeros, HashSet<Race> avaiableTribes, ConsoleOverlay _console)
        {

            int Hero1Pos = 0, Hero2Pos = 0, Hero3Pos = 0, Hero4Pos = 0;
            int Hero1Count = 0, Hero2Count = 0, Hero3Count = 0, Hero4Count = 0;

            _console.SetConsoleText("Tribes found: " + String.Join(",", avaiableTribes));

            List<GameRecord> gamesWithAvaiableHero = new List<GameRecord>();

            if (avaiableHeros.Length == 2)
            {
                int avgPosHero1 = 0;
                int avgPosHero2 = 0;

                foreach (var result in recordList.Where(hero1 => hero1.Hero == avaiableHeros[0]))
                {

                    Hero1Count++;
                    Hero1Pos = Hero1Pos + result.Position;
                    _console.SetConsoleText("You played " + avaiableHeros[0] + " " + Hero1Count + " times.");
                }
                if (Hero1Count >= 1)
                {
                    avgPosHero1 = Hero1Pos / Hero1Count;
                    _console.SetConsoleText("With the avg.: " + avgPosHero1);
                }
                else _console.SetConsoleText("No data for: " + avaiableHeros[0]);
                _console.SetConsoleText("Hero #2: " + avaiableHeros[1]);
                foreach (var result in recordList.Where(hero2 => hero2.Hero == avaiableHeros[1]))
                {

                    Hero2Count++;
                    Hero2Pos = Hero2Pos + result.Position;
                    _console.SetConsoleText("You played " + avaiableHeros[1] + " " + Hero2Count + " times.");
                }
                if (Hero2Count >= 1)
                {
                    avgPosHero2 = Hero2Pos / Hero2Count;
                    _console.SetConsoleText("With the avg.: " + avgPosHero2);
                }
                else _console.SetConsoleText("No data for: " + avaiableHeros[1]);

            }
            if (avaiableHeros.Length == 4)
            {
                int avgPosHero1 = 0;
                int avgPosHero2 = 0;
                int avgPosHero3 = 0;
                int avgPosHero4 = 0;

                foreach (var result in recordList.Where(hero1 => hero1.Hero == avaiableHeros[0]))
                {               
                    Hero1Count++;
                    Hero1Pos = Hero1Pos + result.Position;
               
                }
                if (Hero1Count >= 1)
                {
                    avgPosHero1 = Hero1Pos / Hero1Count;
                    _console.SetConsoleText("You played " + avaiableHeros[0] + " " + Hero1Count + " times.");
                    _console.SetConsoleText("With the avg.: " + avgPosHero1);                  
                } else _console.SetConsoleText("No data for: " + avaiableHeros[0]);

                foreach (var result in recordList.Where(hero2 => hero2.Hero == avaiableHeros[1]))
                {
                    Hero2Count++;
                    Hero2Pos = Hero2Pos + result.Position;
                   
                }
                if (Hero2Count >= 1)
                {
                    avgPosHero2 = Hero2Pos / Hero2Count;
                    _console.SetConsoleText("You played " + avaiableHeros[1] + " " + Hero2Count + " times.");
                    _console.SetConsoleText("With the avg.: " + avgPosHero2);
                }
                else _console.SetConsoleText("No data for: " + avaiableHeros[1]);
                foreach (var result in recordList.Where(hero3 => hero3.Hero == avaiableHeros[2]))
                {
                    Hero3Count++;
                    Hero3Pos = Hero3Pos + result.Position;
                   
                }
                if (Hero3Count >= 1)
                {
                    avgPosHero3 = Hero3Pos / Hero3Count;
                    _console.SetConsoleText("You played " + avaiableHeros[2] + " " + Hero3Count + " times.");
                    _console.SetConsoleText("With the avg.: " + avgPosHero3);
                }
                else _console.SetConsoleText("No data for: " + avaiableHeros[2]);
                foreach (var result in recordList.Where(hero4 => hero4.Hero == avaiableHeros[3]))
                {
                    Hero4Count++;
                    Hero4Pos = Hero4Pos + result.Position;
                    
                }                        
                if (Hero4Count >= 1)
                {
                    avgPosHero4 = Hero4Pos / Hero4Count;
                    _console.SetConsoleText("You played " + avaiableHeros[3] + " " + Hero4Count + " times.");
                    _console.SetConsoleText("With the avg.: " + avgPosHero4);
                }
                else _console.SetConsoleText("No data for: " + avaiableHeros[3]);
            }
        

        }
    }


}
