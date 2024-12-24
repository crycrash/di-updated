namespace TagsCloudVisualization;

public interface IWordHandler
{
    public Dictionary<string, int> ProcessFile(string filePath);
}
public class WordHandler : IWordHandler
{
    public WordHandler(IMorphologicalAnalyzer morphologicalAnalyzer, IFileProcessor fileProcessor)
    {
        _morphologicalAnalyzer = morphologicalAnalyzer;
        _fileProcessor = fileProcessor;
    }
    private readonly Dictionary<string, int> keyValueWords = [];
    private readonly IMorphologicalAnalyzer _morphologicalAnalyzer;
    private readonly IFileProcessor _fileProcessor;

    public Dictionary<string, int> ProcessFile(string filePath)
    {
        var words = _fileProcessor.ReadWords(filePath);

        foreach (var word in words)
        {
            var normalizedWord = word.ToLower();
            if (!_morphologicalAnalyzer.IsExcludedWord(word))
            {
                if (keyValueWords.ContainsKey(normalizedWord))
                    keyValueWords[normalizedWord]++;
                else
                    keyValueWords[normalizedWord] = 1;
            }
        }
        return keyValueWords;
    }
}