//-----------------------------------------------------------------------------
// <copyright file="ReflectedDataUnitSelector.cs" company="FH Wiener Neustadt">
//     Copyright (c) FH Wiener Neustadt. All rights reserved.
// </copyright>
// <author>Benjamin Bogner</author>
// <summary>Contains the ReflectedDataUnitSelector class.</summary>
//-----------------------------------------------------------------------------
namespace DataPipeline.Model.ReflectedDataUnits
{
    /// <summary>
    /// Represents the <see cref="ReflectedDataUnitSelector"/> class.
    /// It is used to retrieve the actual instance of a <see cref="ReflectedDataUnit"/>.
    /// </summary>
    public class ReflectedDataUnitSelector : IReflectedDataUnitVisitor
    {
        /// <summary>
        /// Gets the stored <see cref="ReflectedDataSourceUnit"/> of this <see cref="ReflectedDataUnitSelector"/>.
        /// </summary>
        /// <value>The stored <see cref="ReflectedDataSourceUnit"/> of this <see cref="ReflectedDataUnitSelector"/>.</value>
        public ReflectedDataSourceUnit ReflectedDSU { get; private set; }

        /// <summary>
        /// Gets the stored <see cref="ReflectedDataProcessingUnit"/> of this <see cref="ReflectedDataUnitSelector"/>.
        /// </summary>
        /// <value>The stored <see cref="ReflectedDataProcessingUnit"/> of this <see cref="ReflectedDataUnitSelector"/>.</value>
        public ReflectedDataProcessingUnit ReflectedDPU { get; private set; }

        /// <summary>
        /// Gets the stored <see cref="ReflectedDataVisualisationUnit"/> of this <see cref="ReflectedDataUnitSelector"/>.
        /// </summary>
        /// <value>The stored <see cref="ReflectedDataVisualisationUnit"/> of this <see cref="ReflectedDataUnitSelector"/>.</value>
        public ReflectedDataVisualisationUnit ReflectedDVU { get; private set; }

        /// <summary>
        /// Clears all properties of this <see cref="ReflectedDataUnitSelector"/>.
        /// </summary>
        public void Clear()
        {
            this.ReflectedDSU = null;
            this.ReflectedDPU = null;
            this.ReflectedDVU = null;
        }

        /// <summary>
        /// Stores the visited <see cref="ReflectedDataSourceUnit"/> in the internal property.
        /// </summary>
        /// <param name="reflectedDSU">The <see cref="ReflectedDataSourceUnit"/> to be visited.</param>
        public void Visit(ReflectedDataSourceUnit reflectedDSU)
        {
            this.ReflectedDSU = reflectedDSU;
        }

        /// <summary>
        /// Stores the visited <see cref="ReflectedDataProcessingUnit"/> in the internal property.
        /// </summary>
        /// <param name="reflectedDPU">The <see cref="ReflectedDataProcessingUnit"/> to be visited.</param>
        public void Visit(ReflectedDataProcessingUnit reflectedDPU)
        {
            this.ReflectedDPU = reflectedDPU;
        }

        /// <summary>
        /// Stores the visited <see cref="ReflectedDataVisualisationUnit"/> in the internal property.
        /// </summary>
        /// <param name="reflectedDVU">The <see cref="ReflectedDataVisualisationUnit"/> to be visited.</param>
        public void Visit(ReflectedDataVisualisationUnit reflectedDVU)
        {
            this.ReflectedDVU = reflectedDVU;
        }
    }
}
