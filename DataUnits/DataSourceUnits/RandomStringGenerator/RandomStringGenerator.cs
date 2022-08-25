//-------------------------------------------------------------------------
// <copyright file="RandomStringGenerator.cs" company="FH Wiener Neustadt">
//     Copyright (c) FH Wiener Neustadt. All rights reserved.
// </copyright>
// <author>Benjamin Bogner</author>
// <summary>Contains the RandomStringGenerator class.</summary>
//-------------------------------------------------------------------------
namespace RandomStringGenerator
{
    using System;
    using DataPipeline.Model.Attributes;
    using DataUnits;

    /// <summary>
    /// Represents the <see cref="RandomStringGenerator"/> class.
    /// </summary>
    [DataUnitInformation(
           name: "Random string generator",
           Description = "Generates a string with random characters from A to Z.",
           InputDatatype = typeof(void),
           InputDescription = "None.",
           OutputDatatype = typeof(string),
           OutputDescription = "A string with a minimum length of 5 and a maximum length of 10 characters.")]
    public class RandomStringGenerator
    {
        /// <summary>
        /// The <see cref="Random"/> instance used to create randomised strings.
        /// </summary>
        private Random random;

        /// <summary>
        /// Initialises a new instance of the <see cref="RandomStringGenerator"/> class.
        /// </summary>
        public RandomStringGenerator()
        {
            this.random = new Random();
        }

        /// <summary>
        /// The event that gets fired when a value got generated.
        /// </summary>
        [DataOutput]
        public event EventHandler<ValueOutputEventArgs<string>> ValueGenerated;

        /// <summary>
        /// Starts this data unit.
        /// </summary>
        public void Start()
        {
            int length = this.random.Next(5, 11);
            string value = string.Empty;

            for (int i = 0; i < length; i++)
            {
                value += Convert.ToChar(this.random.Next(65, 91));
            }

            ValueOutputEventArgs<string> valueOutputEventArgs = new ValueOutputEventArgs<string>(value);
            this.ValueGenerated?.Invoke(this, valueOutputEventArgs);
        }

        /// <summary>
        /// Stops this data unit.
        /// </summary>
        public void Stop()
        {
        }
    }
}
