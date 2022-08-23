//----------------------------------------------------------------------------
// <copyright file="ConfigurationApplication.cs" company="FH Wiener Neustadt">
//     Copyright (c) FH Wiener Neustadt. All rights reserved.
// </copyright>
// <author>Benjamin Bogner</author>
// <summary>Contains the ConfigurationApplication class.</summary>
//----------------------------------------------------------------------------
namespace DataPipeline.Model
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;
    using System.Reflection;

    /// <summary>
    /// Represents the <see cref="ConfigurationApplication"/> class.
    /// </summary>
    public class ConfigurationApplication
    {
        private List<ReflectedDataSourceUnit> dataSourceUnits;

        private List<ReflectedDataProcessingUnit> dataProcessingUnits;

        /// <summary>
        /// Initialises a new instance of the <see cref="ConfigurationApplication"/> class.
        /// </summary>
        public ConfigurationApplication()
        {
            this.DataSourceUnits = new List<ReflectedDataSourceUnit>();
            this.DataProcessingUnits = new List<ReflectedDataProcessingUnit>();
        }

        public List<ReflectedDataSourceUnit> DataSourceUnits
        {
            get
            {
                return this.dataSourceUnits;
            }

            private set
            {
                this.dataSourceUnits = value ?? throw new ArgumentNullException(nameof(value), "The specified value cannot be null.");
            }
        }

        public List<ReflectedDataProcessingUnit> DataProcessingUnits
        {
            get
            {
                return this.dataProcessingUnits;
            }

            private set
            {
                this.dataProcessingUnits = value ?? throw new ArgumentNullException(nameof(value), "The specified value cannot be null.");
            }
        }

        /// <summary>
        /// Loads DSUs and DPUs from their corresponding folders on disk.
        /// </summary>
        public void LoadExtensions()
        {
            this.LoadDSUs();
            this.LoadDPUs();

            this.TestLink();
        }

        private void TestLink()
        {
            var reflectedDataSourceUnit = DataSourceUnits.First();
            var reflectedDataProcessingUnit = DataProcessingUnits.First();

            Type tDelegate = reflectedDataSourceUnit.ValueGeneratedEvent.EventHandlerType;
            MethodInfo miHandler = reflectedDataProcessingUnit.ValueInputMethod;

            Delegate d = Delegate.CreateDelegate(tDelegate, reflectedDataProcessingUnit.Instance, miHandler);

            MethodInfo addHandler = reflectedDataSourceUnit.ValueGeneratedEvent.GetAddMethod();
            object[] addHandlerArgs = { d };
            addHandler.Invoke(reflectedDataSourceUnit.Instance, addHandlerArgs);
        }

        /// <summary>
        /// Connects two data units if it is possible.
        /// </summary>
        /// <param name="firstElement">The first data unit.</param>
        /// <param name="secondElement">The second data unit.</param>
        public void Link(int firstElement, int secondElement)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Loads all data source units that are stored in the DSU folder in the application directory.
        /// </summary>
        private void LoadDSUs()
        {
            var dataSourceUnitFiles = Directory.EnumerateFiles("DSU", "*", SearchOption.TopDirectoryOnly);
            List<Assembly> dataSourceUnitAssemblies = new List<Assembly>();

            foreach (var file in dataSourceUnitFiles)
            {
                try
                {
                    Assembly loadedAssembly = Assembly.LoadFrom(file);
                    dataSourceUnitAssemblies.Add(loadedAssembly);
                }
                catch (Exception)
                {
                    // Could not load assembly.
                }
            }

            var dataSourceUnitTypes = dataSourceUnitAssemblies.GetDataUnitTypes();

            foreach (var type in dataSourceUnitTypes)
            {
                try
                {
                    this.DataSourceUnits.Add(new ReflectedDataSourceUnit(type));
                }
                catch (Exception)
                {
                    // Data Source Unit does not meet the requirements.
                }
            }
        }

        /// <summary>
        /// Loads all data processing units that are stored in the DSU folder in the application directory.
        /// </summary>
        private void LoadDPUs()
        {
            var dataProcessingUnitFiles = Directory.EnumerateFiles("DPU", "*", SearchOption.TopDirectoryOnly);
            List<Assembly> dataProcessingUnitAssemblies = new List<Assembly>();

            foreach (var file in dataProcessingUnitFiles)
            {
                try
                {
                    Assembly loadedAssembly = Assembly.LoadFrom(file);
                    dataProcessingUnitAssemblies.Add(loadedAssembly);
                }
                catch (Exception)
                {
                    // Could not load assembly.
                }
            }

            var dataProcessingUnitTypes = dataProcessingUnitAssemblies.GetDataUnitTypes();

            foreach (var type in dataProcessingUnitTypes)
            {
                try
                {
                    this.DataProcessingUnits.Add(new ReflectedDataProcessingUnit(type));
                }
                catch (Exception)
                {
                    // Data Processing Unit does not meet the requirements.
                }
            }
        }
    }
}