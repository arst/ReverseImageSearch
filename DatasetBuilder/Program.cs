using Database;
using Vectorizer;

var datasetPath = "C:\\Projects\\Temp\\ReverseImageSearch\\DatasetBuilder\\Images";
var dbContext = new ImageDbContext();

string[] imageExtensions = { "*.png", "*.jpg", "*.jpeg", "*.bmp", "*.gif", "*.tiff" };

foreach (var ext in imageExtensions)
foreach (var filePath in Directory.EnumerateFiles(datasetPath, ext))
{
    var vector = Vectorizer.Vectorizer.Vectorize(new ImageData
    {
        ImagePath = filePath
    });
    var image = new Image
    {
        FilePath = filePath
    };
    await dbContext.AddAsync(image);
    await dbContext.SaveChangesAsync();
    var index = 0;
    var vectorValues = new List<Vector>();
    foreach (var item in vector)
    {
        vectorValues.Add(new Vector
        {
            ImageId = image.Id,
            VectorPosition = index,
            VectorValue = item,
            Image = image
        });
        index++;
    }

    await dbContext.AddRangeAsync(vectorValues);
}

await dbContext.SaveChangesAsync();

Console.WriteLine("Finished building dataset.");