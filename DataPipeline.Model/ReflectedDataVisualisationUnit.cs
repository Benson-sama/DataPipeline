//----------------------------------------------------------------------------------
// <copyright file="ReflectedDataVisualisationUnit.cs" company="FH Wiener Neustadt">
//     Copyright (c) FH Wiener Neustadt. All rights reserved.
// </copyright>
// <author>Benjamin Bogner</author>
// <summary>Contains the ReflectedDataVisualisationUnit class.</summary>
//----------------------------------------------------------------------------------
namespace DataPipeline.Model
{
    using System;
    using System.Linq;
    using System.Reflection;
    using DataPipeline.Model.Attributes;

    /// <summary>
    /// Represents the <see cref="ReflectedDataVisualisationUnit"/> class.
    /// </summary>
    public class ReflectedDataVisualisationUnit : ReflectedDataUnit
    {
        /// <summary>
        /// The <see cref="MethodInfo"/> for the method that inputs data into the data unit.
        /// </summary>
        private MethodInfo valueInputMethod;

        /// <summary>
        /// Initialises a new instance of the <see cref="ReflectedDataVisualisationUnit"/> class.
        /// </summary>
        /// <param name="dataVisualisationUnitType">The <see cref="Type"/> of the data unit.</param>
        public ReflectedDataVisualisationUnit(Type dataVisualisationUnitType) : base(dataVisualisationUnitType)
        {
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

        public override void Accept(IReflectedDataUnitVisitor reflectedDataUnitVisitor)
        {
            reflectedDataUnitVisitor.Visit(this);
        }
    }
}
