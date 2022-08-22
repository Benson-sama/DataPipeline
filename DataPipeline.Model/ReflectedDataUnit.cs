//---------------------------------------------------------------------
// <copyright file="ReflectedDataUnit.cs" company="FH Wiener Neustadt">
//     Copyright (c) FH Wiener Neustadt. All rights reserved.
// </copyright>
// <author>Benjamin Bogner</author>
// <summary>Contains the ReflectedDataUnit class.</summary>
//---------------------------------------------------------------------
namespace DataPipeline.Model
{
    using System;
    using System.Reflection;

    public abstract class ReflectedDataUnit
    {
        private Type type;

        private object instance;

        private MethodInfo startMethod;

        private MethodInfo stopMethod;

        public ReflectedDataUnit(Type dataUnitType)
        {
            this.Type = dataUnitType;
            this.Instance = Activator.CreateInstance(this.Type);
            this.StartMethod = this.Type.GetMethod("Start");
            this.StopMethod = this.Type.GetMethod("Stop");
        }

        public Type Type
        {
            get
            {
                return this.type;
            }

            private set
            {
                this.type = value ?? throw new ArgumentNullException(nameof(value), "The specified value cannot be null.");
            }
        }

        public object Instance
        {
            get
            {
                return this.instance;
            }

            private set
            {
                this.instance = value ?? throw new ArgumentNullException(nameof(value), "The specified value cannot be null.");
            }
        }

        private MethodInfo StartMethod
        {
            get
            {
                return this.startMethod;
            }

            set
            {
                this.startMethod = value ?? throw new ArgumentNullException(nameof(value), "The specified value cannot be null.");
            }
        }

        private MethodInfo StopMethod
        {
            get
            {
                return this.stopMethod;
            }

            set
            {
                this.stopMethod = value ?? throw new ArgumentNullException(nameof(value), "The specified value cannot be null.");
            }
        }

        public void Start()
        {
            this.StartMethod.Invoke(this.Instance, null);
        }

        public void Stop()
        {
            this.StopMethod.Invoke(this.Instance, null);
        }
    }
}
