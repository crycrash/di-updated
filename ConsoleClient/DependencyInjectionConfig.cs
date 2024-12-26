using System.Drawing;
using Autofac;
using DrawingTagsCloudVisualization;
using TagsCloudVisualization;
using TagsCloudVisualization.FilesProcessing;
using TagsCloudVisualization.ManagingRendering;

namespace ConsoleClient;

public static class DependencyInjectionConfig
{
    public static IContainer BuildContainer(Options options)
    {
        var builder = new ContainerBuilder();

        builder.RegisterInstance(new MyStemWrapper.MyStem
        {
            PathToMyStem = "mystem",
            Parameters = "-ni"
        }).As<MyStemWrapper.MyStem>().SingleInstance();

        builder.RegisterType<MorphologicalProcessing>()
            .As<IMorphologicalAnalyzer>()
            .InstancePerDependency();

        builder.RegisterType<TxtFileProcessor>()
            .As<IFileProcessor>()
            .InstancePerDependency();

        builder.Register<ISpiral>(c =>
        {
            var centerPoint = new Point(options.CenterX, options.CenterY);
            return options.Algorithm switch
            {
                "Circle" => new ArchimedeanSpiral(centerPoint, 1),
                _ => new FermatSpiral(centerPoint, 20),
            };
        }).As<ISpiral>().InstancePerDependency();

        builder.RegisterType<WordHandler>()
            .As<IWordHandler>()
            .InstancePerDependency();
        builder.RegisterType<RectangleGenerator>()
            .As<IRectangleGenerator>()
            .InstancePerDependency();
        builder.RegisterType<TagsCloudDrawingFacade>()
            .As<ITagsCloudDrawingFacade>()
            .InstancePerDependency();
        builder.RegisterType<TagsCloudDrawer>()
            .As<ITagsCloudDrawer>()
            .InstancePerDependency();
        builder.RegisterType<ImageSaver>()
            .As<IImageSaver>()
            .InstancePerDependency();

        return builder.Build();
    }
}
