namespace NotepadBasedCalculator.Api
{
    public interface IGrammarProvider
    {
        IReadOnlyList<TokenDefinitionGrammar>? LoadTokenDefinitionGrammar(string culture);
    }
}
