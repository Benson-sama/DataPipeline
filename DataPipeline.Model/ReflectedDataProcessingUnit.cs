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

    public class ReflectedDataProcessingUnit : ReflectedDataUnit
    {
        private EventInfo valueProcessedEvent;

        private MethodInfo valueInputMethod;

        public ReflectedDataProcessingUnit(Type dataProcessingUnitType) : base(dataProcessingUnitType)
        {
            this.ValueProcessedEvent = this.Type.GetEvents().First(x => x.GetCustomAttribute<DataOutputAttribute>() != null);
            this.ValueInputMethod = this.Type.GetMethods().First(x => x.GetCustomAttribute<DataInputAttribute>() != null);
        }

        public MethodInfo ValueInputMethod
        {
            get
            {
                return this.valueInputMethod;
            }

            private set
            {
                this.valueInputMethod = value ?? throw new ArgumentNullException(nameof(value), "The specified value cannot be null.");
            }
        }

        public EventInfo ValueProcessedEvent
        {
            get
            {
                return this.valueProcessedEvent;
            }

            private set
            {
                this.valueProcessedEvent = value ?? throw new ArgumentNullException(nameof(value), "The specified value cannot be null.");
            }
        }
    }
}