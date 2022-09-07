namespace NotepadBasedCalculator.Api
{
    public interface ILexer
    {
        IReadOnlyList<TokenizedTextLine> Tokenize(string culture, string? wholeDocument);
    }
}
