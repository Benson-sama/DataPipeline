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

    [DataUnitInformation(
        name:"Numbers Adder",
        Description = "Takes two numbers and adds them together.",
        InputDatatype = typeof(int),
        InputDescription = "A whole number.",
        OutputDatatype = typeof(int),
        OutputDescription = "The sum of the previous two inputs.")]
    public class NumbersAdder
    {
        Queue<int> values = new Queue<int>();

        public NumbersAdder()
        {
            this.values = new Queue<int>();
        }

        public bool IsRunning { get; private set; }

        [DataOutput]
        public event EventHandler<ValueOutputEventArgs<int>> ValueGenerated;

        public void Start()
        {
            this.IsRunning = true;
        }

        public void Stop()
        {
            this.IsRunning = false;
        }

        [DataInput]
        public void InputValue(object sender, ValueOutputEventArgs<int> e)
        {
            this.values.Enqueue(e.Value);

            if (this.IsRunning)
            {
                this.ProcessQueue();
            }
        }

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
