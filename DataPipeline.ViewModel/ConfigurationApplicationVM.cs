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

        private List<Type> loadedTypes;

        private ObservableCollection<ReflectedDataUnit> dataUnits;

        private ObservableCollection<ReflectedDataUnit> sourceDataUnits;

        private ObservableCollection<ReflectedDataUnit> destinationDataUnits;

        private ObservableCollection<ReflectedDataVisualisationUnit> dataVisualisationUnits;

        /// <summary>
        /// Initialises a new instance of the <see cref="ConfigurationApplicationVM"/> class.
        /// </summary>
        public ConfigurationApplicationVM()
        {
            this.configApp = new ConfigurationApplication();
            this.LoadedTypes = new List<Type>();
            this.DataUnits = new ObservableCollection<ReflectedDataUnit>();
            this.SourceDataUnits = new ObservableCollection<ReflectedDataUnit>();
            this.DestinationDataUnits = new ObservableCollection<ReflectedDataUnit>();
            this.DataVisualisationUnits = new ObservableCollection<ReflectedDataVisualisationUnit>();
        }

        public List<Type> LoadedTypes
        {
            get => this.loadedTypes;

            private set
            {
                this.loadedTypes = value ?? throw new ArgumentNullException(nameof(value), "The specified value cannot be null.");
            }
        }

        public ObservableCollection<ReflectedDataUnit> DataUnits
        {
            get => this.dataUnits;

            private set
            {
                this.dataUnits = value ?? throw new ArgumentNullException(nameof(value), "The specified value cannot be null.");
            }
        }

        public ObservableCollection<ReflectedDataUnit> SourceDataUnits
        {
            get => this.sourceDataUnits;

            private set
            {
                this.sourceDataUnits = value ?? throw new ArgumentNullException(nameof(value), "The specified value cannot be null.");
            }
        }

        public ObservableCollection<ReflectedDataVisualisationUnit> DataVisualisationUnits
        {
            get => this.dataVisualisationUnits;

            private set
            {
                this.dataVisualisationUnits = value ?? throw new ArgumentNullException(nameof(value), "The specified value cannot be null.");
            }
        }

        public ObservableCollection<ReflectedDataUnit> DestinationDataUnits
        {
            get => this.destinationDataUnits;

            private set
            {
                this.destinationDataUnits = value ?? throw new ArgumentNullException(nameof(value), "The specified value cannot be null.");
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

            // Set up collection for data unit information.
            this.DataUnits.Clear();
            this.configApp.DataSourceUnits.ForEach(x => this.DataUnits.Add(x));
            this.configApp.DataProcessingUnits.ForEach(x => this.DataUnits.Add(x));
            this.DataVisualisationUnits.ToList().ForEach(x => this.DataUnits.Add(x));

            // Set up collection for linking the first data unit.
            this.SourceDataUnits.Clear();
            this.configApp.DataSourceUnits.ForEach(x => this.SourceDataUnits.Add(x));
            this.configApp.DataProcessingUnits.ForEach(x => this.SourceDataUnits.Add(x));

            // Set up collection for linking the second data unit.
            this.DestinationDataUnits.Clear();
            this.configApp.DataProcessingUnits.ForEach(x => this.DestinationDataUnits.Add(x));
            this.DataVisualisationUnits.ToList().ForEach(x => this.DestinationDataUnits.Add(x));
        }

        public bool Link(ReflectedDataSourceUnit sourceUnit, ReflectedDataProcessingUnit processingUnit)
        {
            return this.configApp.Link(sourceUnit, processingUnit);
        }

        public bool Link(ReflectedDataProcessingUnit firstProcessingUnit, ReflectedDataProcessingUnit secondProcessingUnit)
        {
            return this.configApp.Link(firstProcessingUnit, secondProcessingUnit);
        }

        public bool Link(ReflectedDataSourceUnit sourceUnit, ReflectedDataVisualisationUnit visualisationUnit)
        {
            if (!this.configApp.DataSourceUnits.Contains(sourceUnit) || !this.DataVisualisationUnits.Contains(visualisationUnit))
            {
                return false;
            }

            try
            {
                Type delegateType = sourceUnit.ValueGeneratedEvent.EventHandlerType;
                MethodInfo handlerMethodInfo = visualisationUnit.ValueInputMethod;

                Delegate d = Delegate.CreateDelegate(delegateType, visualisationUnit.Instance, handlerMethodInfo);

                MethodInfo addHandler = sourceUnit.ValueGeneratedEvent.GetAddMethod();
                object[] addHandlerArgs = { d };
                addHandler.Invoke(sourceUnit.Instance, addHandlerArgs);

                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

        public bool Link(ReflectedDataProcessingUnit processingUnit, ReflectedDataVisualisationUnit visualisationUnit)
        {
            if (!this.configApp.DataProcessingUnits.Contains(processingUnit) || !this.DataVisualisationUnits.Contains(visualisationUnit))
            {
                return false;
            }

            try
            {
                Type delegateType = processingUnit.ValueProcessedEvent.EventHandlerType;
                MethodInfo handlerMethodInfo = visualisationUnit.ValueInputMethod;

                Delegate d = Delegate.CreateDelegate(delegateType, visualisationUnit.Instance, handlerMethodInfo);

                MethodInfo addHandler = processingUnit.ValueProcessedEvent.GetAddMethod();
                object[] addHandlerArgs = { d };
                addHandler.Invoke(processingUnit.Instance, addHandlerArgs);

                return true;
            }
            catch (Exception)
            {
                return false;
            }
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

            List<Assembly> loadedAssemblies = this.LoadAssemblies(dataVisualisationUnitFiles);
            var dataProcessingUnitTypes = loadedAssemblies.GetDataUnitTypes();

            foreach (var type in dataProcessingUnitTypes)
            {
                if (this.LoadedTypes.Contains(type))
                {
                    continue;
                }

                try
                {
                    this.DataVisualisationUnits.Add(new ReflectedDataVisualisationUnit(type));
                    this.LoadedTypes.Add(type);
                }
                catch (Exception)
                {
                    // Data Visualisation Unit does not meet the requirements.
                }
            }
        }

        private List<Assembly> LoadAssemblies(IEnumerable<string> dataVisualisationUnitFiles)
        {
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

            return loadedAssemblies;
        }
    }
}
