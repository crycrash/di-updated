using System.Drawing;
using TagsCloudVisualization.ManagingRendering;

namespace TagsCloudVisualization;

public interface IRectangleGenerator
{
    public List<RectangleInformation> ExecuteRectangles(Dictionary<string, int> frequencyRectangles, Point center);
}
public class RectangleGenerator : IRectangleGenerator
{
    private static readonly List<RectangleInformation> rectangleInformation = [];
    private static readonly Dictionary<string, Size> rectangleData = [];
    private static void GenerateRectangles(Dictionary<string, int> frequencyRectangles)
    {
        var totalCountWords = frequencyRectangles.Sum(x => x.Value);
        var sortedWords = frequencyRectangles.OrderByDescending(word => word.Value);
        foreach (var word in sortedWords)
        {
            var width = word.Value * 300 / totalCountWords;
            var height = word.Value * 170 / totalCountWords;
            var rectangleSize = new Size(Math.Max(width, 1), Math.Max(height, 1));
            rectangleData.Add(word.Key, rectangleSize);
        }
    }
    private static List<RectangleInformation> PutRectangles(Point center)
    {
        var layouter = new CircularCloudLayouter(center);
        foreach (var rect in rectangleData)
        {
            var tempRect = layouter.PutNextRectangle(rect.Value);
            rectangleInformation.Add(new RectangleInformation(tempRect, rect.Key));
        }
        return rectangleInformation;
    }
    public List<RectangleInformation> ExecuteRectangles(Dictionary<string, int> frequencyRectangles, Point center)
    {
        GenerateRectangles(frequencyRectangles);
        return PutRectangles(center);
    }
}