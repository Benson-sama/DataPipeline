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

        /// <summary>
        /// Loads all extensions of the <see cref="ConfigurationApplication"/>.
        /// </summary>
        public void LoadExtensions()
        {
            this.configApp.LoadExtensions();
            this.LoadDVUs();
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
