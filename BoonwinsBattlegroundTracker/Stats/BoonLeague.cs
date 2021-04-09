using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using Hearthstone_Deck_Tracker.Enums;

namespace BoonwinsBattlegroundTracker
{
    public class BoonLeague
    {
        public string Player { get; set; }
        public Region Region { get; set; }
        public int Season { get; set; }
        public int Points { get; set; }


        public static async Task<BoonLeague> GetData(HttpClient client)
        {

            using (HttpResponseMessage response = await client.GetAsync("api/BoonLeague"))
            {
                if (response.IsSuccessStatusCode)
                {
                    BoonLeague boon = await response.Content.ReadAsAsync<BoonLeague>();
                    return boon;
                } else
                {
                    return null;
                }
            }

        }

     

        public static ByteArrayContent MakeJsonObject(BoonLeague boon)
        {
            var json = JsonConvert.SerializeObject(boon);
            var buffer = System.Text.Encoding.UTF8.GetBytes(json);
            var byteContent = new ByteArrayContent(buffer);
            byteContent.Headers.ContentType = new MediaTypeHeaderValue("application/json");

            return byteContent;
          
        } 

        public static BoonLeague MakeBoonLeague(string json)
        {
            return JsonConvert.DeserializeObject<BoonLeague>(json);
        }
    }

    

}
