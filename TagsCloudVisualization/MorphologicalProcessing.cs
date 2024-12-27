using MyStemWrapper;
namespace TagsCloudVisualization;

public class MorphologicalProcessing : IMorphologicalAnalyzer
{
    private readonly MyStem _mystem;

    public MorphologicalProcessing(MyStem mystem)
    {
        _mystem = mystem;
    }

    public bool IsExcludedWord(string word, string option)
    {
        var res = _mystem.Analysis(word);
        if (option.Equals("all"))
            return res.Contains("CONJ") || res.Contains("INTJ") || res.Contains("PART")
                || res.Contains("PR") || res.Contains("SPRO");
        else
            return !res.Contains(option) || res.Contains("CONJ") || res.Contains("INTJ") || res.Contains("PART")
                || res.Contains("PR") || res.Contains("SPRO");
    }
}