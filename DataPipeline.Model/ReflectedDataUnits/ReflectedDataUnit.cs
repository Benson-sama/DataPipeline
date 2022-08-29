//---------------------------------------------------------------------
// <copyright file="ReflectedDataUnit.cs" company="FH Wiener Neustadt">
//     Copyright (c) FH Wiener Neustadt. All rights reserved.
// </copyright>
// <author>Benjamin Bogner</author>
// <summary>Contains the ReflectedDataUnit class.</summary>
//---------------------------------------------------------------------
namespace DataPipeline.Model.ReflectedDataUnits
{
    using System;
    using System.Reflection;
    using DataPipeline.Model.Attributes;

    /// <summary>
    /// Represents the <see cref="ReflectedDataUnit"/> class.
    /// </summary>
    public abstract class ReflectedDataUnit
    {
        /// <summary>
        /// The attribute of the data unit.
        /// </summary>
        private DataUnitInformationAttribute attribute;

        /// <summary>
        /// The <see cref="System.Type"/> of the data unit.
        /// </summary>
        private Type type;

        /// <summary>
        /// The instance of the data unit.
        /// </summary>
        private object instance;

        /// <summary>
        /// The <see cref="MethodInfo"/> used to start the data unit.
        /// </summary>
        private MethodInfo startMethod;

        /// <summary>
        /// The <see cref="MethodInfo"/> used to stop the data unit.
        /// </summary>
        private MethodInfo stopMethod;

        /// <summary>
        /// Initialises a new instance of the <see cref="ReflectedDataUnit"/> class.
        /// </summary>
        /// <param name="dataUnitType">The <see cref="System.Type"/> of the data unit.</param>
        public ReflectedDataUnit(Type dataUnitType)
        {
            this.Attribute = dataUnitType.GetCustomAttribute<DataUnitInformationAttribute>();
            this.Type = dataUnitType;
            this.Instance = Activator.CreateInstance(this.Type);
            this.StartMethod = this.Type.GetMethod("Start");
            this.StopMethod = this.Type.GetMethod("Stop");
        }

        /// <summary>
        /// Gets the <see cref="DataUnitInformationAttribute"/> of the data unit.
        /// </summary>
        /// <value>The <see cref="DataUnitInformationAttribute"/> of the data unit.</value>
        public DataUnitInformationAttribute Attribute
        {
            get => this.attribute;

            private set => this.attribute = value ?? throw new ArgumentNullException(nameof(value), "The specified value cannot be null.");
        }

        /// <summary>
        /// Gets the <see cref="System.Type"/> of the data unit.
        /// </summary>
        /// <value>The <see cref="System.Type"/> of the data unit.</value>
        public Type Type
        {
            get => this.type;

            private set => this.type = value ?? throw new ArgumentNullException(nameof(value), "The specified value cannot be null.");
        }

        /// <summary>
        /// Gets the instance of the data unit.
        /// </summary>
        /// <value>The instance of the data unit.</value>
        public object Instance
        {
            get => this.instance;

            private set => this.instance = value ?? throw new ArgumentNullException(nameof(value), "The specified value cannot be null.");
        }

        /// <summary>
        /// Gets or sets the <see cref="MethodInfo"/> used to start the data unit.
        /// </summary>
        /// <value>The <see cref="MethodInfo"/> used to start the data unit.</value>
        private MethodInfo StartMethod
        {
            get => this.startMethod;

            set => this.startMethod = value ?? throw new ArgumentNullException(nameof(value), "The specified value cannot be null.");
        }

        /// <summary>
        /// Gets or sets the <see cref="MethodInfo"/> used to stop the data unit.
        /// </summary>
        /// <value>The <see cref="MethodInfo"/> used to stop the data unit.</value>
        private MethodInfo StopMethod
        {
            get => this.stopMethod;

            set => this.stopMethod = value ?? throw new ArgumentNullException(nameof(value), "The specified value cannot be null.");
        }

        /// <summary>
        /// Invokes the underlying <seealso cref="StartMethod"/> and thereby starts the data unit.
        /// </summary>
        public void Start()
        {
            this.StartMethod.Invoke(this.Instance, null);
        }

        /// <summary>
        /// Invokes the underlying <seealso cref="StopMethod"/> and thereby stops the data unit.
        /// </summary>
        public void Stop()
        {
            this.StopMethod.Invoke(this.Instance, null);
        }

        /// <summary>
        /// Accepts a <see cref="IReflectedDataUnitVisitor"/> that performs specific actions on this <see cref="ReflectedDataUnit"/>.
        /// </summary>
        /// <param name="reflectedDataUnitVisitor">The accepted <see cref="IReflectedDataUnitVisitor"/>.</param>
        public abstract void Accept(IReflectedDataUnitVisitor reflectedDataUnitVisitor);

        /// <summary>
        /// Creates a string representation of this <see cref="ReflectedDataUnit"/>.
        /// </summary>
        /// <returns>The name of this <see cref="ReflectedDataUnit"/>.</returns>
        public override string ToString()
        {
            return this.Attribute.Name;
        }
    }
}
