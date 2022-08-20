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

    public class ValueOutputEventArgs<T> : EventArgs
    {
        public ValueOutputEventArgs(T value)
        {
            this.Value = value;
        }

        public T Value { get; private set; }
    }
}
