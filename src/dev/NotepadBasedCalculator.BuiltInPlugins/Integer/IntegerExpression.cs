using NotepadBasedCalculator.Api;
using NotepadBasedCalculator.Api.AbstractSyntaxTree;

namespace NotepadBasedCalculator.BuiltInPlugins.Integer
{
    internal sealed class IntegerExpression : Expression
    {
        internal int Value { get; }

        public IntegerExpression(
            LinkedToken firstToken,
            LinkedToken lastToken,
            int value)
            : base(firstToken, lastToken)
        {
            Value = value;
        }

        public override string ToString()
        {
            return Value.ToString();
        }
    }
}
