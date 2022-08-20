//----------------------------------------------------------------------
// <copyright file="DataInputAttribute.cs" company="FH Wiener Neustadt">
//     Copyright (c) FH Wiener Neustadt. All rights reserved.
// </copyright>
// <author>Benjamin Bogner</author>
// <summary>Contains the DataInputAttribute class.</summary>
//----------------------------------------------------------------------
namespace DataPipeline.Model.Attributes
{
    using System;

    [AttributeUsage(AttributeTargets.Method)]
    public class DataInputAttribute : Attribute
    {
    }
}
