using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Xml.Linq;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace SociOLRoom.Analytics.Core
{
    public class TranslatorService
    {
        public class AdmAccessToken
        {
            public string access_token { get; set; }
            public string token_type { get; set; }
            public string expires_in { get; set; }
            public string scope { get; set; }
        }

        public static async Task<string> Translate(string str, string from, string to)
        {
            var bearer = await Auth();


            string uri = "http://api.microsofttranslator.com/v2/Http.svc/Translate?text=" + System.Web.HttpUtility.UrlEncode(str) + "&from=" + from + "&to=" + to;

            using (HttpClient client = new HttpClient())
            {
                client.DefaultRequestHeaders.TryAddWithoutValidation("Authorization", bearer);
                var xml = await client.GetStringAsync(uri);
                return XDocument.Parse(xml).Root.Value;
            }
        }


        public static async Task<string> Auth()
        {
            string clientID = Config.Bing.TranslatorClientID;
            string clientSecret = Config.Bing.TranslatorClientSecret;

            using (var httpClient = new HttpClient())
            {
                var response = await httpClient.PostAsync("https://datamarket.accesscontrol.windows.net/v2/OAuth2-13", new FormUrlEncodedContent(
                    new KeyValuePair<string, string>[] {
                        new KeyValuePair<string,string>("grant_type", "client_credentials"),
                        new KeyValuePair<string,string>("client_id", clientID),
                        new KeyValuePair<string,string>("client_secret", clientSecret),
                        new KeyValuePair<string,string>("scope", "http://api.microsofttranslator.com"),
                    }));

                response.EnsureSuccessStatusCode();

                var json = await response.Content.ReadAsStringAsync();

                dynamic admAccessToken = JObject.Parse(json);

                return "Bearer " + admAccessToken.access_token;
            }
        }
    }
}