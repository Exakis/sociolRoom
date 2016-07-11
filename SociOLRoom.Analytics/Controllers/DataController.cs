using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Http;
using Microsoft.Azure.Documents.Client;
using SociOLRoom.Analytics.Core;
using SociOLRoom.Analytics.Models;

namespace SociOLRoom.Analytics.Controllers
{
    public class DataController : ApiController
    {
       
        private readonly DocumentClient _client;
        
        public DataController()
        {
            this._client = new DocumentClient(new Uri(Config.EndpointUri), Config.PrimaryKey);
         
        }

        [HttpGet]
        public Statistics Statistics()
        {
            Statistics statistics = new Statistics();

            statistics.Men = this._client.CreateDocumentQuery<SocialAnalytics>(
                UriFactory.CreateDocumentCollectionUri(Config.DataBaseId, Config.DatabaseCollectionName))
                .SelectMany(s => s.FaceResult).Where(s => s.FaceAttributes.Gender == "male").ToList().Count;

            statistics.Women = this._client.CreateDocumentQuery<SocialAnalytics>(
                UriFactory.CreateDocumentCollectionUri(Config.DataBaseId, Config.DatabaseCollectionName))
                .SelectMany(s => s.FaceResult).Where(s => s.FaceAttributes.Gender == "female").ToList().Count;

            var ages = this._client.CreateDocumentQuery<SocialAnalytics>(
                UriFactory.CreateDocumentCollectionUri(Config.DataBaseId, Config.DatabaseCollectionName))
                .SelectMany(s => s.FaceResult).Select(s => s.FaceAttributes.Age).ToList();

            statistics.Children = ages.Count(a => a < 18);

            statistics.Age = (int)ages.Average();


            var lastItem = this._client.CreateDocumentQuery<SocialAnalytics>(
              UriFactory.CreateDocumentCollectionUri(Config.DataBaseId, Config.DatabaseCollectionName))
              .OrderByDescending(c => c.CreatedAtTimeStamp).Take(1).ToList();

            statistics.From = lastItem.FirstOrDefault()?.CreatedAt;

            var emotions = this._client.CreateDocumentQuery<SocialAnalytics>(
              UriFactory.CreateDocumentCollectionUri(Config.DataBaseId, Config.DatabaseCollectionName))
              .SelectMany(s => s.EmotionResult).Select(s => s.Scores).ToList();

            var emotionStat = new[]
            {
                new {Name = "Anger", Value = emotions.Sum(t => t.Anger)},
                new {Name = "Contempt", Value = emotions.Sum(t => t.Contempt)},
                new {Name = "Disgust", Value = emotions.Sum(t => t.Disgust)},
                new {Name = "Fear", Value = emotions.Sum(t => t.Fear)},
                new {Name = "Happiness", Value = emotions.Sum(t => t.Happiness)},
                new {Name = "Neutral", Value = emotions.Sum(t => t.Neutral)},
                new {Name = "Sadness", Value = emotions.Sum(t => t.Sadness)},
                new {Name = "Surprise", Value = emotions.Sum(t => t.Surprise)}
            };

            var topEmotion = emotionStat.OrderByDescending(e => e.Value).FirstOrDefault();

            if (topEmotion != null)
                statistics.Emotion = new NamePercent { Name = topEmotion.Name, Percent = (int)Math.Round(topEmotion.Value / emotionStat.Sum(t => t.Value) * 100) };

            var categories = this._client.CreateDocumentQuery<SocialAnalytics>(
             UriFactory.CreateDocumentCollectionUri(Config.DataBaseId, Config.DatabaseCollectionName))
             .SelectMany(s => s.AnalysisResult.Categories).ToList();

            var predicateCat = categories.GroupBy(s => s.Name).Select(s => new { Name = s.Key, Value = s.Sum(g => g.Score) });

            var cat = predicateCat.OrderByDescending(s => s.Value).FirstOrDefault();

            statistics.Category = new NamePercent
            {
                Name = cat.Name,
                Percent = (int)Math.Round(cat.Value / predicateCat.Sum(s => s.Value) * 100)
            };


            return statistics;
        }


        [HttpGet]
        public List<SocialAnalytics> Wall()
        {
            FeedOptions queryOptions = new FeedOptions { MaxItemCount = -1 };
          
            var items = this._client.CreateDocumentQuery<SocialAnalytics>(
                UriFactory.CreateDocumentCollectionUri(Config.DataBaseId, Config.DatabaseCollectionName), queryOptions)
                .OrderByDescending(c => c.CreatedAtTimeStamp).Take(52).ToList();

            foreach (var item in items)
            {
                var culture = HttpContext.Current.Request.UserLanguages[0];

                if (culture.StartsWith("en", StringComparison.InvariantCultureIgnoreCase))
                    item.AnalysisResult.Description.Captions[0] = item.AnalysisResult.Description.Captions[1];
                else if (culture.StartsWith("fr", StringComparison.InvariantCultureIgnoreCase))
                    item.AnalysisResult.Description.Captions[0] = item.AnalysisResult.Description.Captions[0];
                else if (culture.StartsWith("es", StringComparison.InvariantCultureIgnoreCase))
                    item.AnalysisResult.Description.Captions[0] = item.AnalysisResult.Description.Captions[2];
            }

            return items;
        }
    }
}
