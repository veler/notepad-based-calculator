using System.Reflection;
using System.Runtime.CompilerServices;
using Microsoft.Recognizers.Text;

namespace NotepadBasedCalculator.BuiltInPlugins
{
    [Export(typeof(IGrammarProvider))]
    [Culture(SupportedCultures.English)]
    [Shared]
    internal class GrammarProvider : IGrammarProvider
    {
        public IReadOnlyList<TokenDefinitionGrammar>? LoadTokenDefinitionGrammar(string culture)
        {
            culture = culture.Replace("-", "_");

            var grammars = new List<TokenDefinitionGrammar>();
            TokenDefinitionGrammar? grammar = LoadGrammar($"NotepadBasedCalculator.BuiltInPlugins.Grammars.{culture}.TokenDefinition.json");
            if (grammar is not null)
            {
                grammars.Add(grammar);
            }

            grammar = LoadGrammar($"NotepadBasedCalculator.BuiltInPlugins.Grammars.SpecialTokenDefinition.json");
            if (grammar is not null)
            {
                grammars.Add(grammar);
            }

            return grammars;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private static TokenDefinitionGrammar? LoadGrammar(string resourceName)
        {
            var assembly = Assembly.GetExecutingAssembly();

            using Stream? embeddedResourceStream = assembly.GetManifestResourceStream(resourceName);
            if (embeddedResourceStream is null)
            {
                throw new Exception("Unable to find the grammar file.");
            }

            using var textStreamReader = new StreamReader(embeddedResourceStream);

            return TokenDefinitionGrammar.Load(textStreamReader.ReadToEnd());
        }
    }
}
