using System.Runtime.CompilerServices;

namespace NotepadBasedCalculator.Api
{
    public static class Algebra
    {
        #region Add

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void Add(this ref long input, IData value)
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
            else
            {
                ThrowHelper.ThrowNotSupportedException();
            }
        }

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
        public static void Add(this ref float input, float value)
        {
            input += value;
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
