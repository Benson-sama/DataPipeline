//-----------------------------------------------------------------------------
// <copyright file="ReflectedDataUnitSelector.cs" company="FH Wiener Neustadt">
//     Copyright (c) FH Wiener Neustadt. All rights reserved.
// </copyright>
// <author>Benjamin Bogner</author>
// <summary>Contains the ReflectedDataUnitSelector class.</summary>
//-----------------------------------------------------------------------------
namespace DataPipeline.Model.ReflectedDataUnits
{
    public class ReflectedDataUnitSelector : IReflectedDataUnitVisitor
    {
        public ReflectedDataSourceUnit ReflectedDSU { get; private set; }

        public ReflectedDataProcessingUnit ReflectedDPU { get; private set; }

        public ReflectedDataVisualisationUnit ReflectedDVU { get; private set; }

        public void Clear()
        {
            this.ReflectedDSU = null;
            this.ReflectedDPU = null;
            this.ReflectedDVU = null;
        }

        public void Visit(ReflectedDataSourceUnit reflectedDSU)
        {
            this.ReflectedDSU = reflectedDSU;
        }

        public void Visit(ReflectedDataProcessingUnit reflectedDPU)
        {
            this.ReflectedDPU = reflectedDPU;
        }

        public void Visit(ReflectedDataVisualisationUnit reflectedDVU)
        {
            this.ReflectedDVU = reflectedDVU;
        }
    }
}
