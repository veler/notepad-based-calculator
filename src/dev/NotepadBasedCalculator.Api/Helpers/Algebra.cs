using System.Runtime.CompilerServices;

namespace NotepadBasedCalculator.Api
{
    public static class Algebra
    {
        #region Add

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void Add(this ref float input, IData value)
        {
            if (value is PercentageData percentageData)
            {
                input.Add(percentageData);
            }
            else if (value is DecimalData decimalData)
            {
                input.Add(decimalData);
            }
            else if (value is FractionData fractionData)
            {
                input.Add(fractionData);
            }
            else if (value is BooleanData booleanData)
            {
                input.Add(booleanData);
            }
            else
            {
                ThrowHelper.ThrowNotSupportedException();
            }
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void Add(this ref float input, PercentageData percentage)
        {
            input.Add(input.GetPercentage(percentage));
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void Add(this ref float input, DecimalData value)
        {
            input.Add(value.Value);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void Add(this ref float input, FractionData fraction)
        {
            input.Add(input.GetFraction(fraction));
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void Add(this ref float input, BooleanData value)
        {
            input.Add(Convert.ToInt32(value.Value));
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void Add(this ref float input, float value)
        {
            input += value;
        }

        #endregion

        #region Substract

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void Substract(this ref float input, IData value)
        {
            if (value is PercentageData percentageData)
            {
                input.Substract(percentageData);
            }
            else if (value is DecimalData decimalData)
            {
                input.Substract(decimalData);
            }
            else if (value is FractionData fractionData)
            {
                input.Substract(fractionData);
            }
            else if (value is BooleanData booleanData)
            {
                input.Substract(booleanData);
            }
            else
            {
                ThrowHelper.ThrowNotSupportedException();
            }
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void Substract(this ref float input, PercentageData percentage)
        {
            input.Substract(input.GetPercentage(percentage));
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void Substract(this ref float input, DecimalData value)
        {
            input.Substract(value.Value);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void Substract(this ref float input, FractionData fraction)
        {
            input.Substract(input.GetFraction(fraction));
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void Substract(this ref float input, BooleanData value)
        {
            input.Substract(Convert.ToInt32(value.Value));
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void Substract(this ref float input, float value)
        {
            input -= value;
        }

        #endregion

        #region Multiply

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void Multiply(this ref float input, IData value)
        {
            if (value is PercentageData percentageData)
            {
                input.Multiply(percentageData);
            }
            else if (value is DecimalData decimalData)
            {
                input.Multiply(decimalData);
            }
            else if (value is FractionData fractionData)
            {
                input.Multiply(fractionData);
            }
            else if (value is BooleanData booleanData)
            {
                input.Multiply(booleanData);
            }
            else
            {
                ThrowHelper.ThrowNotSupportedException();
            }
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void Multiply(this ref float input, PercentageData percentage)
        {
            input.Multiply(1f.GetPercentage(percentage));
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void Multiply(this ref float input, DecimalData value)
        {
            input.Multiply(value.Value);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void Multiply(this ref float input, FractionData fraction)
        {
            input.Multiply(1f.GetFraction(fraction));
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void Multiply(this ref float input, BooleanData value)
        {
            input.Multiply(Convert.ToInt32(value.Value));
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void Multiply(this ref float input, float value)
        {
            input *= value;
        }

        #endregion

        #region Divide

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void Divide(this ref float input, IData value)
        {
            if (value is PercentageData percentageData)
            {
                input.Divide(percentageData);
            }
            else if (value is DecimalData decimalData)
            {
                input.Divide(decimalData);
            }
            else if (value is FractionData fractionData)
            {
                input.Divide(fractionData);
            }
            else if (value is BooleanData booleanData)
            {
                input.Divide(booleanData);
            }
            else
            {
                ThrowHelper.ThrowNotSupportedException();
            }
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void Divide(this ref float input, PercentageData percentage)
        {
            input.Divide(1f.GetPercentage(percentage));
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void Divide(this ref float input, DecimalData value)
        {
            input.Divide(value.Value);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void Divide(this ref float input, FractionData fraction)
        {
            input.Divide(1f.GetFraction(fraction));
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void Divide(this ref float input, BooleanData value)
        {
            if (!value.Value)
            {
                input = float.PositiveInfinity;
            }
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void Divide(this ref float input, float value)
        {
            if (value == 0)
            {
                input = float.PositiveInfinity;
            }
            else
            {
                input /= value;
            }
        }

        #endregion

        #region Fraction

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static float GetFraction(this float input, FractionData fraction)
        {
            return GetFraction(input, fraction.Value);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static float GetFraction(this float input, float fraction)
        {
            return input / fraction;
        }

        #endregion

        #region Percentage

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static float GetPercentage(this float input, PercentageData percentage)
        {
            return GetPercentage(input, percentage.Value);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static float GetPercentage(this float input, float percentage)
        {
            return percentage / 100 * input;
        }

        #endregion
    }
}
