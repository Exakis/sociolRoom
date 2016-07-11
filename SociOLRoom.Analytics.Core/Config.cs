using Microsoft.Azure;

namespace SociOLRoom.Analytics.Core
{
    public class Config
    {
        public const string Tag = "parcol";
        public static string EndpointUri = CloudConfigurationManager.GetSetting("DocumentDBUrl");
        public static string PrimaryKey = CloudConfigurationManager.GetSetting("DocumentDBPrimaryKey");

        public const string DatabaseCollectionName = "SocialAnalyticsV3";

        public const string DataBaseId = "SociOLRoom";

        public class Cognitive
        {
            public static string VisionAPIKey = CloudConfigurationManager.GetSetting("Cognitive:VisionAPIKey");
            public static string EmotionAPIKey = CloudConfigurationManager.GetSetting("Cognitive:EmotionAPIKey");
            public static string FaceAPIKey = CloudConfigurationManager.GetSetting("Cognitive:FaceAPIKey");
        }

        public class Bing
        {
            public static string TranslatorClientID = CloudConfigurationManager.GetSetting("Bing:TranslatorClientID");
            public static string TranslatorClientSecret = CloudConfigurationManager.GetSetting("Bing:TranslatorClientSecret");
        }

        public class Twitter
        {
            public static string ConsumerKey = CloudConfigurationManager.GetSetting("Twitter:ConsumerKey");
            public static string ConsumerSecret = CloudConfigurationManager.GetSetting("Twitter:ConsumerSecret");
            public static string AccessToken = CloudConfigurationManager.GetSetting("Twitter:AccessToken");
            public static string AccessTokenSecret = CloudConfigurationManager.GetSetting("Twitter:AccessTokenSecret");
        }
    }
}