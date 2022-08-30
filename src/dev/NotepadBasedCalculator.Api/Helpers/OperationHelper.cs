using System.Runtime.CompilerServices;

namespace NotepadBasedCalculator.Api
{
    public static class OperationHelper
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static IData? PerformOperation(IData? leftData, BinaryOperatorType binaryOperatorType, IData? rightData)
        {
            if (leftData is not null)
            {
                switch (binaryOperatorType)
                {
                    case BinaryOperatorType.Equality:
                    case BinaryOperatorType.NoEquality:
                    case BinaryOperatorType.LessThan:
                    case BinaryOperatorType.LessThanOrEqualTo:
                    case BinaryOperatorType.GreaterThan:
                    case BinaryOperatorType.GreaterThanOrEqualTo:
                        return PerformBinaryOperation(leftData, binaryOperatorType, rightData);

                    case BinaryOperatorType.Addition:
                    case BinaryOperatorType.Subtraction:
                    case BinaryOperatorType.Multiply:
                    case BinaryOperatorType.Division:
                        return PerformAlgebraOperation(leftData, binaryOperatorType, rightData);

                    default:
                        ThrowHelper.ThrowNotSupportedException();
                        break;
                }
            }

            return null;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static IData? PerformBinaryOperation(IData? leftData, BinaryOperatorType binaryOperatorType, IData? rightData)
        {
            if (leftData is null || rightData is null)
            {
                return leftData;
            }

            if (leftData is not INumericData leftNumericData
                || rightData is not INumericData rightNumericData)
            {
                return null;
            }

            bool result;

            switch (binaryOperatorType)
            {
                case BinaryOperatorType.Equality:
                    result
                        = leftNumericData.ToStandardUnit().NumericValue
                        == rightNumericData.ToStandardUnit().NumericValue;
                    break;

                case BinaryOperatorType.NoEquality:
                    result
                        = leftNumericData.ToStandardUnit().NumericValue
                        != rightNumericData.ToStandardUnit().NumericValue;
                    break;

                case BinaryOperatorType.LessThan:
                    result
                        = leftNumericData.ToStandardUnit().NumericValue
                        < rightNumericData.ToStandardUnit().NumericValue;
                    break;

                case BinaryOperatorType.LessThanOrEqualTo:
                    result
                        = leftNumericData.ToStandardUnit().NumericValue
                        <= rightNumericData.ToStandardUnit().NumericValue;
                    break;

                case BinaryOperatorType.GreaterThan:
                    result
                        = leftNumericData.ToStandardUnit().NumericValue
                        > rightNumericData.ToStandardUnit().NumericValue;
                    break;

                case BinaryOperatorType.GreaterThanOrEqualTo:
                    result
                        = leftNumericData.ToStandardUnit().NumericValue
                        >= rightNumericData.ToStandardUnit().NumericValue;
                    break;

                default:
                    ThrowHelper.ThrowNotSupportedException();
                    return null;
            }

            return new BooleanData(
                leftData.LineTextIncludingLineBreak,
                leftData.StartInLine,
                rightData.EndInLine,
                result);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static IData? PerformAlgebraOperation(IData? leftData, BinaryOperatorType binaryOperatorType, IData? rightData)
        {
            if (leftData is null || rightData is null)
            {
                return leftData;
            }

            if (leftData is not INumericData leftNumericData
                || rightData is not INumericData rightNumericData)
            {
                return null;
            }

            bool tryToConvertLeftData = true;
            if (binaryOperatorType is not BinaryOperatorType.Multiply and not BinaryOperatorType.Division
                && leftData is IConvertibleNumericData leftConvertibleNumericData)
            {
                if (leftConvertibleNumericData.CanConvertFrom(rightNumericData))
                {
                    INumericData? newRightNumericData = leftConvertibleNumericData.ConvertFrom(rightNumericData);
                    if (newRightNumericData is not null)
                    {
                        rightNumericData = newRightNumericData;
                        tryToConvertLeftData = false;
                    }
                }
            }

            if (tryToConvertLeftData
                && binaryOperatorType is not BinaryOperatorType.Multiply and not BinaryOperatorType.Division
                && rightData is IConvertibleNumericData rightConvertibleNumericData)
            {
                if (rightConvertibleNumericData.CanConvertFrom(leftNumericData))
                {
                    INumericData? newLeftNumericData = rightConvertibleNumericData.ConvertFrom(leftNumericData);
                    if (newLeftNumericData is null)
                    {
                        return null;
                    }
                    leftNumericData = newLeftNumericData;
                }
                else
                {
                    return null;
                }
            }

            double result;
            switch (binaryOperatorType)
            {
                case BinaryOperatorType.Addition:
                    result
                        = leftNumericData.ToStandardUnit().NumericValue
                        + rightNumericData.ToStandardUnit().GetNumericValueToRelativeTo(leftNumericData.ToStandardUnit());
                    break;

                case BinaryOperatorType.Subtraction:
                    result
                        = leftNumericData.ToStandardUnit().NumericValue
                        - rightNumericData.ToStandardUnit().GetNumericValueToRelativeTo(leftNumericData.ToStandardUnit());
                    break;

                case BinaryOperatorType.Multiply:
                    result
                        = leftNumericData.ToStandardUnit().NumericValue
                        * rightNumericData.ToStandardUnit().NumericValue;
                    break;

                case BinaryOperatorType.Division:
                    double divisor = rightNumericData.ToStandardUnit().NumericValue;
                    if (divisor == 0)
                    {
                        result = double.PositiveInfinity;
                    }
                    else
                    {
                        result
                            = leftNumericData.ToStandardUnit().NumericValue
                            / divisor;
                    }
                    break;

                default:
                    ThrowHelper.ThrowNotSupportedException();
                    return null;
            }

            return leftNumericData.FromStandardUnit(result).MergeDataLocations(rightNumericData);
        }







        public static IData? NewPerformOperation(IData? leftData, BinaryOperatorType binaryOperatorType, IData? rightData)
        {
            if (leftData is not null)
            {
                switch (binaryOperatorType)
                {
                    case BinaryOperatorType.Equality:
                    case BinaryOperatorType.NoEquality:
                    case BinaryOperatorType.LessThan:
                    case BinaryOperatorType.LessThanOrEqualTo:
                    case BinaryOperatorType.GreaterThan:
                    case BinaryOperatorType.GreaterThanOrEqualTo:
                        break;

                    case BinaryOperatorType.Addition:
                    case BinaryOperatorType.Subtraction:
                    case BinaryOperatorType.Multiply:
                    case BinaryOperatorType.Division:
                        break;

                    default:
                        ThrowHelper.ThrowNotSupportedException();
                        break;
                }
            }

            return null;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static IData? NewPerformBinaryOperation(IData? leftData, BinaryOperatorType binaryOperatorType, IData? rightData)
        {
            return null;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static IData? NewPerformAlgebraOperation(IData? leftData, BinaryOperatorType binaryOperatorType, IData? rightData)
        {
            return null;
        }
    }
}
