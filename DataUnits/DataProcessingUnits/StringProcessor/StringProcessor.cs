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

    [DataUnitInformation(
        name: "String processor",
        Description = "Processes a single string and outputs a number.",
        InputDatatype = typeof(string),
        InputDescription = "A string with no constraint.",
        OutputDatatype = typeof(int),
        OutputDescription = "A whole number.")]
    public class StringProcessor
    {
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
