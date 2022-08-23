using System.Reflection;
using System.Runtime.CompilerServices;
using Newtonsoft.Json;

namespace NotepadBasedCalculator.BuiltInPlugins.Grammars
{
    [Export(typeof(IFunctionDefinitionProvider))]
    [Shared]
    public sealed class FunctionDefinitionProvider : IFunctionDefinitionProvider
    {
        private readonly ILexer _lexer;
        private readonly Dictionary<string, IReadOnlyList<FunctionDefinition>> _cultureToFunctionDefinition = new();

        [ImportingConstructor]
        public FunctionDefinitionProvider(ILexer lexer)
        {
            _lexer = lexer;
        }

        public IReadOnlyList<FunctionDefinition> LoadFunctionDefinition(string culture)
        {
            culture = culture.Replace("-", "_");

            lock (_cultureToFunctionDefinition)
            {
                if (!_cultureToFunctionDefinition.TryGetValue(culture, out IReadOnlyList<FunctionDefinition> functionDefinition) || functionDefinition is null)
                {
                    functionDefinition
                        = LoadResource(
                            culture,
                            $"NotepadBasedCalculator.BuiltInPlugins.Grammars.{culture}.FunctionDefinition.json");
                    _cultureToFunctionDefinition[culture] = functionDefinition;
                }

                return functionDefinition;
            }
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private IReadOnlyList<FunctionDefinition> LoadResource(string culture, string resourceName)
        {
            var assembly = Assembly.GetExecutingAssembly();

            using Stream? embeddedResourceStream = assembly.GetManifestResourceStream(resourceName);
            if (embeddedResourceStream is null)
            {
                throw new Exception("Unable to find the UnitNames file.");
            }

            using var textStreamReader = new StreamReader(embeddedResourceStream);

            Dictionary<string, Dictionary<string, string[]>>? parsedJson
                = JsonConvert.DeserializeObject<Dictionary<string, Dictionary<string, string[]>>>(
                    textStreamReader.ReadToEnd());

            var result = new List<FunctionDefinition>();

            if (parsedJson is not null)
            {
                foreach (string functionCategory in parsedJson.Keys)
                {
                    Dictionary<string, string[]> functionDefinitions = parsedJson[functionCategory];
                    foreach (string functionName in functionDefinitions.Keys)
                    {
                        string[] functionGrammars = functionDefinitions[functionName];
                        for (int i = 0; i < functionGrammars.Length; i++)
                        {
                            IReadOnlyList<TokenizedTextLine> tokenizedGrammarLines = _lexer.Tokenize(culture, functionGrammars[i]);
                            Guard.HasSizeEqualTo(tokenizedGrammarLines, 1);
                            TokenizedTextLine tokenizedGrammar = tokenizedGrammarLines[0];
                            Guard.IsNotNull(tokenizedGrammar.Tokens);
                            result.Add(new FunctionDefinition($"{functionCategory}.{functionName}", tokenizedGrammar.Tokens));
                        }
                    }
                }
            }

            return result;
        }
    }
}
