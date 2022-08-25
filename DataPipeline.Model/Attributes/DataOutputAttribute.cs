//-----------------------------------------------------------------------
// <copyright file="DataOutputAttribute.cs" company="FH Wiener Neustadt">
//     Copyright (c) FH Wiener Neustadt. All rights reserved.
// </copyright>
// <author>Benjamin Bogner</author>
// <summary>Contains the DataOutputAttribute class.</summary>
//-----------------------------------------------------------------------
namespace DataPipeline.Model.Attributes
{
    using System;

    /// <summary>
    /// Represents the <see cref="DataOutputAttribute"/> class.
    /// It indicates that the event is the output of the data unit.
    /// </summary>
    [AttributeUsage(AttributeTargets.Event)]
    public class DataOutputAttribute : Attribute
    {
    }
}
