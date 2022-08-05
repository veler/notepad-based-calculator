using Xunit;

namespace NotepadBasedCalculator.Core.Tests
{
    public class ParserTests : MefBaseTest
    {
        [Fact]
        public void TokenizeEmpty()
        {
            Parser parser = ExportProvider.Import<Parser>();
            parser.Parse("$123");
        }
    }
}
