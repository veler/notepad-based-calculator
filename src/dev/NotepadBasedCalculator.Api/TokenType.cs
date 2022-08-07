namespace NotepadBasedCalculator.Api
{
    public enum TokenType
    {
        UnsupportedCharacter = 0,
        Whitespace = 1,
        SymbolOrPunctuation = 2,
        Number = 3,
        Word = 4,
        NewLine = 5
    }
}
