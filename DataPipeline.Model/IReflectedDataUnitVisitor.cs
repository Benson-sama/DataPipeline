//-----------------------------------------------------------------------------
// <copyright file="IReflectedDataUnitVisitor.cs" company="FH Wiener Neustadt">
//     Copyright (c) FH Wiener Neustadt. All rights reserved.
// </copyright>
// <author>Benjamin Bogner</author>
// <summary>Contains the IReflectedDataUnitVisitor interface.</summary>
//-----------------------------------------------------------------------------
namespace DataPipeline.Model
{
    public interface IReflectedDataUnitVisitor
    {
        void Visit(ReflectedDataSourceUnit reflectedDataSourceUnit);

        void Visit(ReflectedDataProcessingUnit reflectedDataProcessingUnit);

        void Visit(ReflectedDataVisualisationUnit reflectedDataVisualisationUnit);
    }
}
