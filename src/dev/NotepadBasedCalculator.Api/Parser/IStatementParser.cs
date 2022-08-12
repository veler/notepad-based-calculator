namespace NotepadBasedCalculator.Api
{
    public interface IStatementParser
    {
        bool TryParseStatement(string culture, LinkedToken currentToken, out Statement? statement);
    }
}
