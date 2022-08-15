namespace NotepadBasedCalculator.Core
{
    internal static class CultureHelper
    {
        internal static bool IsCultureApplicable(string culture, string targetCulture)
        {
            return culture == SupportedCultures.Any || string.Equals(culture, targetCulture, StringComparison.OrdinalIgnoreCase);
        }
    }
}
