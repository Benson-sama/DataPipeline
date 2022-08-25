//----------------------------------------------------------------------------
// <copyright file="DateTimeSecondsGenerator.cs" company="FH Wiener Neustadt">
//     Copyright (c) FH Wiener Neustadt. All rights reserved.
// </copyright>
// <author>Benjamin Bogner</author>
// <summary>Contains the DateTimeSecondsGenerator class.</summary>
//----------------------------------------------------------------------------
namespace DateTimeSecondsGenerator
{
    using System;
    using DataPipeline.Model.Attributes;
    using DataUnits;

    /// <summary>
    /// Represents the <see cref="DateTimeSecondsGenerator"/> class.
    /// </summary>
    [DataUnitInformation(
        name: "Current DateTime seconds generator",
        Description = "Generates the current seconds component.",
        InputDatatype = typeof(void),
        InputDescription = "None.",
        OutputDatatype = typeof(int),
        OutputDescription = "A number representing the current seconds of time.")]
    public class DateTimeSecondsGenerator
    {
        /// <summary>
        /// The event that gets fired when a value got generated.
        /// </summary>
        [DataOutput]
        public event EventHandler<ValueOutputEventArgs<int>> ValueGenerated;

        /// <summary>
        /// Starts this data unit, thereby generating only a single value.
        /// </summary>
        public void Start()
        {
            ValueOutputEventArgs<int> valueOutputEventArgs = new ValueOutputEventArgs<int>(DateTime.Now.Second);
            this.ValueGenerated?.Invoke(this, valueOutputEventArgs);
        }

        /// <summary>
        /// Stops this data unit. Does nothing, as it does not generate values periodically.
        /// </summary>
        public void Stop()
        {
        }
    }
}
