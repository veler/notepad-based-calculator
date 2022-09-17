namespace NotepadBasedCalculator.Api
{
    /// <summary>
    /// Represents a grammar provider.
    /// </summary>
    public interface IGrammarProvider
    {
        /// <summary>
        /// Loads the token definition grammars for the given culture.
        /// </summary>
        IReadOnlyList<TokenDefinitionGrammar>? LoadTokenDefinitionGrammars(string culture);
    }
}
