using NotepadBasedCalculator.Api;
using Xunit;

namespace NotepadBasedCalculator.Core.Tests
{
    public sealed class AlgebraTests
    {
        [Theory]
        [InlineData(1, 25, 1.25)]
        [InlineData(1.50, 25, 1.875)]
        public void FloatAddPercentage(float x, float percentage, float expectedResult)
        {
            IData data = new PercentageData(x.ToString(), 0, x.ToString().Length, percentage);
            x.Add(data);
            Assert.Equal(expectedResult, x);
        }

        [Theory]
        [InlineData(1, 1.25, 2.25)]
        [InlineData(1.50, 1.25, 2.75)]
        public void FloatAddDecimal(float x, float y, float expectedResult)
        {
            IData data = new DecimalData(x.ToString(), 0, x.ToString().Length, y);
            x.Add(data);
            Assert.Equal(expectedResult, x);
        }

        [Theory]
        [InlineData(1, 2, 1.50)]
        public void FloatAddFractional(float x, float fraction, float expectedResult)
        {
            IData data = new FractionData(x.ToString(), 0, x.ToString().Length, fraction);
            x.Add(data);
            Assert.Equal(expectedResult, x);
        }

        [Theory]
        [InlineData(1, true, 2)]
        [InlineData(1, false, 1)]
        public void FloatAddBoolean(float x, bool boolean, float expectedResult)
        {
            IData data = new BooleanData(x.ToString(), 0, x.ToString().Length, boolean);
            x.Add(data);
            Assert.Equal(expectedResult, x);
        }

        [Theory]
        [InlineData(1, 25, 0.75)]
        [InlineData(1.50, 25, 1.125)]
        public void FloatSubstractPercentage(float x, float percentage, float expectedResult)
        {
            IData data = new PercentageData(x.ToString(), 0, x.ToString().Length, percentage);
            x.Substract(data);
            Assert.Equal(expectedResult, x);
        }

        [Theory]
        [InlineData(1, 1.25, -0.25)]
        [InlineData(1.50, 1.25, 0.25)]
        public void FloatSubstractDecimal(float x, float y, float expectedResult)
        {
            IData data = new DecimalData(x.ToString(), 0, x.ToString().Length, y);
            x.Substract(data);
            Assert.Equal(expectedResult, x);
        }

        [Theory]
        [InlineData(1, 2, 0.50)]
        public void FloatSubstractFractional(float x, float fraction, float expectedResult)
        {
            IData data = new FractionData(x.ToString(), 0, x.ToString().Length, fraction);
            x.Substract(data);
            Assert.Equal(expectedResult, x);
        }

        [Theory]
        [InlineData(1, true, 0)]
        [InlineData(1, false, 1)]
        public void FloatSubstractBoolean(float x, bool boolean, float expectedResult)
        {
            IData data = new BooleanData(x.ToString(), 0, x.ToString().Length, boolean);
            x.Substract(data);
            Assert.Equal(expectedResult, x);
        }

        [Theory]
        [InlineData(1, 25, 0.25)]
        [InlineData(1.50, 25, 0.375)]
        public void FloatMultiplyPercentage(float x, float percentage, float expectedResult)
        {
            IData data = new PercentageData(x.ToString(), 0, x.ToString().Length, percentage);
            x.Multiply(data);
            Assert.Equal(expectedResult, x);
        }

        [Theory]
        [InlineData(1, 1.25, 1.25)]
        [InlineData(1.50, 1.25, 1.875)]
        public void FloatMultiplyDecimal(float x, float y, float expectedResult)
        {
            IData data = new DecimalData(x.ToString(), 0, x.ToString().Length, y);
            x.Multiply(data);
            Assert.Equal(expectedResult, x);
        }

        [Theory]
        [InlineData(1, 3, 0.333333343)]
        public void FloatMultiplyFractional(float x, float fraction, float expectedResult)
        {
            IData data = new FractionData(x.ToString(), 0, x.ToString().Length, fraction);
            x.Multiply(data);
            Assert.Equal(expectedResult, x);
        }

        [Theory]
        [InlineData(1, true, 1)]
        [InlineData(1, false, 0)]
        public void FloatMultiplyBoolean(float x, bool boolean, float expectedResult)
        {
            IData data = new BooleanData(x.ToString(), 0, x.ToString().Length, boolean);
            x.Multiply(data);
            Assert.Equal(expectedResult, x);
        }

        [Theory]
        [InlineData(1, 25, 4)]
        [InlineData(1.50, 25, 6)]
        public void FloatDividePercentage(float x, float percentage, float expectedResult)
        {
            IData data = new PercentageData(x.ToString(), 0, x.ToString().Length, percentage);
            x.Divide(data);
            Assert.Equal(expectedResult, x);
        }

        [Theory]
        [InlineData(1, 1.25, 0.800000012)]
        [InlineData(1.50, 1.25, 1.20000005)]
        [InlineData(1.50, 0, float.PositiveInfinity)]
        public void FloatDivideDecimal(float x, float y, float expectedResult)
        {
            IData data = new DecimalData(x.ToString(), 0, x.ToString().Length, y);
            x.Divide(data);
            Assert.Equal(expectedResult, x);
        }

        [Theory]
        [InlineData(1, 3, 3)]
        public void FloatDivideFractional(float x, float fraction, float expectedResult)
        {
            IData data = new FractionData(x.ToString(), 0, x.ToString().Length, fraction);
            x.Divide(data);
            Assert.Equal(expectedResult, x);
        }

        [Theory]
        [InlineData(1, true, 1)]
        [InlineData(1, false, float.PositiveInfinity)]
        public void FloatDivideBoolean(float x, bool boolean, float expectedResult)
        {
            IData data = new BooleanData(x.ToString(), 0, x.ToString().Length, boolean);
            x.Divide(data);
            Assert.Equal(expectedResult, x);
        }
    }
}
