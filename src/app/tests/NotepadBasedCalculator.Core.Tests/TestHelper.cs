using NotepadBasedCalculator.Api;

namespace NotepadBasedCalculator.Core.Tests
{
    public static class TestHelper
    {
        public static string GetDataDisplayText(this IData data)
        {
            return data.GetDisplayText(SupportedCultures.English);
        }
    }
}
