using NotepadBasedCalculator.Api;
using Xunit;

namespace NotepadBasedCalculator.Core.Tests
{
    public sealed class BinaryOperationTests
    {
        [Theory]
        [InlineData(4, 25, false)]
        [InlineData(0.4, 40, true)]
        public void FloatIsEqualToPercentage(float x, float percentage, bool expectedResult)
        {
            IData data = new PercentageData(x.ToString(), 0, x.ToString().Length, percentage);
            Assert.Equal(expectedResult, x.IsEqualTo(data));
        }

        [Theory]
        [InlineData(1, 1.25, false)]
        [InlineData(1.50, 1.50, true)]
        public void FloatIsEqualToDecimal(float x, float y, bool expectedResult)
        {
            IData data = new DecimalData(x.ToString(), 0, x.ToString().Length, y);
            Assert.Equal(expectedResult, x.IsEqualTo(data));
        }

        [Theory]
        [InlineData(1, 5, false)]
        [InlineData(0.2, 5, true)]
        public void FloatIsEqualToFractional(float x, float fraction, bool expectedResult)
        {
            IData data = new FractionData(x.ToString(), 0, x.ToString().Length, fraction);
            Assert.Equal(expectedResult, x.IsEqualTo(data));
        }

        [Theory]
        [InlineData(1, true, true)]
        [InlineData(1, false, false)]
        public void FloatIsEqualToBoolean(float x, bool boolean, bool expectedResult)
        {
            IData data = new BooleanData(x.ToString(), 0, x.ToString().Length, boolean);
            Assert.Equal(expectedResult, x.IsEqualTo(data));
        }

        [Theory]
        [InlineData(4, 25, true)]
        [InlineData(0.4, 40, false)]
        public void FloatIsNotEqualToPercentage(float x, float percentage, bool expectedResult)
        {
            IData data = new PercentageData(x.ToString(), 0, x.ToString().Length, percentage);
            Assert.Equal(expectedResult, !x.IsEqualTo(data));
        }

        [Theory]
        [InlineData(1, 1.25, true)]
        [InlineData(1.50, 1.50, false)]
        public void FloatIsNotEqualToDecimal(float x, float y, bool expectedResult)
        {
            IData data = new DecimalData(x.ToString(), 0, x.ToString().Length, y);
            Assert.Equal(expectedResult, !x.IsEqualTo(data));
        }

        [Theory]
        [InlineData(1, 5, true)]
        [InlineData(0.2, 5, false)]
        public void FloatIsNotEqualToFractional(float x, float fraction, bool expectedResult)
        {
            IData data = new FractionData(x.ToString(), 0, x.ToString().Length, fraction);
            Assert.Equal(expectedResult, !x.IsEqualTo(data));
        }

        [Theory]
        [InlineData(1, true, false)]
        [InlineData(1, false, true)]
        public void FloatIsNotEqualToBoolean(float x, bool boolean, bool expectedResult)
        {
            IData data = new BooleanData(x.ToString(), 0, x.ToString().Length, boolean);
            Assert.Equal(expectedResult, !x.IsEqualTo(data));
        }

        [Theory]
        [InlineData(0.1, 25, true)]
        [InlineData(0.4, 40, false)]
        public void FloatLessThanPercentage(float x, float percentage, bool expectedResult)
        {
            IData data = new PercentageData(x.ToString(), 0, x.ToString().Length, percentage);
            Assert.Equal(expectedResult, x.LessThan(data));
        }

        [Theory]
        [InlineData(1, 1.25, true)]
        [InlineData(1.50, 1.50, false)]
        public void FloatLessThanDecimal(float x, float y, bool expectedResult)
        {
            IData data = new DecimalData(x.ToString(), 0, x.ToString().Length, y);
            Assert.Equal(expectedResult, x.LessThan(data));
        }

        [Theory]
        [InlineData(0.1, 5, true)]
        [InlineData(1, 5, false)]
        public void FloatLessThanFractional(float x, float fraction, bool expectedResult)
        {
            IData data = new FractionData(x.ToString(), 0, x.ToString().Length, fraction);
            Assert.Equal(expectedResult, x.LessThan(data));
        }

        [Theory]
        [InlineData(0, true, true)]
        [InlineData(1, false, false)]
        public void FloatLessThanBoolean(float x, bool boolean, bool expectedResult)
        {
            IData data = new BooleanData(x.ToString(), 0, x.ToString().Length, boolean);
            Assert.Equal(expectedResult, x.LessThan(data));
        }

