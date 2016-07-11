using System;
using System.Threading.Tasks;
using Microsoft.ProjectOxford.Emotion;
using Microsoft.ProjectOxford.Face;
using Microsoft.ProjectOxford.Vision;
using Microsoft.ProjectOxford.Vision.Contract;

namespace SociOLRoom.Analytics.Core
{
    public class ImageProcessor : IDisposable
    {
        private readonly VisionServiceClient _visionServiceClient;
        private readonly EmotionServiceClient _emotionServiceClient;
        private readonly FaceServiceClient _faceServiceClient;

        public ImageProcessor()
        {
            _visionServiceClient = new VisionServiceClient(Config.Cognitive.VisionAPIKey);
            _emotionServiceClient = new EmotionServiceClient(Config.Cognitive.EmotionAPIKey);
            _faceServiceClient = new FaceServiceClient(Config.Cognitive.FaceAPIKey);
        }

        public async Task<CogniviteResult> ProcessImage(string url)
        {
            VisualFeature[] visualFeatures = {
                VisualFeature.Adult, VisualFeature.Categories, VisualFeature.Color, VisualFeature.Description, VisualFeature.Faces,
                VisualFeature.ImageType, VisualFeature.Tags
            };

            CogniviteResult result = new CogniviteResult();

            result.VisionTask = _visionServiceClient.AnalyzeImageAsync(url, visualFeatures);
            result.EmotionTask = _emotionServiceClient.RecognizeAsync(url);
            result.FaceTask = _faceServiceClient.DetectAsync(url, false, true,new[] {FaceAttributeType.Gender, FaceAttributeType.Age, FaceAttributeType.Smile, FaceAttributeType.Glasses});

            await Task.WhenAll(result.VisionTask, result.EmotionTask, result.FaceTask);

            var enTxt = result.VisionTask.Result.Description.Captions[0].Text;
            var frTxt = await TranslatorService.Translate(enTxt, "en", "fr");
            var esTxt = await TranslatorService.Translate(enTxt, "en", "es");

            result.VisionTask.Result.Description.Captions = new[] { new Caption { Text = frTxt }, result.VisionTask.Result.Description.Captions[0], new Caption { Text = esTxt }, };

            return result;
        }

        public void Dispose()
        {
            if (_faceServiceClient != null)
                _faceServiceClient.Dispose();
        }
    }
}