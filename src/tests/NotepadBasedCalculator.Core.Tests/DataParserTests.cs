using System.Threading.Tasks;
using NotepadBasedCalculator.Api;
using NotepadBasedCalculator.BuiltInPlugins.Data.Definition;
using UnitsNet;
using UnitsNet.Units;
using Xunit;

namespace NotepadBasedCalculator.Core.Tests
{
    public sealed class DataParserTests : MefBaseTest
    {
        [Theory]
        [InlineData("thirty five thousand", 35000)]
        [InlineData("forty three thousand", 43000)]
        [InlineData("nine hundred and seventy four thousand", 974000)]
        [InlineData("I have nine hundred and seventy four thousand items", 974000)]
        public async Task IntegerParsingAsync(string input, int output)
        {
            Parser parser = ExportProvider.Import<Parser>();
            ParserResult parserResult = await parser.ParseAsync(input);
            Assert.Single(parserResult.Lines[0].Data);
            Assert.True(parserResult.Lines[0].Data[0].Is(PredefinedTokenAndDataTypeNames.Numeric));
            Assert.Equal(PredefinedTokenAndDataTypeNames.SubDataTypeNames.Decimal, parserResult.Lines[0].Data[0].Subtype);
            Assert.Equal(output, ((DecimalData)parserResult.Lines[0].Data[0]).Value);
        }

        [Theory]
        [InlineData("thirty five thousand point one two three", 35000.123)]
        [InlineData("forty three thousand point fifty seven", 43000.57)]
        [InlineData("1.1^+23", 8.95430243255239)]
        public async Task DecimalParsingAsync(string input, double output)
        {
            Parser parser = ExportProvider.Import<Parser>();
            ParserResult parserResult = await parser.ParseAsync(input);
            Assert.Single(parserResult.Lines[0].Data);
            Assert.True(parserResult.Lines[0].Data[0].Is(PredefinedTokenAndDataTypeNames.Numeric));
            Assert.Equal(PredefinedTokenAndDataTypeNames.SubDataTypeNames.Decimal, parserResult.Lines[0].Data[0].Subtype);
            Assert.Equal(output, ((DecimalData)parserResult.Lines[0].Data[0]).Value);
        }

        [Theory]
        [InlineData("a fifth", 0.2)]
        [InlineData("a hundred thousand trillionths", 1E-07)]
        public async Task FractionParsingAsync(string input, double output)
        {
            Parser parser = ExportProvider.Import<Parser>();
            ParserResult parserResult = await parser.ParseAsync(input);
            Assert.Single(parserResult.Lines[0].Data);
            Assert.True(parserResult.Lines[0].Data[0].Is(PredefinedTokenAndDataTypeNames.Numeric));
            Assert.Equal(PredefinedTokenAndDataTypeNames.SubDataTypeNames.Fraction, parserResult.Lines[0].Data[0].Subtype);
            Assert.Equal(output, ((FractionData)parserResult.Lines[0].Data[0]).Value);
        }

        [Theory]
        [InlineData("one hundred percents", 1f)]
        [InlineData("per cent of twenty-two", 0.22)]
        public async Task PercentageParsingAsync(string input, double output)
        {
            Parser parser = ExportProvider.Import<Parser>();
            ParserResult parserResult = await parser.ParseAsync(input);
            Assert.Single(parserResult.Lines[0].Data);
            Assert.True(parserResult.Lines[0].Data[0].Is(PredefinedTokenAndDataTypeNames.Numeric));
            Assert.Equal(PredefinedTokenAndDataTypeNames.SubDataTypeNames.Percentage, parserResult.Lines[0].Data[0].Subtype);
            Assert.Equal(output, ((PercentageData)parserResult.Lines[0].Data[0]).Value);
        }

        [Theory]
        [InlineData("one hundred and fifty thousand dollars", 150000, "Dollar", "", false)]
        [InlineData(" $ -75.3 million USD", -75300000, "United States dollar", "USD", true)]
        public async Task CurrencyParsingAsync(string input, double value, string unit, string iso, bool isNegative)
        {
            Parser parser = ExportProvider.Import<Parser>();
            ParserResult parserResult = await parser.ParseAsync(input);
            Assert.Single(parserResult.Lines[0].Data);
            Assert.True(parserResult.Lines[0].Data[0].Is(PredefinedTokenAndDataTypeNames.Numeric));
            CurrencyValue currency = ((CurrencyData)parserResult.Lines[0].Data[0]).Value;
            Assert.Equal(isNegative, ((CurrencyData)parserResult.Lines[0].Data[0]).IsNegative);
            Assert.Equal(value, currency.Value);
            Assert.Equal(unit, currency.Currency);
            Assert.Equal(iso, currency.IsoCurrency);
        }

        [Theory]
        [InlineData("three kilometers", 3, "Length", LengthUnit.Kilometer, false)]
        public async Task LengthParsingAsync(string input, double value, string subType, LengthUnit unit, bool isNegative)
        {
            Parser parser = ExportProvider.Import<Parser>();
            ParserResult parserResult = await parser.ParseAsync(input);
            Assert.Single(parserResult.Lines[0].Data);
            Assert.True(parserResult.Lines[0].Data[0].Is(PredefinedTokenAndDataTypeNames.Numeric));
            Assert.Equal(subType, ((LengthData)parserResult.Lines[0].Data[0]).Subtype);
            Length unitFloat = ((LengthData)parserResult.Lines[0].Data[0]).Value;
            Assert.Equal(isNegative, ((LengthData)parserResult.Lines[0].Data[0]).IsNegative);
            Assert.Equal(value, unitFloat.Value);
            Assert.Equal(unit, unitFloat.Unit);
        }

