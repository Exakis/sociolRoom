using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Azure;
using Microsoft.Azure.WebJobs;
using Microsoft.ServiceBus.Messaging;
using SociOLRoom.Analytics.Core;

namespace SociOLRoom.Analytics.InstagramListener
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

            DateTime createdDateTime = DateTime.MinValue;
            while (true)
            {
                try
                {
                    var grams = await InstagramService.GetPhoto(Config.Tag);
                    grams = grams.OrderBy(c => c.CreatedAt).ToList();
                    List<BrokeredMessage> toSend = new List<BrokeredMessage>();
                    foreach (var gram in grams)
                    {
                        if (gram.CreatedAt > createdDateTime)
                        {
                            SocialAnalytics analytics = new SocialAnalytics
                            {
                                SocialNetwork = SocialNetwork.Instagram,
                                Id = gram.Id,
                                CreatedAt = gram.CreatedAt,
                                CreatedAtTimeStamp = gram.CreatedAt.Ticks,
                                From = gram.User,
                                Tags = gram.Tags,
                                ImageUrl = gram.ImageUrl
                            };
                            toSend.Add(new BrokeredMessage(analytics));
                            createdDateTime = gram.CreatedAt;
                        }
                    }
                    await client.SendBatchAsync(toSend);
                }
                catch (Exception ex)
                {
                    log.WriteLine(DateTime.UtcNow + " -- InstagramListener: " + ex.Message);
                }
                await Task.Delay(TimeSpan.FromMinutes(3));
            }
        }
    }
}
