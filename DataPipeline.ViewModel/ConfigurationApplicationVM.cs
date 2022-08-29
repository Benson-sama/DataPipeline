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

        /// <summary>
        /// The data units of the <see cref="ConfigurationApplicationVM"/>.
        /// </summary>
        private ObservableCollection<ReflectedDataUnit> dataUnits;

        /// <summary>
        /// The data units that can output values.
        /// </summary>
        private ObservableCollection<ReflectedDataUnit> sourceDataUnits;

        /// <summary>
        /// The data units that can take input values.
        /// </summary>
        private ObservableCollection<ReflectedDataUnit> destinationDataUnits;

        /// <summary>
        /// The created connections between data units.
        /// </summary>
        private ObservableCollection<KeyValuePair<ReflectedDataUnit, ReflectedDataUnit>> connections;

        /// <summary>
        /// Initialises a new instance of the <see cref="ConfigurationApplicationVM"/> class.
        /// </summary>
        public ConfigurationApplicationVM()
        {
            this.configApp = new ConfigurationApplication();
            this.configApp.ConnectionsChanged += this.ConfigApp_ConnectionsChanged;
            this.DataUnits = new ObservableCollection<ReflectedDataUnit>();
            this.SourceDataUnits = new ObservableCollection<ReflectedDataUnit>();
            this.DestinationDataUnits = new ObservableCollection<ReflectedDataUnit>();
            this.Connections = new ObservableCollection<KeyValuePair<ReflectedDataUnit, ReflectedDataUnit>>(this.configApp.Connections);
        }

        /// <summary>
        /// Gets the loaded <see cref="Type"/> collection of the underlying <see cref="ConfigurationApplication"/>.
        /// </summary>
        /// <value>The loaded <see cref="Type"/> collection of the underlying <see cref="ConfigurationApplication"/>.</value>
        public List<Type> LoadedTypes => this.configApp.LoadedTypes;

        /// <summary>
        /// Gets the <see cref="ReflectedDataUnit"/> instances of this <see cref="ConfigurationApplicationVM"/>.
        /// </summary>
        /// <value>The <see cref="ReflectedDataUnit"/> instances of this <see cref="ConfigurationApplicationVM"/>.</value>
        public ObservableCollection<ReflectedDataUnit> DataUnits
        {
            get => this.dataUnits;

            private set
            {
                this.dataUnits = value ?? throw new ArgumentNullException(nameof(value), "The specified value cannot be null.");
            }
        }

        /// <summary>
        /// Gets the <see cref="ReflectedDataUnit"/> instances of this <see cref="ConfigurationApplicationVM"/> that can output values.
        /// </summary>
        /// <value>The <see cref="ReflectedDataUnit"/> instances of this <see cref="ConfigurationApplicationVM"/> that can output values.</value>
        public ObservableCollection<ReflectedDataUnit> SourceDataUnits
        {
            get => this.sourceDataUnits;

            private set
            {
                this.sourceDataUnits = value ?? throw new ArgumentNullException(nameof(value), "The specified value cannot be null.");
            }
        }

        /// <summary>
        /// Gets the <see cref="ReflectedDataUnit"/> instances of this <see cref="ConfigurationApplicationVM"/> that can take input values.
        /// </summary>
        /// <value>The <see cref="ReflectedDataUnit"/> instances of this <see cref="ConfigurationApplicationVM"/> that can take input values.</value>
        public ObservableCollection<ReflectedDataUnit> DestinationDataUnits
        {
            get => this.destinationDataUnits;

            private set
            {
                this.destinationDataUnits = value ?? throw new ArgumentNullException(nameof(value), "The specified value cannot be null.");
            }
        }

        /// <summary>
        /// Gets the <see cref="ReflectedDataVisualisationUnit"/> collection
        /// from the underlying <see cref="ConfigurationApplication"/>.
        /// </summary>
        /// <value>The <see cref="ReflectedDataVisualisationUnit"/> collection
        /// from the underlying <see cref="ConfigurationApplication"/>.</value>
        public IEnumerable<ReflectedDataVisualisationUnit> DataVisualisationUnits
        {
            get => this.configApp.DataVisualisationUnits;
        }

        /// <summary>
        /// Gets the created connections between data units.
        /// </summary>
        /// <value>The created connections between data units.</value>
        public ObservableCollection<KeyValuePair<ReflectedDataUnit, ReflectedDataUnit>> Connections
        {
            get => this.connections;

            private set
            {
                this.connections = value ?? throw new ArgumentNullException(nameof(value), "The specified value cannot be null");
            }
        }

        /// <summary>
        /// Gets a value indicating whether the underlying <see cref="ConfigurationApplication"/> is running.
        /// </summary>
        /// <value>The value indicating whether the underlying <see cref="ConfigurationApplication"/> is running.</value>
        public bool IsRunning => this.configApp.IsRunning;

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

        public bool Link(ReflectedDataUnit firstDU, ReflectedDataUnit secondDU)
        {
            return this.configApp.Link(firstDU, secondDU);
        }

        /// <summary>
        /// Starts the underlying <see cref="ConfigurationApplication"/>.
        /// </summary>
        public void Start()
        {
            this.configApp.Start();
        }

        /// <summary>
        /// Stops the underlying <see cref="ConfigurationApplication"/>.
        /// </summary>
        public void Stop()
        {
            this.configApp.Stop();
        }

        /// <summary>
        /// Updates the collection of created connections of this <see cref="ConfigurationApplicationVM"/>.
        /// </summary>
        /// <param name="sender">The sender of the event.</param>
        /// <param name="e">The arguments of the event.</param>
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
    }
}
