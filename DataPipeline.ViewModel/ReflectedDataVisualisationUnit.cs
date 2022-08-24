//----------------------------------------------------------------------------------
// <copyright file="ReflectedDataVisualisationUnit.cs" company="FH Wiener Neustadt">
//     Copyright (c) FH Wiener Neustadt. All rights reserved.
// </copyright>
// <author>Benjamin Bogner</author>
// <summary>Contains the ReflectedDataVisualisationUnit class.</summary>
//----------------------------------------------------------------------------------
namespace DataPipeline.ViewModel
{
    using System;
    using System.Linq;
    using System.Reflection;
    using DataPipeline.Model;
    using DataPipeline.Model.Attributes;

    public class ReflectedDataVisualisationUnit : ReflectedDataUnit
    {
        private MethodInfo valueInputMethod;

        public ReflectedDataVisualisationUnit(Type dataVisualisationUnitType) : base(dataVisualisationUnitType)
        {
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

    }
}
