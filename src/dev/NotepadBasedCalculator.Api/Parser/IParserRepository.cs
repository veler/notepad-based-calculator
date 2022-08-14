namespace NotepadBasedCalculator.Api
{
    internal interface IParserRepository
    {
        IEnumerable<IDataParser> GetApplicableDataParsers(string culture);

        IEnumerable<IExpressionParser> GetApplicableExpressionParsers(string culture);

        IEnumerable<IStatementParser> GetApplicableStatementParsers(string culture);
    }
}
