//--------------------------------------------------------------------------------
// <copyright file="DataUnitInformationAttribute.cs" company="FH Wiener Neustadt">
//     Copyright (c) FH Wiener Neustadt. All rights reserved.
// </copyright>
// <author>Benjamin Bogner</author>
// <summary>Contains the DataUnitInformationAttribute class.</summary>
//--------------------------------------------------------------------------------
namespace DataPipeline.Model.Attributes
{
    using System;

    /// <summary>
    /// Represents the <see cref="DataUnitInformationAttribute"/> class.
    /// It is used to provide various information about a data unit.
    /// </summary>
    [AttributeUsage(AttributeTargets.Class)]
    public class DataUnitInformationAttribute : Attribute
    {
        /// <summary>
        /// The name of the data unit.
        /// </summary>
        private string name;

        /// <summary>
        /// The description of the data unit.
        /// </summary>
        private string description;

        /// <summary>
        /// The <see cref="Type"/> for the data input.
        /// </summary>
        private Type inputDataType;

        /// <summary>
        /// The description for the data input.
        /// </summary>
        private string inputDescription;

        /// <summary>
        /// The <see cref="Type"/> for the data output.
        /// </summary>
        private Type outputDataType;

        /// <summary>
        /// The description for the data output.
        /// </summary>
        private string outputDescription;

        /// <summary>
        /// Initialises a new instance of the <see cref="DataUnitInformationAttribute"/> class.
        /// By trusting the user of this class, the parameters for this constructor have been
        /// reduced to only the name. It is advised to pass all remaining parameters via advanced initialising.
        /// </summary>
        /// <param name="name">The name for this <see cref="DataUnitInformationAttribute"/>.</param>
        public DataUnitInformationAttribute(string name)
        {
            this.Name = name;
        }

        /// <summary>
        /// Gets or sets the name of the data unit.
        /// </summary>
        /// <value>The name of the data unit.</value>
        public string Name
        {
            get
            {
                return this.name;
            }

            set
            {
                if (string.IsNullOrEmpty(value))
                {
                    throw new ArgumentOutOfRangeException(nameof(value), "The specified value cannot be null or empty.");
                }

                this.name = value;
            }
        }

        /// <summary>
        /// Gets or sets the description of the data unit.
        /// </summary>
        /// <value>The description of the data unit.</value>
        public string Description
        {
            get
            {
                return this.description;
            }

            set
            {
                if (string.IsNullOrEmpty(value))
                {
                    throw new ArgumentOutOfRangeException(nameof(value), "The specified value cannot be null or empty.");
                }

                this.description = value;
            }
        }

        /// <summary>
        /// Gets or sets the <see cref="Type"/> for the input data.
        /// </summary>
        /// <value>The <see cref="Type"/> for the input data.</value>
        public Type InputDatatype
        {
            get
            {
                return this.inputDataType;
            }

            set
            {
                this.inputDataType = value ?? throw new ArgumentNullException(nameof(value), "The specified value cannot be null.");
            }
        }

        /// <summary>
        /// Gets or sets the description for the input data.
        /// </summary>
        /// <value>The description for the input data.</value>
        public string InputDescription
        {
            get
            {
                return this.inputDescription;
            }

            set
            {
                if (string.IsNullOrEmpty(value))
                {
                    throw new ArgumentOutOfRangeException(nameof(value), "The specified value cannot be null or empty.");
                }

                this.inputDescription = value;
            }
        }

        /// <summary>
        /// Gets or sets the <see cref="Type"/> for the output data.
        /// </summary>
        /// <value>The <see cref="Type"/> for the output data.</value>
        public Type OutputDatatype
        {
            get
            {
                return this.outputDataType;
            }

            set
            {
                this.outputDataType = value ?? throw new ArgumentNullException(nameof(value), "The specified value cannot be null.");
            }
        }

        /// <summary>
        /// Gets or sets the description for the output data.
        /// </summary>
        /// <value>The description for the output data.</value>
        public string OutputDescription
        {
            get
            {
                return this.outputDescription;
            }

            set
            {
                if (string.IsNullOrEmpty(value))
                {
                    throw new ArgumentOutOfRangeException(nameof(value), "The specified value cannot be null or empty.");
                }

                this.outputDescription = value;
            }
        }
    }
}
