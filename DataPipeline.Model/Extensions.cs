//--------------------------------------------------------------
// <copyright file="Extensions.cs" company="FH Wiener Neustadt">
//     Copyright (c) FH Wiener Neustadt. All rights reserved.
// </copyright>
// <author>Benjamin Bogner</author>
// <summary>Contains the Extensions class.</summary>
//--------------------------------------------------------------
namespace DataPipeline.Model
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Reflection;
    using DataPipeline.Model.Attributes;

    /// <summary>
    /// Represents the <see cref="Extensions"/> class.
    /// </summary>
    public static class Extensions
    {
        /// <summary>
        /// Gets a collection of types that have the specified attribute type based on a collection of assemblies.
        /// </summary>
        /// <param name="assemblies">The assemblies that get searched for types.</param>
        /// <returns>The desired collection of types as an IEnumerable.</returns>
        public static IEnumerable<Type> GetDataUnitTypes(this IEnumerable<Assembly> assemblies)
        {
            foreach (var assembly in assemblies)
            {
                foreach (var type in assembly.GetDataUnitTypes())
                {
                    yield return type;
                }
            }
        }

        /// <summary>
        /// Gets a collection of types that have the specified attribute type based on one assembly.
        /// </summary>
        /// <param name="assembly">The assembly that gets searched for types.</param>
        /// <returns>The desired collection of types as an IEnumerable.</returns>
        public static IEnumerable<Type> GetDataUnitTypes(this Assembly assembly)
        {
            List<Type> loadedTypes;

            try
            {
                loadedTypes = assembly.GetTypes().ToList();
            }
            catch (ReflectionTypeLoadException e)
            {
                loadedTypes = e.Types.Where(x => x != null).ToList();
            }

            foreach (var type in loadedTypes)
            {
                if (type.GetCustomAttribute<DataUnitInformationAttribute>() != null)
                {
                    yield return type;
                }
            }
        }
    }
}
