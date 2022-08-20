using System.Runtime.CompilerServices;

namespace NotepadBasedCalculator.Api
{
    public static class BinaryOperation
    {
        #region Equal

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool IsEqualTo(this float input, IData value)
        {
            if (value is PercentageData percentageData)
            {
                return input.IsEqualTo(percentageData);
            }
            else if (value is DecimalData decimalData)
            {
                return input.IsEqualTo(decimalData);
            }
            else if (value is FractionData fractionData)
            {
                return input.IsEqualTo(fractionData);
            }
            else if (value is BooleanData booleanData)
            {
                return input.IsEqualTo(booleanData);
            }
            else
            {
                ThrowHelper.ThrowNotSupportedException();
                return false;
            }
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool IsEqualTo(this float input, PercentageData percentage)
        {
            return input.IsEqualTo(1f.GetPercentage(percentage));
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool IsEqualTo(this float input, DecimalData value)
        {
            return input.IsEqualTo(value.Value);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool IsEqualTo(this float input, FractionData fraction)
        {
            return input.IsEqualTo(1f.GetFraction(fraction));
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool IsEqualTo(this float input, BooleanData boolean)
        {
            return input.IsEqualTo(Convert.ToInt32(boolean.Value));
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool IsEqualTo(this float input, float value)
        {
            return input == value;
        }

        #endregion

        #region LessThan

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool LessThan(this float input, IData value)
        {
            if (value is PercentageData percentageData)
            {
                return input.LessThan(percentageData);
            }
            else if (value is DecimalData decimalData)
            {
                return input.LessThan(decimalData);
            }
            else if (value is FractionData fractionData)
            {
                return input.LessThan(fractionData);
            }
            else if (value is BooleanData booleanData)
            {
                return input.LessThan(booleanData);
            }
            else
            {
                ThrowHelper.ThrowNotSupportedException();
                return false;
            }
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool LessThan(this float input, PercentageData percentage)
        {
            return input.LessThan(1f.GetPercentage(percentage));
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool LessThan(this float input, DecimalData value)
        {
            return input.LessThan(value.Value);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool LessThan(this float input, FractionData fraction)
        {
            return input.LessThan(1f.GetFraction(fraction));
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool LessThan(this float input, BooleanData boolean)
        {
            return input.LessThan(Convert.ToInt32(boolean.Value));
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool LessThan(this float input, float value)
        {
            return input < value;
        }

        #endregion

        #region LessThanOrEqualTo

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool LessThanOrEqualTo(this float input, IData value)
        {
            if (value is PercentageData percentageData)
            {
                return input.LessThanOrEqualTo(percentageData);
            }
            else if (value is DecimalData decimalData)
            {
                return input.LessThanOrEqualTo(decimalData);
            }
            else if (value is FractionData fractionData)
            {
                return input.LessThanOrEqualTo(fractionData);
            }
            else if (value is BooleanData booleanData)
            {
                return input.LessThanOrEqualTo(booleanData);
            }
            else
            {
                ThrowHelper.ThrowNotSupportedException();
                return false;
            }
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool LessThanOrEqualTo(this float input, PercentageData percentage)
        {
            return input.LessThanOrEqualTo(1f.GetPercentage(percentage));
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool LessThanOrEqualTo(this float input, DecimalData value)
        {
            return input.LessThanOrEqualTo(value.Value);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool LessThanOrEqualTo(this float input, FractionData fraction)
        {
            return input.LessThanOrEqualTo(1f.GetFraction(fraction));
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool LessThanOrEqualTo(this float input, BooleanData boolean)
        {
            return input.LessThanOrEqualTo(Convert.ToInt32(boolean.Value));
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool LessThanOrEqualTo(this float input, float value)
        {
            return input <= value;
        }

        #endregion

        #region GreaterThan

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool GreaterThan(this float input, IData value)
        {
            if (value is PercentageData percentageData)
            {
                return input.GreaterThan(percentageData);
            }
            else if (value is DecimalData decimalData)
            {
                return input.GreaterThan(decimalData);
            }
            else if (value is FractionData fractionData)
            {
                return input.GreaterThan(fractionData);
            }
            else if (value is BooleanData booleanData)
            {
                return input.GreaterThan(booleanData);
            }
            else
            {
                ThrowHelper.ThrowNotSupportedException();
                return false;
            }
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool GreaterThan(this float input, PercentageData percentage)
        {
            return input.GreaterThan(1f.GetPercentage(percentage));
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool GreaterThan(this float input, DecimalData value)
        {
            return input.GreaterThan(value.Value);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool GreaterThan(this float input, FractionData fraction)
        {
            return input.GreaterThan(1f.GetFraction(fraction));
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool GreaterThan(this float input, BooleanData boolean)
        {
            return input.GreaterThan(Convert.ToInt32(boolean.Value));
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool GreaterThan(this float input, float value)
        {
            return input > value;
        }

        #endregion

        #region GreaterThanOrEqualTo

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool GreaterThanOrEqualTo(this float input, IData value)
        {
            if (value is PercentageData percentageData)
            {
                return input.GreaterThanOrEqualTo(percentageData);
            }
            else if (value is DecimalData decimalData)
            {
                return input.GreaterThanOrEqualTo(decimalData);
            }
            else if (value is FractionData fractionData)
            {
                return input.GreaterThanOrEqualTo(fractionData);
            }
            else if (value is BooleanData booleanData)
            {
                return input.GreaterThanOrEqualTo(booleanData);
            }
            else
            {
                ThrowHelper.ThrowNotSupportedException();
                return false;
            }
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool GreaterThanOrEqualTo(this float input, PercentageData percentage)
        {
            return input.GreaterThanOrEqualTo(1f.GetPercentage(percentage));
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool GreaterThanOrEqualTo(this float input, DecimalData value)
        {
            return input.GreaterThanOrEqualTo(value.Value);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool GreaterThanOrEqualTo(this float input, FractionData fraction)
        {
            return input.GreaterThanOrEqualTo(1f.GetFraction(fraction));
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool GreaterThanOrEqualTo(this float input, BooleanData boolean)
        {
            return input.GreaterThanOrEqualTo(Convert.ToInt32(boolean.Value));
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool GreaterThanOrEqualTo(this float input, float value)
        {
            return input >= value;
        }

        #endregion
    }
}
