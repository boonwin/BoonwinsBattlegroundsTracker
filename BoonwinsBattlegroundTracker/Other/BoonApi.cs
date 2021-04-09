using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace BoonwinsBattlegroundTracker.Other
{
    internal class BoonApi
    {
       public static async void SendBoon(ByteArrayContent boon, HttpClient client)
        {
            using (HttpResponseMessage response = await client.GetAsync("api/BoonLeague"))
            {
                if (response.IsSuccessStatusCode)
                {
                    await client.PostAsync("api/BoonLeague", boon);

                }
            }

        }

        public static async void SendGameRecord(ByteArrayContent record, HttpClient client)
        {
            using (HttpResponseMessage response = await client.GetAsync("api/GameRecord"))
            {
                if (response.IsSuccessStatusCode)
                {
                    await client.PostAsync("api/GameRecord", record);

                }
            }

        }




    }
}
