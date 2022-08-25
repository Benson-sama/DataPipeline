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

        public bool IsRunning { get; private set; }

        /// <summary>
        /// Loads DSUs and DPUs from their corresponding folders on disk.
        /// </summary>
        public void LoadExtensions()
        {
            this.LoadDSUs();
            this.LoadDPUs();
        }

        public void Start()
        {
        }

        public void Stop()
        {
        }

        private void Link(ReflectedDataSourceUnit sourceUnit, ReflectedDataProcessingUnit processingUnit)
        {
            if (!this.DataSourceUnits.Contains(sourceUnit) || !this.DataProcessingUnits.Contains(processingUnit))
            {
                return;
            }

            Type delegateType = sourceUnit.ValueGeneratedEvent.EventHandlerType;
            MethodInfo handlerMethodInfo = processingUnit.ValueInputMethod;

            Delegate d = Delegate.CreateDelegate(delegateType, processingUnit.Instance, handlerMethodInfo);

            MethodInfo addHandler = sourceUnit.ValueGeneratedEvent.GetAddMethod();
            object[] addHandlerArgs = { d };
            addHandler.Invoke(sourceUnit.Instance, addHandlerArgs);
        }

        private void Link(ReflectedDataProcessingUnit firstProcessingUnit, ReflectedDataProcessingUnit secondProcessingUnit)
        {
            if (!this.DataProcessingUnits.Contains(firstProcessingUnit) || !this.DataProcessingUnits.Contains(secondProcessingUnit))
            {
                return;
            }

            Type delegateType = firstProcessingUnit.ValueProcessedEvent.EventHandlerType;
            MethodInfo handlerMethodInfo = secondProcessingUnit.ValueInputMethod;

            Delegate d = Delegate.CreateDelegate(delegateType, secondProcessingUnit.Instance, handlerMethodInfo);

            MethodInfo addHandler = firstProcessingUnit.ValueProcessedEvent.GetAddMethod();
            object[] addHandlerArgs = { d };
            addHandler.Invoke(firstProcessingUnit.Instance, addHandlerArgs);
        }

        /// <summary>
        /// Loads all data source units that are stored in the DSU folder in the application directory.
        /// </summary>
        private void LoadDSUs()
        {
            var dataSourceUnitFiles = Directory.EnumerateFiles("DSU", "*.dll", SearchOption.TopDirectoryOnly);
            dataSourceUnitFiles = dataSourceUnitFiles.Concat(Directory.EnumerateFiles("DSU", "*.exe", SearchOption.TopDirectoryOnly)).ToList();
            List<Assembly> loadedAssemblies = new List<Assembly>();

            foreach (var file in dataSourceUnitFiles)
            {
                try
                {
                    Assembly loadedAssembly = Assembly.LoadFrom(file);
                    loadedAssemblies.Add(loadedAssembly);
                }
                catch (Exception)
                {
                    // Could not load assembly.
                }
            }

            var dataSourceUnitTypes = loadedAssemblies.GetDataUnitTypes();

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
            var dataProcessingUnitFiles = Directory.EnumerateFiles("DPU", "*.dll", SearchOption.TopDirectoryOnly);
            dataProcessingUnitFiles = dataProcessingUnitFiles.Concat(Directory.EnumerateFiles("DPU", "*.exe", SearchOption.TopDirectoryOnly)).ToList();
            List<Assembly> loadedAssemblies = new List<Assembly>();

            foreach (var file in dataProcessingUnitFiles)
            {
                try
                {
                    Assembly loadedAssembly = Assembly.LoadFrom(file);
                    loadedAssemblies.Add(loadedAssembly);
                }
                catch (Exception)
                {
                    // Could not load assembly.
                }
            }

            var dataProcessingUnitTypes = loadedAssemblies.GetDataUnitTypes();

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