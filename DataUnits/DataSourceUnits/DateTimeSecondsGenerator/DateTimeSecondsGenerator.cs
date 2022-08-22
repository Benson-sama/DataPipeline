//-------------------------------------------------------------------------
// <copyright file="RandomNumberGenerator.cs" company="FH Wiener Neustadt">
//     Copyright (c) FH Wiener Neustadt. All rights reserved.
// </copyright>
// <author>Benjamin Bogner</author>
// <summary>Contains the RandomNumberGenerator class.</summary>
//-------------------------------------------------------------------------
namespace DateTimeSecondsGenerator
{
    using System;
    using DataPipeline.Model.Attributes;
    using DataPipeline.Model.Interfaces;
    using DataUnits;

    [DataUnitInformation(
        name: "Current DateTime seconds generator",
        Description = "Generates the current seconds component.",
        InputDatatype = typeof(void),
        InputDescription = "None.",
        OutputDatatype = typeof(int),
        OutputDescription = "A number representing the current seconds of time.")]
    public class DateTimeSecondsGenerator
    {
        [DataOutput]
        public event EventHandler<ValueOutputEventArgs<int>> ValueGenerated;

        public void Start()
        {
            ValueOutputEventArgs<int> valueOutputEventArgs = new ValueOutputEventArgs<int>(DateTime.Now.Second);
            this.ValueGenerated?.Invoke(this, valueOutputEventArgs);
        }

        public void Stop()
        {
        }
    }
}
