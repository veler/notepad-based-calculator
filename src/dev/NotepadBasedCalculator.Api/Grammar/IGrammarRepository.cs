namespace NotepadBasedCalculator.Api
{
    public interface IGrammarProvider
    {
        TokenDefinitionGrammar? LoadTokenDefinitionGrammar(string culture);
    }
}
