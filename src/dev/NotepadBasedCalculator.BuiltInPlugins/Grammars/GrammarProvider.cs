using System.Reflection;

namespace NotepadBasedCalculator.BuiltInPlugins
{
    [Export(typeof(IGrammarProvider))]
    [Culture(SupportedCultures.Any)]
    internal class GrammarProvider : IGrammarProvider
    {
        public TokenDefinitionGrammar? LoadTokenDefinitionGrammar(string culture)
        {
            culture = culture.Replace("-", "_");
            var assembly = Assembly.GetExecutingAssembly();

            using Stream? embeddedResourceStream = assembly.GetManifestResourceStream($"NotepadBasedCalculator.BuiltInPlugins.Grammars.{culture}.TokenDefinition.json");
            if (embeddedResourceStream is null)
            {
                throw new Exception("Unable to find the grammar file.");
            }

            using var textStreamReader = new StreamReader(embeddedResourceStream);

            return TokenDefinitionGrammar.Load(textStreamReader.ReadToEnd());
        }
    }
}
