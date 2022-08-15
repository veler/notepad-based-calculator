namespace NotepadBasedCalculator.Api
{
    internal interface IParserRepository
    {
        IEnumerable<IDataParser> GetApplicableDataParsers(string culture);

        IEnumerable<IExpressionParser> GetApplicableExpressionParsers(string culture);

        IEnumerable<IStatementParser> GetApplicableStatementParsers(string culture);

        IExpressionParser? GetExpressionParser(string culture, string expressionParserName);

        IStatementParser? GetStatementParser(string culture, string expressionParserName);
    }
}
