using Microsoft.Maui.Graphics;
using Microsoft.Maui.Graphics.Skia;
using TagsCloudVisualization;

namespace DrawingTagsCloudVisualization;

public interface IDrawingTagsCloud
{
    public void SaveToFile(string filePath);
}
public class DrawingTagsCloud : IDrawingTagsCloud
{
    private List<RectangleInformation> rectangleInformation;

    public DrawingTagsCloud(List<RectangleInformation> rectangleInformation)
    {
        this.rectangleInformation = rectangleInformation;
    }

    public void SaveToFile(string filePath)
    {
        using var bitmapContext = new SkiaBitmapExportContext(400, 400, 2.0f);

        var canvas = bitmapContext.Canvas;
        canvas.FontColor = Colors.Black;
        canvas = Draw(canvas);
        using var image = bitmapContext.Image;
        using var stream = File.OpenWrite(filePath);
        image.Save(stream);
    }

    private ICanvas Draw(ICanvas canvas)
    {
        foreach (var rectInfo in rectangleInformation)
        {
            var rect = rectInfo.rectangle;
            var text = rectInfo.word;
            canvas.FillColor = Colors.Blue;
            //canvas.FillRectangle(rect.X, rect.Y, rect.Width, rect.Height);

            float fontSize = rect.Height;
            canvas.FontColor = Colors.White;
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