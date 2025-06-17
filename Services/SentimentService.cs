using Microsoft.ML;
using ForumSentiment.Models;

namespace ForumSentiment.Services
{
    public class SentimentService
    {
        private readonly PredictionEngine<CommentData, CommentPrediction> _predictionEngine;

        public SentimentService()
        {
            var mlContext = new MLContext();
            ITransformer mlModel = mlContext.Model.Load("SentimentModel.zip", out _);
            Console.WriteLine("ML model loaded successfully.");
            _predictionEngine = mlContext.Model.CreatePredictionEngine<CommentData, CommentPrediction>(mlModel);
        }

        public bool IsOffensive(string commentContent)
        {
            var prediction = _predictionEngine.Predict(new CommentData { SentimentText = commentContent });
            Console.WriteLine($"Text: {commentContent} -> Prediction: {prediction.PredictedLabel}");
            return prediction.PredictedLabel == true;
        }
    }

    public class CommentData { public string SentimentText { get; set; } }
    public class CommentPrediction
{
    public bool PredictedLabel { get; set; }
    public float Score { get; set; } // беше float[] -> трябва да е float
}
}
