using System.Threading.Tasks;
using Microsoft.ProjectOxford.Emotion.Contract;
using Microsoft.ProjectOxford.Vision.Contract;
using Face = Microsoft.ProjectOxford.Face.Contract.Face;

namespace SociOLRoom.Analytics.Core
{
    public class CogniviteResult
    {
        public Task<AnalysisResult> VisionTask { get; set; }
        public Task<Emotion[]> EmotionTask { get; set; }
        public Task<Face[]> FaceTask { get; set; }
    }
}