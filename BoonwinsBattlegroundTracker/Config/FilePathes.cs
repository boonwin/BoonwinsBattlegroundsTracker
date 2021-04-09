using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Hearthstone_Deck_Tracker;

namespace BoonwinsBattlegroundTracker
{
    
        public class FilePathes
        {

            public string writePath = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData) + @"\boonwin\data\";
            public string gameResultPath = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData) + @"\boonwin\data\gameresults.dat";
            public string underlordsGameResultPath = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData) + @"\boonwin\data\ungameresults.dat";
            public string skinConfigPath = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData) + @"\boonwin\data\skinConfig.dat";
            public string mmrPath = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData) + @"\boonwin\data\mmr.dat";
            public string changeLogPath = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData) + @"\boonwin\data\changeLogPath.dat";
           

    }
    
}
