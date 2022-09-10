namespace NotepadBasedCalculator.Api
{
    internal interface IParserRepository
    {
        IEnumerable<IDataParser> GetApplicableDataParsers(string culture);

        IEnumerable<IStatementParserAndInterpreter> GetApplicableStatementParsersAndInterpreters(string culture);

        IEnumerable<IExpressionParserAndInterpreter> GetApplicableExpressionParsersAndInterpreters(string culture);

        IExpressionParserAndInterpreter[] GetExpressionParserAndInterpreters(string culture, params string[] expressionParserAndInterpreterNames);
    }
}
