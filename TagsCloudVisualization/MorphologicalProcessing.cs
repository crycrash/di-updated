using MyStemWrapper;
namespace TagsCloudVisualization;

public interface IMorphologicalAnalyzer
{
    bool IsExcludedWord(string word);
}

public class MorphologicalProcessing : IMorphologicalAnalyzer
{
    private readonly MyStem _mystem;

    public MorphologicalProcessing(MyStem mystem)
    {
        _mystem = mystem;
    }

    public bool IsExcludedWord(string word)
    {
        var res = _mystem.Analysis(word);
        return res.Contains("CONJ") || res.Contains("INTJ") || res.Contains("PART")
               || res.Contains("PR") || res.Contains("SPRO");
    }
}