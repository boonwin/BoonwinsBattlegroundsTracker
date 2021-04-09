using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
namespace Leaderboard
{
    public class Leaderboard
    {
        public string AccountId { get; set; }
        public string Rank { get; set; }
        public string Rating { get; set; }

        public static string GetLeaderboard(string region, string nickname)
        {

            var adress = @"https://playhearthstone.com/en-us/api/community/leaderboardsData?region=" + region + "&leaderboardId=BG";
            WebClient client = new WebClient();
            var strPAgeCode = client.DownloadString(adress);

            dynamic dobj = JsonConvert.DeserializeObject<dynamic>(strPAgeCode);
            List<Leaderboard> leaderboardList = new List<Leaderboard>();
            Leaderboard leaderboard;

            for (int i = 0; i < 200; i++)
            {
                leaderboard = new Leaderboard();
                leaderboard.AccountId = dobj["leaderboard"]["rows"][i]["accountid"];
                leaderboard.Rank = dobj["leaderboard"]["rows"][i]["rank"];
                leaderboard.Rating = dobj["leaderboard"]["rows"][i]["rating"];
                leaderboardList.Add(leaderboard);
            }

            return leaderboardList.Where(d => d.AccountId.Contains(nickname)).Select(s => s.Rank).FirstOrDefault();


        }
    }
}
