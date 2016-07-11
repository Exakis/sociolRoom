using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using Newtonsoft.Json.Linq;

namespace SociOLRoom.Analytics.Core
{
    public class InstagramService
    {
        public static async Task<List<Instagram>> GetPhoto(string tag)
        {
            List<Instagram> instagrams = new List<Instagram>();
            using (HttpClient client = new HttpClient())
            {
                var json = await client.GetStringAsync("https://api.instagram.com/v1/tags/" + tag + "/media/recent?access_token=2070635583.5b9e1e6.7c079693a6c94fcdbcde3437252b7efe");

                dynamic jsonObject = JObject.Parse(json);

                JArray dataArray = jsonObject.data as JArray;
                foreach (dynamic data in dataArray)
                {
                    if (data.type == "image")
                    {
                        Instagram instagram = new Instagram();
                        instagram.ImageUrl = data.images.standard_resolution.url;
                        instagram.Id = data.id;
                        instagram.Tags = String.Join(", ", ((JArray)data.tags).Select(s => "#" + s));

                        DateTime dt = new DateTime(1970, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc);
                        dt = dt.AddSeconds(int.Parse((string)data.created_time)).ToLocalTime();

                        instagram.CreatedAt = dt;
                        instagram.User = data.user.username;
                        instagrams.Add(instagram);
                    }
                }

            }
            return instagrams;
        }
    }
}