        [Theory]
        [InlineData("three megabyte", 3, "Information", InformationUnit.Megabyte, false)]
        public async Task InformationParsingAsync(string input, decimal value, string subType, InformationUnit unit, bool isNegative)
        {
            Parser parser = ExportProvider.Import<Parser>();
            ParserResult parserResult = await parser.ParseAsync(input);
            Assert.Single(parserResult.Lines[0].Data);
            Assert.True(parserResult.Lines[0].Data[0].Is(PredefinedTokenAndDataTypeNames.Numeric));
            Assert.Equal(subType, ((InformationData)parserResult.Lines[0].Data[0]).Subtype);
            Information unitFloat = ((InformationData)parserResult.Lines[0].Data[0]).Value;
            Assert.Equal(isNegative, ((InformationData)parserResult.Lines[0].Data[0]).IsNegative);
            Assert.Equal(value, unitFloat.Value);
            Assert.Equal(unit, unitFloat.Unit);
        }

        [Theory]
        [InlineData("three sq km", 3, "Area", AreaUnit.SquareKilometer, false)]
        public async Task AreaParsingAsync(string input, double value, string subType, AreaUnit unit, bool isNegative)
        {
            Parser parser = ExportProvider.Import<Parser>();
            ParserResult parserResult = await parser.ParseAsync(input);
            Assert.Single(parserResult.Lines[0].Data);
            Assert.True(parserResult.Lines[0].Data[0].Is(PredefinedTokenAndDataTypeNames.Numeric));
            Assert.Equal(subType, ((AreaData)parserResult.Lines[0].Data[0]).Subtype);
            Area unitFloat = ((AreaData)parserResult.Lines[0].Data[0]).Value;
            Assert.Equal(isNegative, ((AreaData)parserResult.Lines[0].Data[0]).IsNegative);
            Assert.Equal(value, unitFloat.Value);
            Assert.Equal(unit, unitFloat.Unit);
        }

        [Theory]
        [InlineData("three mph", 3, "Speed", SpeedUnit.MilePerHour, false)]
        public async Task SpeedParsingAsync(string input, double value, string subType, SpeedUnit unit, bool isNegative)
        {
            Parser parser = ExportProvider.Import<Parser>();
            ParserResult parserResult = await parser.ParseAsync(input);
            Assert.Single(parserResult.Lines[0].Data);
            Assert.True(parserResult.Lines[0].Data[0].Is(PredefinedTokenAndDataTypeNames.Numeric));
            Assert.Equal(subType, ((SpeedData)parserResult.Lines[0].Data[0]).Subtype);
            Speed unitFloat = ((SpeedData)parserResult.Lines[0].Data[0]).Value;
            Assert.Equal(isNegative, ((SpeedData)parserResult.Lines[0].Data[0]).IsNegative);
            Assert.Equal(value, unitFloat.Value);
            Assert.Equal(unit, unitFloat.Unit);
        }

        [Theory]
        [InlineData("three liter", 3, "Volume", VolumeUnit.Liter, false)]
        public async Task VolumeParsingAsync(string input, double value, string subType, VolumeUnit unit, bool isNegative)
        {
            Parser parser = ExportProvider.Import<Parser>();
            ParserResult parserResult = await parser.ParseAsync(input);
            Assert.Single(parserResult.Lines[0].Data);
            Assert.True(parserResult.Lines[0].Data[0].Is(PredefinedTokenAndDataTypeNames.Numeric));
            Assert.Equal(subType, ((VolumeData)parserResult.Lines[0].Data[0]).Subtype);
            Volume unitFloat = ((VolumeData)parserResult.Lines[0].Data[0]).Value;
            Assert.Equal(isNegative, ((VolumeData)parserResult.Lines[0].Data[0]).IsNegative);
            Assert.Equal(value, unitFloat.Value);
            Assert.Equal(unit, unitFloat.Unit);
        }

        [Theory]
        [InlineData("three kilograms", 3, "Mass", MassUnit.Kilogram, false)]
        public async Task MassParsingAsync(string input, double value, string subType, MassUnit unit, bool isNegative)
        {
            Parser parser = ExportProvider.Import<Parser>();
            ParserResult parserResult = await parser.ParseAsync(input);
            Assert.Single(parserResult.Lines[0].Data);
            Assert.True(parserResult.Lines[0].Data[0].Is(PredefinedTokenAndDataTypeNames.Numeric));
            Assert.Equal(subType, ((MassData)parserResult.Lines[0].Data[0]).Subtype);
            Mass unitFloat = ((MassData)parserResult.Lines[0].Data[0]).Value;
            Assert.Equal(isNegative, ((MassData)parserResult.Lines[0].Data[0]).IsNegative);
            Assert.Equal(value, unitFloat.Value);
            Assert.Equal(unit, unitFloat.Unit);
        }

        [Theory]
        [InlineData("three degrees", 3, "Angle", AngleUnit.Degree, false)]
        public async Task AngleParsingAsync(string input, double value, string subType, AngleUnit unit, bool isNegative)
        {
            Parser parser = ExportProvider.Import<Parser>();
            ParserResult parserResult = await parser.ParseAsync(input);
            Assert.Single(parserResult.Lines[0].Data);
            Assert.True(parserResult.Lines[0].Data[0].Is(PredefinedTokenAndDataTypeNames.Numeric));
            Assert.Equal(subType, ((AngleData)parserResult.Lines[0].Data[0]).Subtype);
            Angle unitFloat = ((AngleData)parserResult.Lines[0].Data[0]).Value;
            Assert.Equal(isNegative, ((AngleData)parserResult.Lines[0].Data[0]).IsNegative);
            Assert.Equal(value, unitFloat.Value);
            Assert.Equal(unit, unitFloat.Unit);
        }
    }
}
