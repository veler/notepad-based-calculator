﻿namespace NotepadBasedCalculator.Api.AbstractSyntaxTree
{
    /// <summary>
    /// Basic class that represents an expression in a statement.
    /// </summary>
    public abstract class Expression
    {
        /// <summary>
        /// Gets a string representation of the expression.
        /// </summary>
        /// <returns>String that reprensents the expression</returns>
        public abstract override string ToString();
    }
}
