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
            //if (leftData is null || rightData is null)
            //{
            //    return leftData;
            //}

            //if (leftData is not INumericData leftNumericData
            //    || rightData is not INumericData rightNumericData)
            //{
            //    return null;
            //}

            //bool result;

            //switch (binaryOperatorType)
            //{
            //    case BinaryOperatorType.Equality:
            //        result
            //            = leftNumericData.NumericValueInStandardUnit
            //            == rightNumericData.NumericValueInStandardUnit;
            //        break;

            //    case BinaryOperatorType.NoEquality:
            //        result
            //            = leftNumericData.NumericValueInStandardUnit
            //            != rightNumericData.NumericValueInStandardUnit;
            //        break;

            //    case BinaryOperatorType.LessThan:
            //        result
            //            = leftNumericData.NumericValueInStandardUnit
            //            < rightNumericData.NumericValueInStandardUnit;
            //        break;

            //    case BinaryOperatorType.LessThanOrEqualTo:
            //        result
            //            = leftNumericData.NumericValueInStandardUnit
            //            <= rightNumericData.NumericValueInStandardUnit;
            //        break;

            //    case BinaryOperatorType.GreaterThan:
            //        result
            //            = leftNumericData.NumericValueInStandardUnit
            //            > rightNumericData.NumericValueInStandardUnit;
            //        break;

            //    case BinaryOperatorType.GreaterThanOrEqualTo:
            //        result
            //            = leftNumericData.NumericValueInStandardUnit
            //            >= rightNumericData.NumericValueInStandardUnit;
            //        break;

            //    default:
            //        ThrowHelper.ThrowNotSupportedException();
            //        return null;
            //}

            //return new BooleanData(
            //    leftData.LineTextIncludingLineBreak,
            //    leftData.StartInLine,
            //    rightData.EndInLine,
            //    result);
            return null;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static IData? PerformAlgebraOperation(IData? leftData, BinaryOperatorType binaryOperatorType, IData? rightData)
        {
            //if (leftData is null || rightData is null)
            //{
            //    return leftData;
            //}

            //if (leftData is not INumericData leftNumericData
            //    || rightData is not INumericData rightNumericData)
            //{
            //    return null;
            //}

            //bool tryToConvertLeftData = true;
            //if (binaryOperatorType is not BinaryOperatorType.Multiply and not BinaryOperatorType.Division
            //    && leftData is IConvertibleNumericData leftConvertibleNumericData)
            //{
            //    if (leftConvertibleNumericData.CanConvertFrom(rightNumericData))
            //    {
            //        INumericData? newRightNumericData = leftConvertibleNumericData.ConvertFrom(rightNumericData);
            //        if (newRightNumericData is not null)
            //        {
            //            rightNumericData = newRightNumericData;
            //            tryToConvertLeftData = false;
            //        }
            //    }
            //}

            //if (tryToConvertLeftData
            //    && binaryOperatorType is not BinaryOperatorType.Multiply and not BinaryOperatorType.Division
            //    && rightData is IConvertibleNumericData rightConvertibleNumericData)
            //{
            //    if (rightConvertibleNumericData.CanConvertFrom(leftNumericData))
            //    {
            //        INumericData? newLeftNumericData = rightConvertibleNumericData.ConvertFrom(leftNumericData);
            //        if (newLeftNumericData is null)
            //        {
            //            return null;
            //        }
            //        leftNumericData = newLeftNumericData;
            //    }
            //    else
            //    {
            //        return null;
            //    }
            //}

            //double result;
            //switch (binaryOperatorType)
            //{
            //    case BinaryOperatorType.Addition:
            //        result
            //            = leftNumericData.NumericValueInStandardUnit
            //            + rightNumericData.ToStandardUnit().GetNumericValueToRelativeTo(leftNumericData.ToStandardUnit());
            //        break;

            //    case BinaryOperatorType.Subtraction:
            //        result
            //            = leftNumericData.NumericValueInStandardUnit
            //            - rightNumericData.ToStandardUnit().GetNumericValueToRelativeTo(leftNumericData.ToStandardUnit());
            //        break;

            //    case BinaryOperatorType.Multiply:
            //        result
            //            = leftNumericData.NumericValueInStandardUnit
            //            * rightNumericData.NumericValueInStandardUnit;
            //        break;

            //    case BinaryOperatorType.Division:
            //        double divisor = rightNumericData.NumericValueInStandardUnit;
            //        if (divisor == 0)
            //        {
            //            result = double.PositiveInfinity;
            //        }
            //        else
            //        {
            //            result
            //                = leftNumericData.NumericValueInStandardUnit
            //                / divisor;
            //        }
            //        break;

            //    default:
            //        ThrowHelper.ThrowNotSupportedException();
            //        return null;
            //}

            //return leftNumericData.FromStandardUnit(result).MergeDataLocations(rightNumericData);
            return null;
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
            if (leftData is not INumericData leftNumericData || rightData is not INumericData rightNumericData)
            {
                return leftData;
            }

            bool canLeftConvertToRight
                = leftNumericData is IConvertibleNumericData leftConvertibleNumericData
                && leftConvertibleNumericData.CanConvertTo(rightNumericData);

            bool canRightConvertToRight
                = rightNumericData is IConvertibleNumericData rightConvertibleNumericData
                && rightConvertibleNumericData.CanConvertTo(leftNumericData);

            return null;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static IData? Convert(IData? from, object toUnit)
        {
            
        }
    }
}
