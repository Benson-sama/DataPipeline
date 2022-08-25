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

    /// <summary>
    /// Represents the <see cref="ReflectedDataSourceUnit"/> class.
    /// </summary>
    public class ReflectedDataSourceUnit : ReflectedDataUnit
    {
        /// <summary>
        /// The <see cref="EventInfo"/> for the event that gets fired when a value got generated.
        /// </summary>
        private EventInfo valueGeneratedEvent;

        /// <summary>
        /// Initialises a new instance of the <see cref="ReflectedDataSourceUnit"/> class.
        /// </summary>
        /// <param name="dataSourceUnitType">The <see cref="Type"/> of the data source unit.</param>
        public ReflectedDataSourceUnit(Type dataSourceUnitType) : base(dataSourceUnitType)
        {
            this.ValueGeneratedEvent = this.Type.GetEvents().First(x => x.GetCustomAttribute<DataOutputAttribute>() != null);
        }

        /// <summary>
        /// Gets the <see cref="EventInfo"/> for the event that gets fired when a value got generated.
        /// </summary>
        /// <value>The <see cref="EventInfo"/> for the event that gets fired when a value got generated.</value>
        public EventInfo ValueGeneratedEvent
        {
            get => this.valueGeneratedEvent;

            private set
            {
                this.valueGeneratedEvent = value ?? throw new ArgumentNullException(nameof(value), "The specified value cannot be null.");
            }
        }
    }
}
