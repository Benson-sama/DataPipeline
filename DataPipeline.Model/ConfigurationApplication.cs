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
    using DataPipeline.Model.ReflectedDataUnits;

    /// <summary>
    /// Represents the <see cref="ConfigurationApplication"/> class.
    /// </summary>
    public class ConfigurationApplication
    {
        /// <summary>
        /// The loaded types of this <see cref="ConfigurationApplication"/>.
        /// </summary>
        private List<Type> loadedTypes;

        /// <summary>
        /// The activated data source units of this <see cref="ConfigurationApplication"/>.
        /// </summary>
        private List<ReflectedDataSourceUnit> dataSourceUnits;

        /// <summary>
        /// The activated data processing units of this <see cref="ConfigurationApplication"/>.
        /// </summary>
        private List<ReflectedDataProcessingUnit> dataProcessingUnits;

        /// <summary>
        /// The activated data visualisation units of this <see cref="ConfigurationApplication"/>.
        /// </summary>
        private List<ReflectedDataVisualisationUnit> dataVisualisationUnits;

        /// <summary>
        /// The created connections between data units.
        /// </summary>
        private Dictionary<ReflectedDataUnit, ReflectedDataUnit> connections;

        /// <summary>
        /// Initialises a new instance of the <see cref="ConfigurationApplication"/> class.
        /// </summary>
        public ConfigurationApplication()
        {
            this.LoadedTypes = new List<Type>();
            this.DataSourceUnits = new List<ReflectedDataSourceUnit>();
            this.DataProcessingUnits = new List<ReflectedDataProcessingUnit>();
            this.DataVisualisationUnits = new List<ReflectedDataVisualisationUnit>();
            this.Connections = new Dictionary<ReflectedDataUnit, ReflectedDataUnit>();
        }

        /// <summary>
        /// The event that gets fired when data units got connected to or disconnected from each other.
        /// </summary>
        public event EventHandler<ConnectionsChangedEventArgs> ConnectionsChanged;

        /// <summary>
        /// Gets the loaded types of this <see cref="ConfigurationApplication"/>.
        /// </summary>
        /// <value>The loaded types of this <see cref="ConfigurationApplication"/>.</value>
        public List<Type> LoadedTypes
        {
            get => this.loadedTypes;

            private set
            {
                this.loadedTypes = value ?? throw new ArgumentNullException(nameof(value), "The specified value cannot be null.");
            }
        }

        /// <summary>
        /// Gets the collection of activated <see cref="ReflectedDataSourceUnit"/> instances.
        /// </summary>
        /// <value>The collection of activated <see cref="ReflectedDataSourceUnit"/> instances.</value>
        public List<ReflectedDataSourceUnit> DataSourceUnits
        {
            get => this.dataSourceUnits;

            private set
            {
                this.dataSourceUnits = value ?? throw new ArgumentNullException(nameof(value), "The specified value cannot be null.");
            }
        }

        /// <summary>
        /// Gets the collection of activated <see cref="ReflectedDataProcessingUnit"/> instances.
        /// </summary>
        /// <value>The collection of activated <see cref="ReflectedDataProcessingUnit"/> instances.</value>
        public List<ReflectedDataProcessingUnit> DataProcessingUnits
        {
            get => this.dataProcessingUnits;

            private set
            {
                this.dataProcessingUnits = value ?? throw new ArgumentNullException(nameof(value), "The specified value cannot be null.");
            }
        }

        /// <summary>
        /// Gets the collection of activated <see cref="ReflectedDataVisualisationUnit"/> instances.
        /// </summary>
        /// <value>The collection of activated <see cref="ReflectedDataVisualisationUnit"/> instances.</value>
        public List<ReflectedDataVisualisationUnit> DataVisualisationUnits
        {
            get => this.dataVisualisationUnits;

            private set
            {
                this.dataVisualisationUnits = value ?? throw new ArgumentNullException(nameof(value), "The specified value cannot be null.");
            }
        }

        /// <summary>
        /// Gets the created connections between <see cref="ReflectedDataUnit"/> instances.
        /// </summary>
        /// <value>The created connections between <see cref="ReflectedDataUnit"/> instances.</value>
        public Dictionary<ReflectedDataUnit, ReflectedDataUnit> Connections
        {
            get => this.connections;

            private set
            {
                this.connections = value ?? throw new ArgumentNullException(nameof(value), "The specified value cannot be null.");
            }
        }

        /// <summary>
        /// Gets a value indicating whether this <see cref="ConfigurationApplication"/> is running.
        /// </summary>
        /// <value>The value indicating whether this <see cref="ConfigurationApplication"/> is running.</value>
        public bool IsRunning { get; private set; }

        /// <summary>
        /// Loads DSUs and DPUs from their corresponding folders on disk.
        /// </summary>
        public void LoadExtensions()
        {
            this.LoadDSUs();
            this.LoadDPUs();
            this.LoadDVUs();
        }

        /// <summary>
        /// Starts all connected <see cref="ReflectedDataUnit"/> instances in the order DVU, DPU and DSU.
        /// </summary>
        public void Start()
        {
            if (this.IsRunning)
            {
                return;
            }

            foreach (var dVU in this.DataVisualisationUnits)
            {
                if (this.Connections.ContainsValue(dVU))
                {
                    dVU.Start();
                }
            }

            foreach (var dPU in this.DataProcessingUnits)
            {
                if (this.Connections.ContainsKey(dPU) || this.Connections.ContainsValue(dPU))
                {
                    dPU.Start();
                }
            }

            foreach (var dSU in this.DataSourceUnits)
            {
                if (this.Connections.ContainsKey(dSU) || this.Connections.ContainsValue(dSU))
                {
                    dSU.Start();
                }
            }

            this.IsRunning = true;
        }

        /// <summary>
        /// Stops all <see cref="ReflectedDataUnit"/> instances in the order DSU, DPU and DVU.
        /// </summary>
        public void Stop()
        {
            if (!this.IsRunning)
            {
                return;
            }

            this.DataSourceUnits.ForEach(x => x.Stop());
            this.DataProcessingUnits.ForEach(x => x.Stop());
            this.DataVisualisationUnits.ForEach(x => x.Stop());
            this.IsRunning = false;
        }

        /// <summary>
        /// Determines the actual types of the data units and invokes the corresponding link method.
        /// </summary>
        /// <param name="firstDU">The source data unit.</param>
        /// <param name="secondDU">The destination data unit.</param>
        /// <returns>The value indicating whether or not the link attempt was successful.</returns>
        public bool Link(ReflectedDataUnit firstDU, ReflectedDataUnit secondDU)
        {
            if (this.IsRunning || this.Connections.ContainsKey(firstDU))
            {
                return false;
            }

            ReflectedDataUnitSelector firstSelector = new ReflectedDataUnitSelector();
            firstDU.Accept(firstSelector);
            ReflectedDataUnitSelector secondSelector = new ReflectedDataUnitSelector();
            secondDU.Accept(secondSelector);

            // Source unit cannot be a DVU and destination unit cannot be a DSU.
            if (firstSelector.ReflectedDVU != null || secondSelector.ReflectedDSU != null)
            {
                return false;
            }

            // Link based on combination of actual data units.
            if (firstSelector.ReflectedDSU != null)
            {
                return secondSelector.ReflectedDPU != null
                    ? this.Link(firstSelector.ReflectedDSU, secondSelector.ReflectedDPU)
                    : this.Link(firstSelector.ReflectedDSU, secondSelector.ReflectedDVU);
            }
            else
            {
                return secondSelector.ReflectedDPU != null
                    ? this.Link(firstSelector.ReflectedDPU, secondSelector.ReflectedDPU)
                    : this.Link(firstSelector.ReflectedDPU, secondSelector.ReflectedDVU);
            }
        }

        /// <summary>
        /// Attempts to link the given data units.
        /// </summary>
        /// <param name="dSU">The source data unit.</param>
        /// <param name="dPU">The destination data unit.</param>
        /// <returns>The value indicating whether or not the link attempt was successful.</returns>
        public bool Link(ReflectedDataSourceUnit dSU, ReflectedDataProcessingUnit dPU)
        {
            if (!this.DataSourceUnits.Contains(dSU) || !this.DataProcessingUnits.Contains(dPU))
            {
                return false;
            }

            if (this.TryAddEventHandler(dSU.Instance, dSU.ValueGeneratedEvent, dPU.Instance, dPU.ValueInputMethod))
            {
                this.Connections.Add(dSU, dPU);
                this.ConnectionsChanged?.Invoke(this, new ConnectionsChangedEventArgs("Add", this.Connections.Last()));

                return true;
            }

            return false;
        }

        /// <summary>
        /// Attempts to link the given data units.
        /// </summary>
        /// <param name="dSU">The source data unit.</param>
        /// <param name="dVU">The destination data unit.</param>
        /// <returns>The value indicating whether or not the link attempt was successful.</returns>
        public bool Link(ReflectedDataSourceUnit dSU, ReflectedDataVisualisationUnit dVU)
        {
            if (!this.DataSourceUnits.Contains(dSU) || !this.DataVisualisationUnits.Contains(dVU))
            {
                return false;
            }

            if (this.TryAddEventHandler(dSU.Instance, dSU.ValueGeneratedEvent, dVU.Instance, dVU.ValueInputMethod))
            {
                this.Connections.Add(dSU, dVU);
                this.ConnectionsChanged?.Invoke(this, new ConnectionsChangedEventArgs("Add", this.Connections.Last()));

                return true;
            }

            return false;
        }

        /// <summary>
        /// Attempts to link the given data units.
        /// </summary>
        /// <param name="firstDPU">The source data unit.</param>
        /// <param name="secondDPU">The destination data unit.</param>
        /// <returns>The value indicating whether or not the link attempt was successful.</returns>
        public bool Link(ReflectedDataProcessingUnit firstDPU, ReflectedDataProcessingUnit secondDPU)
        {
            if (!this.DataProcessingUnits.Contains(firstDPU) || !this.DataProcessingUnits.Contains(secondDPU))
            {
                return false;
            }

            if (this.TryAddEventHandler(firstDPU.Instance, firstDPU.ValueProcessedEvent, secondDPU.Instance, secondDPU.ValueInputMethod))
            {
                this.Connections.Add(firstDPU, secondDPU);
                this.ConnectionsChanged?.Invoke(this, new ConnectionsChangedEventArgs("Add", this.Connections.Last()));

                return true;
            }

            return false;
        }

        /// <summary>
        /// Attempts to link the given data units.
        /// </summary>
        /// <param name="dPU">The source data unit.</param>
        /// <param name="dVU">The destination data unit.</param>
        /// <returns>The value indicating whether or not the link attempt was successful.</returns>
        public bool Link(ReflectedDataProcessingUnit dPU, ReflectedDataVisualisationUnit dVU)
        {
            if (!this.DataProcessingUnits.Contains(dPU) || !this.DataVisualisationUnits.Contains(dVU))
            {
                return false;
            }

            if (this.TryAddEventHandler(dPU.Instance, dPU.ValueProcessedEvent, dVU.Instance, dVU.ValueInputMethod))
            {
                this.Connections.Add(dPU, dVU);
                this.ConnectionsChanged?.Invoke(this, new ConnectionsChangedEventArgs("Add", this.Connections.Last()));

                return true;
            }

            return false;
        }

        /// <summary>
        /// Determines the actual types of the data units and invokes the corresponding unlink method.
        /// </summary>
        /// <param name="connection">The desired connection between data units to unlink.</param>
        /// <returns>The value indicating whether or not the unlink attempt was successful.</returns>
        public bool Unlink(KeyValuePair<ReflectedDataUnit, ReflectedDataUnit> connection)
        {
            if (this.IsRunning || !this.Connections.Contains(connection))
            {
                return false;
            }

            ReflectedDataUnitSelector firstSelector = new ReflectedDataUnitSelector();
            connection.Key.Accept(firstSelector);
            ReflectedDataUnitSelector secondSelector = new ReflectedDataUnitSelector();
            connection.Value.Accept(secondSelector);

            // Source unit cannot be a DVU and destination unit cannot be a DSU.
            if (firstSelector.ReflectedDVU != null || secondSelector.ReflectedDSU != null)
            {
                return false;
            }

            // Unlink based on combination of actual data units.
            if (firstSelector.ReflectedDSU != null)
            {
                return secondSelector.ReflectedDPU != null
                    ? this.Unlink(firstSelector.ReflectedDSU, secondSelector.ReflectedDPU)
                    : this.Unlink(firstSelector.ReflectedDSU, secondSelector.ReflectedDVU);
            }
            else
            {
                return secondSelector.ReflectedDPU != null
                    ? this.Unlink(firstSelector.ReflectedDPU, secondSelector.ReflectedDPU)
                    : this.Unlink(firstSelector.ReflectedDPU, secondSelector.ReflectedDVU);
            }
        }

        /// <summary>
        /// Attempts to unlink the given connection between data units.
        /// </summary>
        /// <param name="dPU">The source data unit.</param>
        /// <param name="dVU">The destination data unit.</param>
        /// <returns>The value indicating whether or not the unlink attempt was successful.</returns>
        private bool Unlink(ReflectedDataProcessingUnit dPU, ReflectedDataVisualisationUnit dVU)
        {
            if (this.TryRemoveEventHandler(dPU.Instance, dPU.ValueProcessedEvent, dVU.Instance, dVU.ValueInputMethod))
            {
                var connection = this.Connections.FirstOrDefault(x => x.Key == dPU && x.Value == dVU);
                this.Connections.Remove(dPU);
                this.ConnectionsChanged?.Invoke(this, new ConnectionsChangedEventArgs("Remove", connection));

                return true;
            }

            return false;
        }

        /// <summary>
        /// Attempts to unlink the given connection between data units.
        /// </summary>
        /// <param name="firstDPU">The source data unit.</param>
        /// <param name="secondDPU">The destination data unit.</param>
        /// <returns>The value indicating whether or not the unlink attempt was successful.</returns>
        private bool Unlink(ReflectedDataProcessingUnit firstDPU, ReflectedDataProcessingUnit secondDPU)
        {
            if (this.TryRemoveEventHandler(firstDPU.Instance, firstDPU.ValueProcessedEvent, secondDPU.Instance, secondDPU.ValueInputMethod))
            {
                var connection = this.Connections.FirstOrDefault(x => x.Key == firstDPU && x.Value == secondDPU);
                this.Connections.Remove(firstDPU);
                this.ConnectionsChanged?.Invoke(this, new ConnectionsChangedEventArgs("Remove", connection));

                return true;
            }

            return false;
        }

        /// <summary>
        /// Attempts to unlink the given connection between data units.
        /// </summary>
        /// <param name="dSU">The source data unit.</param>
        /// <param name="dVU">The destination data unit.</param>
        /// <returns>The value indicating whether or not the unlink attempt was successful.</returns>
        private bool Unlink(ReflectedDataSourceUnit dSU, ReflectedDataVisualisationUnit dVU)
        {
            if (this.TryRemoveEventHandler(dSU.Instance, dSU.ValueGeneratedEvent, dVU.Instance, dVU.ValueInputMethod))
            {
                var connection = this.Connections.FirstOrDefault(x => x.Key == dSU && x.Value == dVU);
                this.Connections.Remove(dSU);
                this.ConnectionsChanged?.Invoke(this, new ConnectionsChangedEventArgs("Remove", connection));

                return true;
            }

            return false;
        }

        /// <summary>
        /// Attempts to unlink the given connection between data units.
        /// </summary>
        /// <param name="dSU">The source data unit.</param>
        /// <param name="dPU">The destination data unit.</param>
        /// <returns>The value indicating whether or not the unlink attempt was successful.</returns>
        private bool Unlink(ReflectedDataSourceUnit dSU, ReflectedDataProcessingUnit dPU)
        {
            if (this.TryRemoveEventHandler(dSU.Instance, dSU.ValueGeneratedEvent, dPU.Instance, dPU.ValueInputMethod))
            {
                var connection = this.Connections.FirstOrDefault(x => x.Key == dSU && x.Value == dPU);
                this.Connections.Remove(dSU);
                this.ConnectionsChanged?.Invoke(this, new ConnectionsChangedEventArgs("Remove", connection));

                return true;
            }

            return false;
        }

        /// <summary>
        /// Tries to add an event handler to a given event.
        /// </summary>
        /// <param name="eventInstance">The instance where the event resides.</param>
        /// <param name="valueGeneratedEvent">The event, where the event handler gets subscribed.</param>
        /// <param name="handlerInstance">The instance where the handler method resides.</param>
        /// <param name="valueInputMethod">The method that gets subscribed to the event.</param>
        /// <returns>The value indicating whether or not the attempt to add the event handler was successful.</returns>
        private bool TryAddEventHandler(object eventInstance, EventInfo valueGeneratedEvent, object handlerInstance, MethodInfo valueInputMethod)
        {
            try
            {
                Type delegateType = valueGeneratedEvent.EventHandlerType;
                MethodInfo handlerMethodInfo = valueInputMethod;

                Delegate d = Delegate.CreateDelegate(delegateType, handlerInstance, handlerMethodInfo);

                MethodInfo addHandler = valueGeneratedEvent.GetAddMethod();
                object[] addHandlerArgs = { d };
                addHandler.Invoke(eventInstance, addHandlerArgs);

                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

        /// <summary>
        /// Tries to remove an event handler from a given event.
        /// </summary>
        /// <param name="eventInstance">The instance where the event resides.</param>
        /// <param name="valueGeneratedEvent">The event, where the event handler gets unsubscribed.</param>
        /// <param name="handlerInstance">The instance where the handler method resides.</param>
        /// <param name="valueInputMethod">The method that gets unsubscribed from the event.</param>
        /// <returns>The value indicating whether or not the attempt to remove the event handler was successful.</returns>
        private bool TryRemoveEventHandler(object eventInstance, EventInfo valueGeneratedEvent, object handlerInstance, MethodInfo valueInputMethod)
        {
            try
            {
                Type delegateType = valueGeneratedEvent.EventHandlerType;
                MethodInfo handlerMethodInfo = valueInputMethod;

                Delegate d = Delegate.CreateDelegate(delegateType, handlerInstance, handlerMethodInfo);

                MethodInfo removeHandler = valueGeneratedEvent.GetRemoveMethod();
                object[] removeHandlerArgs = { d };
                removeHandler.Invoke(eventInstance, removeHandlerArgs);

                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

        /// <summary>
        /// Loads all data source units that are stored in the DSU folder of the current working directory.
        /// </summary>
        private void LoadDSUs()
        {
            var dataSourceUnitFiles = Directory.EnumerateFiles("DSU", "*.dll", SearchOption.TopDirectoryOnly);
            dataSourceUnitFiles = dataSourceUnitFiles.Concat(Directory.EnumerateFiles("DSU", "*.exe", SearchOption.TopDirectoryOnly)).ToList();
            
            var loadedAssemblies = this.LoadAssemblies(dataSourceUnitFiles);
            var dataSourceUnitTypes = loadedAssemblies.GetDataUnitTypes();

            foreach (var type in dataSourceUnitTypes)
            {
                if (this.LoadedTypes.Contains(type))
                {
                    continue;
                }

                try
                {
                    this.DataSourceUnits.Add(new ReflectedDataSourceUnit(type));
                    this.LoadedTypes.Add(type);
                }
                catch (Exception)
                {
                    // Data Source Unit does not meet the requirements.
                }
            }
        }

        /// <summary>
        /// Loads all data processing units that are stored in the DPU folder of the current working directory.
        /// </summary>
        private void LoadDPUs()
        {
            var dataProcessingUnitFiles = Directory.EnumerateFiles("DPU", "*.dll", SearchOption.TopDirectoryOnly);
            dataProcessingUnitFiles = dataProcessingUnitFiles.Concat(Directory.EnumerateFiles("DPU", "*.exe", SearchOption.TopDirectoryOnly)).ToList();

            var loadedAssemblies = this.LoadAssemblies(dataProcessingUnitFiles);
            var dataProcessingUnitTypes = loadedAssemblies.GetDataUnitTypes();

            foreach (var type in dataProcessingUnitTypes)
            {
                if (this.LoadedTypes.Contains(type))
                {
                    continue;
                }

                try
                {
                    this.DataProcessingUnits.Add(new ReflectedDataProcessingUnit(type));
                    this.LoadedTypes.Add(type);
                }
                catch (Exception)
                {
                    // Data Processing Unit does not meet the requirements.
                }
            }
        }

        /// <summary>
        /// Loads all data visualisation units that are stored in the DVU folder of the current working directory.
        /// </summary>
        private void LoadDVUs()
        {
            var dataVisualisationUnitFiles = Directory.EnumerateFiles("DVU", "*.dll", SearchOption.TopDirectoryOnly);
            dataVisualisationUnitFiles = dataVisualisationUnitFiles.Concat(Directory.EnumerateFiles("DVU", "*.exe", SearchOption.TopDirectoryOnly)).ToList();

            var loadedAssemblies = this.LoadAssemblies(dataVisualisationUnitFiles);
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

        /// <summary>
        /// Loads all assemblies from the specified files.
        /// Assemblies, where an exception occurs, get skipped.
        /// </summary>
        /// <param name="files">The collection of file paths used to load assemblies.</param>
        /// <returns>The collection of loaded assemblies.</returns>
        private IEnumerable<Assembly> LoadAssemblies(IEnumerable<string> files)
        {
            List<Assembly> loadedAssemblies = new List<Assembly>();

            foreach (var file in files)
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
