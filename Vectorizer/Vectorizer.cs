using Microsoft.ML;
using Microsoft.ML.Data;

namespace Vectorizer;

public static class Vectorizer
{
    public static float[] Vectorize(ImageData image)
    {
        return Vectorize(new List<ImageData> { image })[0];
    }

    private static float[][] Vectorize(IEnumerable<ImageData> images)
    {
        var mlContext = new MLContext();
        var m = new MemoryStream();

        var dataview = mlContext.Data.LoadFromEnumerable(images);

        var pipeline = mlContext.Transforms
            .LoadImages("ImageObject", "Images", "ImagePath")
            .Append(mlContext.Transforms.ResizeImages("ImageObject", 224, 224))
            .Append(mlContext.Transforms.ExtractPixels("Pixels", "ImageObject"))
            .Append(mlContext.Transforms.DnnFeaturizeImage("Features",
                m => m.ModelSelector.ResNet18(mlContext, m.OutputColumn, m.InputColumn), "Pixels"));

        var transformedData = pipeline.Fit(dataview).Transform(dataview);

        var vector = transformedData.GetColumn<float[]>("Features").ToArray();

        return vector;
    }
}