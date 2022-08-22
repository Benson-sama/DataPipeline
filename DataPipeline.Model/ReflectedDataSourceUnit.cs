//---------------------------------------------------------------------------
// <copyright file="ReflectedDataSourceUnit.cs" company="FH Wiener Neustadt">
//     Copyright (c) FH Wiener Neustadt. All rights reserved.
// </copyright>
// <author>Benjamin Bogner</author>
// <summary>Contains the ReflectedDataSourceUnit class.</summary>
//---------------------------------------------------------------------------
namespace DataPipeline.Model
{
    using System;
    using System.Linq;
    using System.Reflection;
    using DataPipeline.Model.Attributes;

    public class ReflectedDataSourceUnit : ReflectedDataUnit
    {
        private EventInfo valueGeneratedEvent;

        public ReflectedDataSourceUnit(Type dataSourceUnitType) : base(dataSourceUnitType)
        {
            this.ValueGeneratedEvent = this.Type.GetEvents().First(x => x.GetCustomAttribute<DataOutputAttribute>() != null);
        }

        public EventInfo ValueGeneratedEvent
        {
            get
            {
                return this.valueGeneratedEvent;
            }

            private set
            {
                this.valueGeneratedEvent = value ?? throw new ArgumentNullException(nameof(value), "The specified value cannot be null.");
            }
        }
    }
}