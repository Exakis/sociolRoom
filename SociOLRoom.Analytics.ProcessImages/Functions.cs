using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Azure.Documents.Client;
using Microsoft.Azure.WebJobs;
using Microsoft.ProjectOxford.Emotion;
using Microsoft.ProjectOxford.Face;
using Microsoft.ProjectOxford.Vision;
using Microsoft.ServiceBus.Messaging;
using SociOLRoom.Analytics.Core;

namespace SociOLRoom.Analytics.ProcessImages
{
    public class Functions
    {
        public static async Task ProcessQueueMessage(TextWriter log, [ServiceBusTrigger("sociolroom", AccessRights.Listen)] SocialAnalytics socialAnalytics, [ServiceBus("sociolroom")] IAsyncCollector<SocialAnalytics> output)
        {
            var client = new DocumentClient(new Uri(Config.EndpointUri), Config.PrimaryKey);
            try
            {
                if (string.IsNullOrWhiteSpace(socialAnalytics.Id))
                {
                    log.WriteLine(DateTime.UtcNow + " -- ProcessImage: id is null");
                    return;
                }
                using (ImageProcessor processor = new ImageProcessor())
                {
                    var result = await processor.ProcessImage(socialAnalytics.ImageUrl);
                    socialAnalytics.AnalysisResult = result.VisionTask.Result;
                    socialAnalytics.EmotionResult = result.EmotionTask.Result;
                    socialAnalytics.FaceResult = result.FaceTask.Result;
                    await client.CreateDocumentIfNotExists(Config.DataBaseId, Config.DatabaseCollectionName, socialAnalytics, (t) => t.Id.ToString());
                }
            }
            catch (ClientException ex)
            {
                if (ex.Error.Code != "InvalidImageUrl")
                {
                    log.WriteLine(DateTime.UtcNow + " -- ProcessImage: " + ex.Message);
                    await output.AddAsync(socialAnalytics);
                }
            }
            catch (Exception ex)
            {
                log.WriteLine(DateTime.UtcNow + " -- ProcessImage: " + ex.Message);
                await output.AddAsync(socialAnalytics);
            }
            await Task.Delay(TimeSpan.FromSeconds(30));
        }
    }
}
