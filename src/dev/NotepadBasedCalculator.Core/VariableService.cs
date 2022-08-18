namespace NotepadBasedCalculator.Core
{
    internal sealed class VariableService : IVariableService
    {
        private Dictionary<string, IData?> _variables = new();

        public IData? GetVariableValue(string variableName)
        {
            Guard.IsNotNullOrWhiteSpace(variableName);
            lock (_variables)
            {
                if (_variables.TryGetValue(variableName, out IData? value))
                {
                    return value;
                }

                return null;
            }
        }

        public void SetVariableValue(string variableName, IData? value)
        {
            Guard.IsNotNullOrWhiteSpace(variableName);
            lock (_variables)
            {
                _variables[variableName] = value;
            }
        }

        internal IReadOnlyDictionary<string, IData?> CreateBackup()
        {
            lock (_variables)
            {
                return new Dictionary<string, IData?>(_variables);
            }
        }

        internal void RestoreBackup(IReadOnlyDictionary<string, IData?>? backup)
        {
            lock (_variables)
            {
                if (backup is null)
                {
                    _variables = new();
                }
                else
                {
                    _variables = backup.ToDictionary(kv => kv.Key, kv => kv.Value);
                }
            }
        }
    }
}
