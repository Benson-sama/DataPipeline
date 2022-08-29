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
    using System.Linq;
    using System.Reflection;
    using DataPipeline.Model;
    using DataPipeline.Model.ReflectedDataUnits;

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

        private ObservableCollection<KeyValuePair<ReflectedDataUnit, ReflectedDataUnit>> connections;

        /// <summary>
        /// Initialises a new instance of the <see cref="ConfigurationApplicationVM"/> class.
        /// </summary>
        public ConfigurationApplicationVM()
        {
            this.configApp = new ConfigurationApplication();
            this.configApp.ConnectionsChanged += this.ConfigApp_ConnectionsChanged;
            this.LoadedTypes = new List<Type>();
            this.DataUnits = new ObservableCollection<ReflectedDataUnit>();
            this.SourceDataUnits = new ObservableCollection<ReflectedDataUnit>();
            this.DestinationDataUnits = new ObservableCollection<ReflectedDataUnit>();
            this.Connections = new ObservableCollection<KeyValuePair<ReflectedDataUnit, ReflectedDataUnit>>(this.configApp.Connections);
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

        public ObservableCollection<ReflectedDataUnit> DestinationDataUnits
        {
            get => this.destinationDataUnits;

            private set
            {
                this.destinationDataUnits = value ?? throw new ArgumentNullException(nameof(value), "The specified value cannot be null.");
            }
        }

        public IEnumerable<ReflectedDataVisualisationUnit> DataVisualisationUnits
        {
            get => this.configApp.DataVisualisationUnits;
        }

        public ObservableCollection<KeyValuePair<ReflectedDataUnit, ReflectedDataUnit>> Connections
        {
            get => this.connections;

            private set
            {
                this.connections = value ?? throw new ArgumentNullException(nameof(value), "The specified value cannot be null");
            }
        }

        public bool IsRunning { get; private set; }

        /// <summary>
        /// Loads all extensions of the <see cref="ConfigurationApplication"/>.
        /// </summary>
        public void LoadExtensions()
        {
            this.configApp.LoadExtensions();

            // Set up collection for data unit information.
            this.DataUnits.Clear();
            this.configApp.DataSourceUnits.ForEach(x => this.DataUnits.Add(x));
            this.configApp.DataProcessingUnits.ForEach(x => this.DataUnits.Add(x));
            this.configApp.DataVisualisationUnits.ToList().ForEach(x => this.DataUnits.Add(x));

            // Set up collection for linking the first data unit.
            this.SourceDataUnits.Clear();
            this.configApp.DataSourceUnits.ForEach(x => this.SourceDataUnits.Add(x));
            this.configApp.DataProcessingUnits.ForEach(x => this.SourceDataUnits.Add(x));

            // Set up collection for linking the second data unit.
            this.DestinationDataUnits.Clear();
            this.configApp.DataProcessingUnits.ForEach(x => this.DestinationDataUnits.Add(x));
            this.configApp.DataVisualisationUnits.ToList().ForEach(x => this.DestinationDataUnits.Add(x));
        }

        public bool Link(ReflectedDataSourceUnit sourceUnit, ReflectedDataProcessingUnit processingUnit)
        {
            return this.configApp.Link(sourceUnit, processingUnit);
        }

        public bool Link(ReflectedDataSourceUnit sourceUnit, ReflectedDataVisualisationUnit visualisationUnit)
        {
            return this.configApp.Link(sourceUnit, visualisationUnit);
        }

        public bool Link(ReflectedDataProcessingUnit firstProcessingUnit, ReflectedDataProcessingUnit secondProcessingUnit)
        {
            return this.configApp.Link(firstProcessingUnit, secondProcessingUnit);
        }

        public bool Link(ReflectedDataProcessingUnit processingUnit, ReflectedDataVisualisationUnit visualisationUnit)
        {
            return this.configApp.Link(processingUnit, visualisationUnit);
        }

        public void Start()
        {
            if (this.IsRunning)
            {
                return;
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
            this.IsRunning = false;
        }

        private void ConfigApp_ConnectionsChanged(object sender, ConnectionsChangedEventArgs e)
        {
            switch (e.Change)
            {
                case "Add":
                    this.Connections.Add(e.KeyValuePair);
                    break;
                case "Remove":
                    this.Connections.Remove(e.KeyValuePair);
                    break;
                default:
                    throw new ArgumentOutOfRangeException($"The specified change operation: \"{e.Change}\" is not supported.");
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
