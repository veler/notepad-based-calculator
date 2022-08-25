using System.Collections.Immutable;

namespace NotepadBasedCalculator.BuiltInPlugins.Statements.Function
{
    [Export(typeof(IStatementParser))]
    [Culture(SupportedCultures.Any)]
    [Shared]
    internal sealed class FunctionStatementParser : ParserBase, IStatementParser, IComparer<FunctionDefinition>
    {
        private readonly IEnumerable<Lazy<IFunctionDefinitionProvider, CultureCodeMetadata>> _functionDefinitionProviders;
        private readonly ILexer _lexer;
        private readonly Dictionary<string, IEnumerable<IFunctionDefinitionProvider>> _applicableFunctionDefinitionProviders = new();
        private readonly Dictionary<string, IReadOnlyList<FunctionDefinition>> _applicableFunctionDefinitions = new();

        [ImportingConstructor]
        public FunctionStatementParser(
            [ImportMany] IEnumerable<Lazy<IFunctionDefinitionProvider, CultureCodeMetadata>> functionDefinitionProviders,
            ILexer lexer)
        {
            _functionDefinitionProviders = functionDefinitionProviders;
            _lexer = lexer;
        }

        public bool TryParseStatement(string culture, LinkedToken currentToken, out Statement? statement)
        {
            IReadOnlyList<FunctionDefinition> functionDefinitions = GetOrderedFunctionDefinitions(culture);

            for (int i = 0; i < functionDefinitions.Count; i++)
            {
                // TODO: Optimization:
                //       Many functions start with the same set of words. We could group them in addition of their lenght
                //       so the search of matching function would run faster. Although, we'd need to do a .GetText() on 
                //       each token, which isn't efficient either.
                FunctionDefinition functionDefinition = functionDefinitions[i];

                LinkedToken? documentToken = currentToken;
                LinkedToken? functionDefinitionToken = functionDefinition.TokenizedFunctionDefinition;
                while (documentToken is not null
                    && functionDefinitionToken is not null)
                {
                    // 1. Check whether functionDefinitionToken.Token corresponds to a data type / expression / statement.
                    // 2. If so, try to parse that expression / statement. We will need to interpret the expression to know if it's the expected data type.
                    // 3. if the token doesn't correspond to a data type / expression...etc, then let's just compare it with the documentToken.

                    if (IsSpecialToken(functionDefinitionToken.Token))
                    {
                        string nextExpectedFunctionTokenType = functionDefinitionToken.Next?.Token.Type ?? string.Empty;
                        string nextExpectedFunctionTokenText = functionDefinitionToken.Next?.Token.GetText() ?? string.Empty;
                        Expression? expression
                            = ParseExpression(
                                culture,
                                documentToken,
                                nextExpectedFunctionTokenType,
                                nextExpectedFunctionTokenText,
                                out LinkedToken? nextToken);

                        if (expression is  null)
                        {
                            break;
                        }

                        // TODO: interpret the expression.
                    }
                    else if (!documentToken.Token.Is(functionDefinitionToken.Token.Type, functionDefinitionToken.Token.GetText()))
                    {
                        break;
                    }

                    documentToken = documentToken.Next;
                    functionDefinitionToken = functionDefinitionToken.Next;
                }
            }

            statement = null;
            return false;
        }

        public int Compare(FunctionDefinition x, FunctionDefinition y)
        {
            if (x.TokenCount < y.TokenCount)
            {
                return 1;
            }

            return -1;
        }

        private IReadOnlyList<FunctionDefinition> GetOrderedFunctionDefinitions(string culture)
        {
            lock (_applicableFunctionDefinitions)
            {
                if (_applicableFunctionDefinitions.TryGetValue(culture, out IReadOnlyList<FunctionDefinition>? definitions) && definitions is not null)
                {
                    return definitions;
                }

                var result = new List<FunctionDefinition>();
                foreach (IFunctionDefinitionProvider functionProvider in GetApplicableFunctionDefinitionProviders(culture))
                {
                    IReadOnlyList<Dictionary<string, Dictionary<string, string[]>>> parsedJsons = functionProvider.LoadFunctionDefinition(culture);
                    if (parsedJsons is not null)
                    {
                        for (int i = 0; i < parsedJsons.Count; i++)
                        {
                            foreach (string functionCategory in parsedJsons[i].Keys)
                            {
                                Dictionary<string, string[]> functionDefinitions = parsedJsons[i][functionCategory];
                                foreach (string functionName in functionDefinitions.Keys)
                                {
                                    string[] functionGrammars = functionDefinitions[functionName];
                                    for (int j = 0; j < functionGrammars.Length; j++)
                                    {
                                        IReadOnlyList<TokenizedTextLine> tokenizedGrammarLines = _lexer.Tokenize(culture, functionGrammars[j]);
                                        Guard.HasSizeEqualTo(tokenizedGrammarLines, 1);
                                        TokenizedTextLine tokenizedGrammar = tokenizedGrammarLines[0];
                                        Guard.IsNotNull(tokenizedGrammar.Tokens);
                                        result.Add(new FunctionDefinition($"{functionCategory}.{functionName}", tokenizedGrammar.Tokens));
                                    }
                                }
                            }
                        }
                    }
                }

                definitions = result.ToImmutableSortedSet(this);
                Guard.HasSizeEqualTo(definitions, result.Count);

                _applicableFunctionDefinitions[culture] = definitions;
                return definitions;
            }
        }

        private IEnumerable<IFunctionDefinitionProvider> GetApplicableFunctionDefinitionProviders(string culture)
        {
            lock (_applicableFunctionDefinitionProviders)
            {
                if (_applicableFunctionDefinitionProviders.TryGetValue(culture, out IEnumerable<IFunctionDefinitionProvider>? providers) && providers is not null)
                {
                    return providers;
                }

                providers
                    = _functionDefinitionProviders.Where(
                        p => p.Metadata.CultureCodes.Any(
                            c => CultureHelper.IsCultureApplicable(c, culture)))
                    .Select(p => p.Value);

                _applicableFunctionDefinitionProviders[culture] = providers;
                return providers;
            }
        }

        private static bool IsSpecialToken(IToken token)
        {
            for (int i = token.StartInLine; i < token.EndInLine; i++)
            {
                if (char.IsLetter(token.LineTextIncludingLineBreak[i]) && !char.IsUpper(token.LineTextIncludingLineBreak[i]))
                {

                    return false;
                }
            }
            return true;
        }
    }
}
