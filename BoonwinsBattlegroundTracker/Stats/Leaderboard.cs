using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace BoonwinsBattlegroundTracker
{
    public class Leaderboard
    {
        public string AccountId { get; set; }
        public int Rank { get; set; }
        public int Rating { get; set; }

        public static async Task<(string, int)> GetLeaderboard(string region, string nickname)
        {

            var adress = @"https://playhearthstone.com/en-us/api/community/leaderboardsData?region=" + region + "&leaderboardId=BG";
            string leaderboardRank = null;
            int leaderboardRating = 0;
            await Task.Run(() =>
            {
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

                
                leaderboardRank = leaderboardList.Where(d => d.AccountId.Contains(nickname)).Select(s => s.Rank).FirstOrDefault().ToString();
                Debug.WriteLine(leaderboardRank);
                if (leaderboardRank == "0") {
                    leaderboardRank = "n/a";                    
                    leaderboardRating = leaderboardList.Where(r => r.Rank == 200).Select(s => s.Rating).FirstOrDefault();
                }
                else {
                    if (Int32.Parse(leaderboardRank) != 1)
                    {
                        var leaderboardRankAbove = Int32.Parse(leaderboardRank) - 1;
                        leaderboardRating = leaderboardList.Where(r => r.Rank == leaderboardRankAbove).Select(s => s.Rating).FirstOrDefault();
                    }
                    else leaderboardRating = 0;
                }


            });
            return (leaderboardRank, leaderboardRating);
        }



    }


    
}
