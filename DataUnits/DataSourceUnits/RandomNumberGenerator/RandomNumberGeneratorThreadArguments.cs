//----------------------------------------------------------------------------------------
// <copyright file="RandomNumberGeneratorThreadArguments.cs" company="FH Wiener Neustadt">
//     Copyright (c) FH Wiener Neustadt. All rights reserved.
// </copyright>
// <author>Benjamin Bogner</author>
// <summary>Contains the RandomNumberGeneratorThreadArguments class.</summary>
//----------------------------------------------------------------------------------------
namespace RandomNumberGenerator
{
    /// <summary>
    /// Represents the <see cref="RandomNumberGeneratorThreadArguments"/> class.
    /// </summary>
    public class RandomNumberGeneratorThreadArguments
    {
        /// <summary>
        /// Gets or sets a value indicating whether the <see cref="RandomNumberGenerator"/> shall exit.
        /// </summary>
        /// <value>The value indicating whether the <see cref="RandomNumberGenerator"/> shall exit.</value>
        public bool Exit { get; set; }
    }
}
