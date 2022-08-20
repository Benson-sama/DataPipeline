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

    [AttributeUsage(AttributeTargets.Class)]
    public class DataUnitInformationAttribute : Attribute
    {
        private string name;

        private string description;

        private Type inputDataType;

        private string inputDescription;

        private Type outputDataType;

        private string outputDescription;

        public DataUnitInformationAttribute(string name)
        {
            this.Name = name;
        }

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
