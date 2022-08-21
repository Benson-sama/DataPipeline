//---------------------------------------------------------------------------
// <copyright file="StringToNumberProcessor.cs" company="FH Wiener Neustadt">
//     Copyright (c) FH Wiener Neustadt. All rights reserved.
// </copyright>
// <author>Benjamin Bogner</author>
// <summary>Contains the StringToNumberProcessor class.</summary>
//---------------------------------------------------------------------------
namespace StringToNumberProcessor
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using DataPipeline.Model.Attributes;
    using DataUnits;

    [DataUnitInformation(
        name: "String to number processor",
        Description = "Processes one string and outputs a number.",
        InputDatatype = typeof(string),
        InputDescription = "A string with no constraint.",
        OutputDatatype = typeof(int),
        OutputDescription = "A whole number.")]
    public class StringToNumberProcessor
    {
        Queue<string> values = new Queue<string>();

        public StringToNumberProcessor()
        {
            this.values = new Queue<string>();
        }

        public bool IsRunning { get; private set; }

        [DataOutput]
        public event EventHandler<ValueOutputEventArgs<int>> ValueGenerated;

        public void Start()
        {
            this.IsRunning = true;

            while (this.values.Any())
            {
                this.ValueGenerated?.Invoke(this, new ValueOutputEventArgs<int>(Convert.ToInt32(this.values.Dequeue())));
            }
        }

        public void Stop()
        {
            this.IsRunning = false;
        }

        [DataInput]
        public void InputValue(object sender, ValueOutputEventArgs<string> e)
        {
            if (!this.IsRunning)
            {
                this.values.Enqueue(e.Value);
                return;
            }

            this.ValueGenerated?.Invoke(this, new ValueOutputEventArgs<int>(Convert.ToInt32(e.Value)));
        }
    }
}
