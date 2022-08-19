namespace NotepadBasedCalculator.Api
{
    public static class CultureHelper
    {
        public static bool IsCultureApplicable(string culture, string targetCulture)
        {
            return culture == SupportedCultures.Any || string.Equals(culture, targetCulture, StringComparison.OrdinalIgnoreCase);
        }
    }
}
