namespace NotepadBasedCalculator.Api
{
    public static class DictionaryExtensions
    {
        public static TValue? GetValueOrDefault<TKey, TValue>(this IDictionary<TKey, TValue> dictionary, TKey key)
        {
            return dictionary.TryGetValue(key, out TValue? value) ? value : default;
        }
    }
}