        [Theory]
        [InlineData(4, 25, false)]
        [InlineData(0.4, 40, true)]
        public void FloatLessThanOrEqualToPercentage(float x, float percentage, bool expectedResult)
        {
            IData data = new PercentageData(x.ToString(), 0, x.ToString().Length, percentage);
            Assert.Equal(expectedResult, x.LessThanOrEqualTo(data));
        }

        [Theory]
        [InlineData(1.25, 1, false)]
        [InlineData(1.50, 1.50, true)]
        public void FloatLessThanOrEqualToDecimal(float x, float y, bool expectedResult)
        {
            IData data = new DecimalData(x.ToString(), 0, x.ToString().Length, y);
            Assert.Equal(expectedResult, x.LessThanOrEqualTo(data));
        }

        [Theory]
        [InlineData(1, 5, false)]
        [InlineData(0.2, 5, true)]
        public void FloatLessThanOrEqualToFractional(float x, float fraction, bool expectedResult)
        {
            IData data = new FractionData(x.ToString(), 0, x.ToString().Length, fraction);
            Assert.Equal(expectedResult, x.LessThanOrEqualTo(data));
        }

        [Theory]
        [InlineData(1, true, true)]
        [InlineData(1, false, false)]
        public void FloatLessThanOrEqualToBoolean(float x, bool boolean, bool expectedResult)
        {
            IData data = new BooleanData(x.ToString(), 0, x.ToString().Length, boolean);
            Assert.Equal(expectedResult, x.LessThanOrEqualTo(data));
        }

        [Theory]
        [InlineData(4, 25, true)]
        [InlineData(0.4, 40, false)]
        public void FloatGreaterThanPercentage(float x, float percentage, bool expectedResult)
        {
            IData data = new PercentageData(x.ToString(), 0, x.ToString().Length, percentage);
            Assert.Equal(expectedResult, x.GreaterThan(data));
        }

        [Theory]
        [InlineData(1, 1.25, false)]
        [InlineData(1.75, 1.50, true)]
        public void FloatGreaterThanDecimal(float x, float y, bool expectedResult)
        {
            IData data = new DecimalData(x.ToString(), 0, x.ToString().Length, y);
            Assert.Equal(expectedResult, x.GreaterThan(data));
        }

        [Theory]
        [InlineData(1, 5, true)]
        [InlineData(0.2, 5, false)]
        public void FloatGreaterThanFractional(float x, float fraction, bool expectedResult)
        {
            IData data = new FractionData(x.ToString(), 0, x.ToString().Length, fraction);
            Assert.Equal(expectedResult, x.GreaterThan(data));
        }

        [Theory]
        [InlineData(1, true, false)]
        [InlineData(1, false, true)]
        public void FloatGreaterThanBoolean(float x, bool boolean, bool expectedResult)
        {
            IData data = new BooleanData(x.ToString(), 0, x.ToString().Length, boolean);
            Assert.Equal(expectedResult, x.GreaterThan(data));
        }

        [Theory]
        [InlineData(0.1, 25, false)]
        [InlineData(0.4, 40, true)]
        public void FloatGreaterThanOrEqualToPercentage(float x, float percentage, bool expectedResult)
        {
            IData data = new PercentageData(x.ToString(), 0, x.ToString().Length, percentage);
            Assert.Equal(expectedResult, x.GreaterThanOrEqualTo(data));
        }

        [Theory]
        [InlineData(1, 1.25, false)]
        [InlineData(1.50, 1.50, true)]
        public void FloatGreaterThanOrEqualToDecimal(float x, float y, bool expectedResult)
        {
            IData data = new DecimalData(x.ToString(), 0, x.ToString().Length, y);
            Assert.Equal(expectedResult, x.GreaterThanOrEqualTo(data));
        }

        [Theory]
        [InlineData(0.1, 5, false)]
        [InlineData(6, 5, true)]
        public void FloatGreaterThanOrEqualToFractional(float x, float fraction, bool expectedResult)
        {
            IData data = new FractionData(x.ToString(), 0, x.ToString().Length, fraction);
            Assert.Equal(expectedResult, x.GreaterThanOrEqualTo(data));
        }

        [Theory]
        [InlineData(0, true, false)]
        [InlineData(2, true, true)]
        public void FloatGreaterThanOrEqualToBoolean(float x, bool boolean, bool expectedResult)
        {
            IData data = new BooleanData(x.ToString(), 0, x.ToString().Length, boolean);
            Assert.Equal(expectedResult, x.GreaterThanOrEqualTo(data));
        }
    }
}
