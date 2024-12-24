using System.Drawing;

namespace TagsCloudVisualization;

public class RectangleInformation(Rectangle rectangle, string word)
{
    public readonly Rectangle rectangle = rectangle;
    public readonly string word = word;
}