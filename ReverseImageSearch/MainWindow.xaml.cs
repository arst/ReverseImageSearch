using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Media.Imaging;
using Database;
using Microsoft.Win32;
using Newtonsoft.Json;
using Vectorizer;

namespace ReverseImageSearch;

public partial class MainWindow : Window
{
    private readonly ImageDbContext _dbContext;

    public MainWindow()
    {
        InitializeComponent();
        _dbContext = new ImageDbContext();
    }

    private void OnSelectImage(object sender, RoutedEventArgs e)
    {
        var openFileDialog = new OpenFileDialog
        {
            Filter = "Image files (*.png;*.jpeg)|*.png;*.jpeg|All files (*.*)|*.*",
            RestoreDirectory = true
        };

        if (openFileDialog.ShowDialog() == true)
        {
            var selectedFilePath = openFileDialog.FileName;
            var searchResults = PerformSearch(selectedFilePath);
            DisplaySearchResults(searchResults);
        }
    }

    private List<string> PerformSearch(string imagePath)
    {
        var bitmap = new BitmapImage(new Uri(imagePath, UriKind.Absolute));
        selectedImage.Source = bitmap;
        var vector = Vectorizer.Vectorizer.Vectorize(new ImageData { ImagePath = imagePath });
        var stringifiedVector = JsonConvert.SerializeObject(vector);
        var similarImages = _dbContext.GetSimilarImages(stringifiedVector)
            .OrderByDescending(s => s.CosineDistance)
            .Where(s => s.CosineDistance > 0.5)
            .Select(i => i.FilePath)
            .ToList();
        return similarImages;
    }

    private void DisplaySearchResults(List<string> searchResults)
    {
        imageList.Items.Clear();
        foreach (var result in searchResults) imageList.Items.Add(result);
    }
}