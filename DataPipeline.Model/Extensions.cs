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

    /// <summary>
    /// Represents the <see cref="Extensions"/> class.
    /// </summary>
    public static class Extensions
    {
        /// <summary>
        /// Gets a collection of types that implement the specified interface type based on one assembly.
        /// </summary>
        /// <param name="assembly">The assembly that gets searched for types.</param>
        /// <param name="interfaceType">The interface type used for filtering types.</param>
        /// <returns>The desired collection of types as an IEnumerable.</returns>
        public static IEnumerable<Type> GetTypesWithInterface(this Assembly assembly, Type interfaceType)
        {
            foreach (var type in assembly.GetTypes())
            {
                if (type.GetInterfaces().Contains(interfaceType))
                {
                    yield return type;
                }
            }
        }

        /// <summary>
        /// Gets a collection of types that implement the specified interface type based on a list of assemblies.
        /// </summary>
        /// <param name="list">The assemblies that get searched for types.</param>
        /// <param name="interfaceType">The interface type used for filtering types.</param>
        /// <returns>The desired collection of types as an IEnumerable.</returns>
        public static IEnumerable<Type> GetTypesWithInterface(this List<Assembly> list, Type interfaceType)
        {
            foreach (var assembly in list)
            {
                foreach (var type in assembly.GetTypesWithInterface(interfaceType))
                {
                    yield return type;
                }
            }
        }
    }
}
