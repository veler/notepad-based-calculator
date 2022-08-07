using NotepadBasedCalculator.Api;
using NotepadBasedCalculator.Api.AbstractSyntaxTree;

namespace NotepadBasedCalculator.BuiltInPlugins.Comment
{
    internal sealed class CommentExpression : Expression
    {
        public CommentExpression(LinkedToken firstToken, LinkedToken lastToken)
            : base(firstToken, lastToken)
        {
        }

        public override string ToString()
        {
            return "Comment";
        }
    }
}
