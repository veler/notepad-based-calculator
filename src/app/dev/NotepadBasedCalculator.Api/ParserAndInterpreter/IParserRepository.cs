namespace NotepadBasedCalculator.Api
{
    internal interface IParserRepository
    {
        IEnumerable<IDataParser> GetApplicableDataParsers(string culture);

        IEnumerable<IStatementParserAndInterpreter> GetApplicableStatementParsersAndInterpreters(string culture);

        IEnumerable<IExpressionParserAndInterpreter> GetApplicableExpressionParsersAndInterpreters(string culture);

        IExpressionParserAndInterpreter GetExpressionParserAndInterpreter(string culture, string expressionParserName);
    }
}
