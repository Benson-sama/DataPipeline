//------------------------------------------------------------------------------
// <copyright file="ConfigurationApplicationVM.cs" company="FH Wiener Neustadt">
//     Copyright (c) FH Wiener Neustadt. All rights reserved.
// </copyright>
// <author>Benjamin Bogner</author>
// <summary>Contains the ConfigurationApplicationVM class.</summary>
//------------------------------------------------------------------------------
namespace DataPipeline.ViewModel
{
    using System;
    using System.Collections.Generic;
    using System.Collections.ObjectModel;
    using System.IO;
    using System.Linq;
    using System.Reflection;
    using DataPipeline.Model;

    /// <summary>
    /// Represents the <see cref="ConfigurationApplicationVM"/> class.
    /// </summary>
    public class ConfigurationApplicationVM
    {
        /// <summary>
        /// The <see cref="ConfigurationApplication"/> of the <see cref="ConfigurationApplicationVM"/>.
        /// </summary>
        private readonly ConfigurationApplication configApp;

        // private ObservableCollection<ReflectedDataSourceUnit> dataSourceUnits;
        // private ObservableCollection<ReflectedDataProcessingUnit> dataProcessingUnits;
        private ObservableCollection<ReflectedDataVisualisationUnit> dataVisualisationUnits;

        /// <summary>
        /// Initialises a new instance of the <see cref="ConfigurationApplicationVM"/> class.
        /// </summary>
        public ConfigurationApplicationVM()
        {
            this.configApp = new ConfigurationApplication();
            this.DataVisualisationUnits = new ObservableCollection<ReflectedDataVisualisationUnit>();
        }

        public ObservableCollection<ReflectedDataVisualisationUnit> DataVisualisationUnits
        {
            get
            {
                return this.dataVisualisationUnits;
            }

            private set
            {
                this.dataVisualisationUnits = value ?? throw new ArgumentNullException(nameof(value), "The specified value cannot be null.");
            }
        }

        public bool IsRunning { get; private set; }

        /// <summary>
        /// Loads all extensions of the <see cref="ConfigurationApplication"/>.
        /// </summary>
        public void LoadExtensions()
        {
            this.configApp.LoadExtensions();
            this.LoadDVUs();

            this.configApp.Link(this.configApp.DataSourceUnits.First(), this.configApp.DataProcessingUnits.First());
            this.Link(this.configApp.DataProcessingUnits.First(), this.DataVisualisationUnits.First());
        }

        public void Link(ReflectedDataSourceUnit sourceUnit, ReflectedDataVisualisationUnit visualisationUnit)
        {
            if (!this.configApp.DataSourceUnits.Contains(sourceUnit) || !this.DataVisualisationUnits.Contains(visualisationUnit))
            {
                return;
            }

            Type delegateType = sourceUnit.ValueGeneratedEvent.EventHandlerType;
            MethodInfo handlerMethodInfo = visualisationUnit.ValueInputMethod;

            Delegate d = Delegate.CreateDelegate(delegateType, visualisationUnit.Instance, handlerMethodInfo);

            MethodInfo addHandler = sourceUnit.ValueGeneratedEvent.GetAddMethod();
            object[] addHandlerArgs = { d };
            addHandler.Invoke(sourceUnit.Instance, addHandlerArgs);
        }

        public void Link(ReflectedDataProcessingUnit processingUnit, ReflectedDataVisualisationUnit visualisationUnit)
        {
            if (!this.configApp.DataProcessingUnits.Contains(processingUnit) || !this.DataVisualisationUnits.Contains(visualisationUnit))
            {
                return;
            }

            Type delegateType = processingUnit.ValueProcessedEvent.EventHandlerType;
            MethodInfo handlerMethodInfo = visualisationUnit.ValueInputMethod;

            Delegate d = Delegate.CreateDelegate(delegateType, visualisationUnit.Instance, handlerMethodInfo);

            MethodInfo addHandler = processingUnit.ValueProcessedEvent.GetAddMethod();
            object[] addHandlerArgs = { d };
            addHandler.Invoke(processingUnit.Instance, addHandlerArgs);
        }

        public void Start()
        {
            if (this.IsRunning)
            {
                return;
            }

            foreach (var dVU in this.DataVisualisationUnits)
            {
                dVU.Start();
            }

            this.configApp.Start();
            this.IsRunning = true;
        }

        public void Stop()
        {
            if (!this.IsRunning)
            {
                return;
            }

            this.configApp.Stop();

            foreach (var dVU in this.DataVisualisationUnits)
            {
                dVU.Stop();
            }

            this.IsRunning = false;
        }

        private void LoadDVUs()
        {
            var dataVisualisationUnitFiles = Directory.EnumerateFiles("DVU", "*.dll", SearchOption.TopDirectoryOnly);
            dataVisualisationUnitFiles = dataVisualisationUnitFiles.Concat(Directory.EnumerateFiles("DVU", "*.exe", SearchOption.TopDirectoryOnly)).ToList();
            List<Assembly> loadedAssemblies = new List<Assembly>();

            foreach (var file in dataVisualisationUnitFiles)
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
                    this.DataVisualisationUnits.Add(new ReflectedDataVisualisationUnit(type));
                }
                catch (Exception)
                {
                    // Data Visualisation Unit does not meet the requirements.
                }
            }
        }
    }
}
