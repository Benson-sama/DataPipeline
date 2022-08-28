//-------------------------------------------------------------------------------
// <copyright file="ReflectedDataProcessingUnit.cs" company="FH Wiener Neustadt">
//     Copyright (c) FH Wiener Neustadt. All rights reserved.
// </copyright>
// <author>Benjamin Bogner</author>
// <summary>Contains the ReflectedDataProcessingUnit class.</summary>
//-------------------------------------------------------------------------------
namespace DataPipeline.Model
{
    using System;
    using System.Linq;
    using System.Reflection;
    using DataPipeline.Model.Attributes;

    /// <summary>
    /// Represents the <see cref="ReflectedDataProcessingUnit"/> class.
    /// </summary>
    public class ReflectedDataProcessingUnit : ReflectedDataUnit
    {
        /// <summary>
        /// The <see cref="EventInfo"/> for the event that gets fired when a value got processed.
        /// </summary>
        private EventInfo valueProcessedEvent;

        /// <summary>
        /// The <see cref="MethodInfo"/> for the method that inputs data into the data unit.
        /// </summary>
        private MethodInfo valueInputMethod;

        /// <summary>
        /// Initialises a new instance of the <see cref="ReflectedDataProcessingUnit"/> class.
        /// </summary>
        /// <param name="dataProcessingUnitType">The <see cref="Type"/> of the data unit.</param>
        public ReflectedDataProcessingUnit(Type dataProcessingUnitType) : base(dataProcessingUnitType)
        {
            this.ValueProcessedEvent = this.Type.GetEvents().First(x => x.GetCustomAttribute<DataOutputAttribute>() != null);
            this.ValueInputMethod = this.Type.GetMethods().First(x => x.GetCustomAttribute<DataInputAttribute>() != null);
        }

        /// <summary>
        /// Gets the <see cref="MethodInfo"/> for the method that inputs data into the data unit.
        /// </summary>
        /// <value>The <see cref="MethodInfo"/> for the method that inputs data into the data unit.</value>
        public MethodInfo ValueInputMethod
        {
            get => this.valueInputMethod;

            private set
            {
                this.valueInputMethod = value ?? throw new ArgumentNullException(nameof(value), "The specified value cannot be null.");
            }
        }

        /// <summary>
        /// Gets the <see cref="EventInfo"/> for the event that gets fired when a value got processed.
        /// </summary>
        /// <value>The <see cref="EventInfo"/> for the event that gets fired when a value got processed.</value>
        public EventInfo ValueProcessedEvent
        {
            get => this.valueProcessedEvent;

            private set
            {
                this.valueProcessedEvent = value ?? throw new ArgumentNullException(nameof(value), "The specified value cannot be null.");
            }
        }

        public override void Accept(IReflectedDataUnitVisitor reflectedDataUnitVisitor)
        {
            reflectedDataUnitVisitor.Visit(this);
        }
    }
}
