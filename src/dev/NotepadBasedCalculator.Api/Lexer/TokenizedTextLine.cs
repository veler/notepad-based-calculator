namespace NotepadBasedCalculator.Api
{
    public sealed class TokenizedTextLine
    {
        public int Start { get; }

        public int End => Start + Length;

        public int EndIncludingLineBreak => Start + LengthIncludingLineBreak;

        public int Length => LengthIncludingLineBreak - LineBreakLength;

        public int LineBreakLength { get; }

        public int LengthIncludingLineBreak { get; }

        public string LineTextIncludingLineBreak { get; }

        public LinkedToken? Tokens { get; }

        internal TokenizedTextLine(
            int start,
            int lineBreakLength,
            string? lineTextIncludingLineBreak,
            LinkedToken? tokens)
        {
            Guard.IsGreaterThanOrEqualTo(start, 0);
            Guard.IsGreaterThanOrEqualTo(lineBreakLength, 0);

            Start = start;
            LineBreakLength = lineBreakLength;
            LineTextIncludingLineBreak = lineTextIncludingLineBreak ?? string.Empty;
            LengthIncludingLineBreak = LineTextIncludingLineBreak.Length;
            Tokens = tokens;
        }
    }
}
