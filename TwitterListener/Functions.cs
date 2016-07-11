using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LinqToTwitter;
using Microsoft.Azure;
using Microsoft.Azure.Documents.Client;
using Microsoft.Azure.WebJobs;
using Microsoft.ServiceBus;
using Microsoft.ServiceBus.Messaging;
using SociOLRoom.Analytics.Core;

namespace TwitterListener
{
    public class Functions
    {
        // This function will get triggered/executed when a new message is written 
        // on an Azure Queue called queue.
        [NoAutomaticTriggerAttribute]
        public static async Task ProcessMethod(TextWriter log)
        {
            string connectionString = CloudConfigurationManager.GetSetting("Microsoft.ServiceBus.ConnectionString");

            QueueClient client = QueueClient.CreateFromConnectionString(connectionString, "sociolroom");

            while (true)
            {
                try
                {
                    TwitterService service = new TwitterService();
                    await service.Authenticate();
                    service.OnNewMessage += async (s, e) =>
                    {
                        SocialAnalytics analytics = new SocialAnalytics
                        {
                            TweetID = e.Data.StatusID,
                            SocialNetwork = SocialNetwork.Twitter,
                            Id = e.Data.StatusID.ToString(),
                            CreatedAt = e.Data.CreatedAt,
                            CreatedAtTimeStamp = e.Data.CreatedAt.Ticks,
                            From = e.Data.User.Name,
                            Tags = string.Join(", ", e.Data.Entities.HashTagEntities.Select(t => "#" + t.Tag)),
                            ImageUrl = e.Data.Entities.MediaEntities[0].MediaUrl + ":large"
                        };

                        await client.SendAsync(new BrokeredMessage(analytics));
                    };
                    await service.GetStreamTweets(Config.Tag);
                }
                catch (Exception ex)
                {
                    log.WriteLine(DateTime.UtcNow + " -- TwitterListener: "+ex.Message);
                }
                await Task.Delay(TimeSpan.FromSeconds(5));
            }
        }
    }
}
