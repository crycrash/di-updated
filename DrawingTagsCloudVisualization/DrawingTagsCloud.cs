using System.Runtime.CompilerServices;
using Microsoft.Maui.Graphics;
using Microsoft.Maui.Graphics.Skia;
using Microsoft.VisualBasic;
using TagsCloudVisualization;

namespace DrawingTagsCloudVisualization;

public interface IDrawingTagsCloud
{
    public void SaveToFile(string filePath, int lenght, int width, string color);
}
public class DrawingTagsCloud(List<RectangleInformation> rectangleInformation) : IDrawingTagsCloud
{
    private readonly List<RectangleInformation> rectangleInformation = rectangleInformation;

    private readonly Dictionary<string, Color> dictColors = new(){
            { "white", Colors.White },
            { "red", Colors.Red },
            { "green", Colors.Green },
            { "yellow", Colors.Yellow },
            { "blue", Colors.Blue },
            { "pink", Colors.Pink },
            { "black", Colors.Black },
    };

    public void SaveToFile(string filePath, int lenght, int width, string color)
    {
        using var bitmapContext = new SkiaBitmapExportContext(lenght, width, 2.0f);
        var canvas = bitmapContext.Canvas;
        canvas.FontColor = Colors.Black;
        canvas = Draw(canvas, color);
        using var image = bitmapContext.Image;
        using var stream = File.OpenWrite(filePath);
        image.Save(stream);
    }

    private ICanvas Draw(ICanvas canvas, string color)
    {
        foreach (var rectInfo in rectangleInformation)
        {
            var rect = rectInfo.rectangle;
            var text = rectInfo.word;
            //canvas.FillRectangle(rect.X, rect.Y, rect.Width, rect.Height);

            float fontSize = rect.Height;
            if (!dictColors.TryGetValue(color, out var colorGet))
                colorGet = Colors.Black;
            canvas.FontColor = colorGet;
            var textBounds = canvas.GetStringSize(text, Font.Default, fontSize);

            while ((textBounds.Width > rect.Width || textBounds.Height > rect.Height) && fontSize > 1)
            {
                fontSize -= 1;
                textBounds = canvas.GetStringSize(text, Font.Default, fontSize);
            }

            canvas.FontSize = fontSize;
            var textX = rect.X + (rect.Width - textBounds.Width) / 2;
            var textY = rect.Y + (rect.Height - textBounds.Height) / 2;

            canvas.DrawString(text, textX, textY, HorizontalAlignment.Left);
        }

        return canvas;
    }
}