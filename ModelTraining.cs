using Microsoft.ML;
using Microsoft.ML.Data;

public class InputData
{
    [LoadColumn(1)]
    public string SentimentText = string.Empty;

    [LoadColumn(0)]
    public bool Sentiment;
}

public class ModelOutput
{
    public bool PredictedLabel { get; set; }
    public float Score { get; set; }
}

public class Trainer
{
    public static void Train(string dataPath, string modelPath)
    {
        var mlContext = new MLContext();

        var data = mlContext.Data.LoadFromTextFile<InputData>(
            path: dataPath, hasHeader: true, separatorChar: '\t');

        var trainTestSplit = mlContext.Data.TrainTestSplit(data, testFraction: 0.2);

        var pipeline = mlContext.Transforms.Text.FeaturizeText("Features", nameof(InputData.SentimentText))
            .Append(mlContext.BinaryClassification.Trainers.SdcaLogisticRegression(
                labelColumnName: nameof(InputData.Sentiment), featureColumnName: "Features"));

        Console.WriteLine("Training...");
        var model = pipeline.Fit(trainTestSplit.TrainSet);

        Console.WriteLine("Saving model...");
        mlContext.Model.Save(model, trainTestSplit.TrainSet.Schema, modelPath);
        Console.WriteLine("Model saved to " + modelPath);
    }
}