namespace NotepadBasedCalculator.Api
{
    public static class CultureHelper
    {
        /// <summary>
        /// Determines whether the given <paramref name="culture"/> is compatible with the <paramref name="targetCulture"/>.
        /// </summary>
        public static bool IsCultureApplicable(string culture, string targetCulture)
        {
            Guard.IsNotNull(culture);
            Guard.IsNotNull(targetCulture);
            return culture == SupportedCultures.Any || string.Equals(culture, targetCulture, StringComparison.OrdinalIgnoreCase);
        }
    }
}
