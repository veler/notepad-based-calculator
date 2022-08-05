using NotepadBasedCalculator.Api;
using NotepadBasedCalculator.Api.AbstractSyntaxTree;

namespace NotepadBasedCalculator.Core
{
    [Export]
    internal sealed class Parser
    {
        private readonly IEnumerable<Lazy<IExpressionParser, ExpressionParserMetadata>> _expressionParsers;
        private readonly Lexer _lexer = new();

        [ImportingConstructor]
        public Parser(
            [ImportMany] IEnumerable<Lazy<IExpressionParser, ExpressionParserMetadata>> expressionParsers)
        {
            _expressionParsers = expressionParsers;
        }

        internal void Parse(string? input)
        {
            IReadOnlyList<IReadOnlyList<Token>> tokenLines = _lexer.Tokenize(input);

            for (int i = 0; i < tokenLines.Count; i++)
            {
                IReadOnlyList<Token> tokens = tokenLines[i];
                var linkedToken = LinkedToken.CreateFromList(tokens);

                if (linkedToken is not null)
                {
                    foreach (Lazy<IExpressionParser, ExpressionParserMetadata>? expressionParser in _expressionParsers)
                    {
                        expressionParser.Value.TryParseExpression(linkedToken, out Expression? expression);
                    }
                }
            }
        }
    }
}
