using System;
using Microsoft.ProjectOxford.Emotion.Contract;
using Microsoft.ProjectOxford.Vision.Contract;
using Newtonsoft.Json;

using Face = Microsoft.ProjectOxford.Face.Contract.Face;

namespace SociOLRoom.Analytics.Core
{
    public class SocialAnalytics
    {
        [JsonProperty(PropertyName = "id")]
        public string Id { get; set; }
        public ulong TweetID { get; set; }
        public SocialNetwork SocialNetwork { get; set; }

        public AnalysisResult AnalysisResult { get; set; }
        public string ImageUrl { get; set; }
        public DateTime CreatedAt { get; set; }
        public Emotion[] EmotionResult { get; set; }
        public Face[] FaceResult { get; set; }
        public string From { get; set; }
        public string Tags { get; set; }
        public long CreatedAtTimeStamp { get; set; }
    }
}