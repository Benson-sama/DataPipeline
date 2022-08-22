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
        /// <summary>
        /// Initialises a new instance of the <see cref="ConfigurationApplication"/> class.
        /// </summary>
        public ConfigurationApplication()
        {
        }

        /// <summary>
        /// Loads DSUs and DPUs from their corresponding folders on disk.
        /// </summary>
        public void LoadExtensions()
        {
            this.LoadDSUs();
            this.LoadDPUs();
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
            var dataSourceUnitFiles = Directory.GetFiles("DSU").ToList();
            var dataSourceUnitAssemblies = dataSourceUnitFiles.Select(p => Assembly.LoadFrom(p)).ToList();
            List<Type> dataSourceUnitTypes = new List<Type>();

            foreach (var assembly in dataSourceUnitAssemblies)
            {
                dataSourceUnitTypes = dataSourceUnitTypes.Concat(assembly.GetTypes()).ToList();
            }
        }

        /// <summary>
        /// Loads all data processing units that are stored in the DSU folder in the application directory.
        /// </summary>
        private void LoadDPUs()
        {
            var dataProcessingUnitFiles = Directory.GetFiles("DPU").ToArray();
            var dataProcessingUnitAssemblies = dataProcessingUnitFiles.Select(p => Assembly.LoadFrom(p)).ToList();
            List<Type> dataProcessingUnitTypes = new List<Type>();

            foreach (var assembly in dataProcessingUnitAssemblies)
            {
                dataProcessingUnitTypes = dataProcessingUnitTypes.Concat(assembly.GetTypes()).ToList();
            }
        }
    }
}