namespace NotepadBasedCalculator.Api
{
    internal interface ITokenEnumerator : IEnumerator<Token?>
    {
        Token? InternalCurrentToken { get; }
    }
}
