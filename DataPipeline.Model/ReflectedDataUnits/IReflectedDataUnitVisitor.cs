//-----------------------------------------------------------------------------
// <copyright file="IReflectedDataUnitVisitor.cs" company="FH Wiener Neustadt">
//     Copyright (c) FH Wiener Neustadt. All rights reserved.
// </copyright>
// <author>Benjamin Bogner</author>
// <summary>Contains the IReflectedDataUnitVisitor interface.</summary>
//-----------------------------------------------------------------------------
namespace DataPipeline.Model.ReflectedDataUnits
{
    /// <summary>
    /// Represents the <see cref="IReflectedDataUnitVisitor"/> interface.
    /// </summary>
    public interface IReflectedDataUnitVisitor
    {
        /// <summary>
        /// Instructs the <see cref="IReflectedDataUnitVisitor"/> to visit a <see cref="ReflectedDataSourceUnit"/>.
        /// </summary>
        /// <param name="reflectedDataSourceUnit">The accepting <see cref="ReflectedDataSourceUnit"/>.</param>
        void Visit(ReflectedDataSourceUnit reflectedDataSourceUnit);

        /// <summary>
        /// Instructs the <see cref="IReflectedDataUnitVisitor"/> to visit a <see cref="ReflectedDataProcessingUnit"/>.
        /// </summary>
        /// <param name="reflectedDataProcessingUnit">The accepting <see cref="ReflectedDataProcessingUnit"/>.</param>
        void Visit(ReflectedDataProcessingUnit reflectedDataProcessingUnit);

        /// <summary>
        /// Instructs the <see cref="IReflectedDataUnitVisitor"/> to visit a <see cref="ReflectedDataVisualisationUnit"/>.
        /// </summary>
        /// <param name="reflectedDataVisualisationUnit">The accepting <see cref="ReflectedDataVisualisationUnit"/>.</param>
        void Visit(ReflectedDataVisualisationUnit reflectedDataVisualisationUnit);
    }
}
