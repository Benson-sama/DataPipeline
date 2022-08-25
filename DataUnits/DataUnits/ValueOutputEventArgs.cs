//------------------------------------------------------------------------
// <copyright file="ValueOutputEventArgs.cs" company="FH Wiener Neustadt">
//     Copyright (c) FH Wiener Neustadt. All rights reserved.
// </copyright>
// <author>Benjamin Bogner</author>
// <summary>Contains the ValueOutputEventArgs class.</summary>
//------------------------------------------------------------------------
namespace DataUnits
{
    using System;

    /// <summary>
    /// Represents the <see cref="ValueOutputEventArgs{T}"/> class.
    /// </summary>
    /// <typeparam name="T">The <see cref="Type"/> of value.</typeparam>
    public class ValueOutputEventArgs<T> : EventArgs
    {
        /// <summary>
        /// Initialises a new instance of the <see cref="ValueOutputEventArgs{T}"/> class.
        /// </summary>
        /// <param name="value">The output value.</param>
        public ValueOutputEventArgs(T value)
        {
            this.Value = value;
        }

        /// <summary>
        /// Gets the output value of this <see cref="ValueOutputEventArgs{T}"/>.
        /// </summary>
        /// <value>The output value of this <see cref="ValueOutputEventArgs{T}"/>.</value>
        public T Value { get; private set; }
    }
}
