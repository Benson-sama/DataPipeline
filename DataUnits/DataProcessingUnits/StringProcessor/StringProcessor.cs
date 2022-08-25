//-------------------------------------------------------------------
// <copyright file="StringProcessor.cs" company="FH Wiener Neustadt">
//     Copyright (c) FH Wiener Neustadt. All rights reserved.
// </copyright>
// <author>Benjamin Bogner</author>
// <summary>Contains the StringProcessor class.</summary>
//-------------------------------------------------------------------
namespace StringProcessor
{
    using System;
    using DataPipeline.Model.Attributes;
    using DataUnits;

    /// <summary>
    /// Represents the <see cref="StringProcessor"/> class.
    /// </summary>
    [DataUnitInformation(
        name: "String processor",
        Description = "Processes a single string and outputs a number.",
        InputDatatype = typeof(string),
        InputDescription = "A string with no constraint.",
        OutputDatatype = typeof(int),
        OutputDescription = "A whole number.")]
    public class StringProcessor
    {
        /// <summary>
        /// The event that gets fired when a new value got generated.
        /// </summary>
        [DataOutput]
        public event EventHandler<ValueOutputEventArgs<int>> ValueGenerated;

        /// <summary>
        /// Gets a value indicating whether this data unit is running.
        /// </summary>
        /// <value>The value indicating whether this data unit is running.</value>
        public bool IsRunning { get; private set; }

        /// <summary>
        /// Starts this data unit.
        /// </summary>
        public void Start()
        {
            this.IsRunning = true;
        }

        /// <summary>
        /// Stops this data unit.
        /// </summary>
        public void Stop()
        {
            this.IsRunning = false;
        }

        /// <summary>
        /// Processes the input value if this data unit is running.
        /// </summary>
        /// <param name="sender">The sender of the event.</param>
        /// <param name="e">The event args of the event.</param>
        [DataInput]
        public void InputValue(object sender, ValueOutputEventArgs<string> e)
        {
            if (!this.IsRunning)
            {
                return;
            }

            this.ValueGenerated?.Invoke(this, new ValueOutputEventArgs<int>(Convert.ToInt32(e.Value)));
        }
    }
}
