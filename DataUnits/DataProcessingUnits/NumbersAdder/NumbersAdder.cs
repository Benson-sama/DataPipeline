//----------------------------------------------------------------
// <copyright file="NumbersAdder.cs" company="FH Wiener Neustadt">
//     Copyright (c) FH Wiener Neustadt. All rights reserved.
// </copyright>
// <author>Benjamin Bogner</author>
// <summary>Contains the NumbersAdder class.</summary>
//----------------------------------------------------------------
namespace NumbersAdder
{
    using System;
    using System.Collections.Generic;
    using DataPipeline.Model.Attributes;
    using DataUnits;

    /// <summary>
    /// Represents the <see cref="NumbersAdder"/> class.
    /// </summary>
    [DataUnitInformation(
        name: "Numbers Adder",
        Description = "Takes two numbers and adds them together.",
        InputDatatype = typeof(int),
        InputDescription = "A whole number.",
        OutputDatatype = typeof(int),
        OutputDescription = "The sum of the previous two inputs.")]
    public class NumbersAdder
    {
        /// <summary>
        /// The <see cref="Queue{T}"/> for input data.
        /// </summary>
        private Queue<int> values = new Queue<int>();

        /// <summary>
        /// Initialises a new instance of the <see cref="NumbersAdder"/> class.
        /// </summary>
        public NumbersAdder()
        {
            this.values = new Queue<int>();
        }

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
        /// Adds the value to the internal queue and processes it if this data unit is running.
        /// </summary>
        /// <param name="sender">The sender of the event.</param>
        /// <param name="e">The event args of the event.</param>
        [DataInput]
        public void InputValue(object sender, ValueOutputEventArgs<int> e)
        {
            this.values.Enqueue(e.Value);

            if (this.IsRunning)
            {
                this.ProcessQueue();
            }
        }

        /// <summary>
        /// Processes the internal data queue if at least two values in total arrived.
        /// </summary>
        private void ProcessQueue()
        {
            if (this.values.Count < 2)
            {
                return;
            }

            int firstValue = this.values.Dequeue();
            int secondValue = this.values.Dequeue();

            this.ValueGenerated?.Invoke(this, new ValueOutputEventArgs<int>(firstValue + secondValue));
        }
    }
}
