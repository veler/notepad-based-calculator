namespace NotepadBasedCalculator.Api
{
    /// <summary>
    /// Defines identifiers for supported binary operators
    /// </summary>
    public enum BinaryOperatorType
    {
        /// <summary>
        /// Identity equal operator
        /// </summary>
        [Description("==")]
        Equality = 0,

        /// <summary>
        /// Identity no equal operator
        /// </summary>
        [Description("!=")]
        NoEquality = 1,

        /// <summary>
        /// Boolean or operator. This represents a short circuiting operator. A short circuiting
        /// operator will evaluate only as many expressions as necessary before returning
        /// a correct value.
        /// </summary>
        [Description("OR")]
        LogicalOr = 2,

        /// <summary>
        /// Boolean and operator. This represents a short circuiting operator. A short circuiting
        /// operator will evaluate only as many expressions as necessary before returning
        /// a correct value.
        /// </summary>
        [Description("AND")]
        LogicalAnd = 3,

        /// <summary>
        /// Less than operator
        /// </summary>
        [Description("<")]
        LessThan = 4,

        /// <summary>
        /// Less than or equal operator
        /// </summary>
        [Description("<=")]
        LessThanOrEqualTo = 5,

        /// <summary>
        /// Greater than operator
        /// </summary>
        [Description(">")]
        GreaterThan = 6,

        /// <summary>
        /// Greater than or equal operator
        /// </summary>
        [Description(">=")]
        GreaterThanOrEqualTo = 7,

        /// <summary>
        /// Addition operator
        /// </summary>
        [Description("+")]
        Addition = 8,

        /// <summary>
        /// Subtraction operator
        /// </summary>
        [Description("-")]
        Subtraction = 9,

        /// <summary>
        /// Multiplication operator
        /// </summary>
        [Description("*")]
        Multiply = 10,

        /// <summary>
        /// Division operator
        /// </summary>
        [Description("/")]
        Division = 11,
    }
}
