//---------------------------------------------------------------------------------
// <copyright file="DataListViewVisualisationUnit.cs" company="FH Wiener Neustadt">
//     Copyright (c) FH Wiener Neustadt. All rights reserved.
// </copyright>
// <author>Benjamin Bogner</author>
// <summary>Contains the DataListViewVisualisationUnit class.</summary>
//---------------------------------------------------------------------------------
namespace DataListViewVisualisationUnit.View
{
    using System;
    using System.Windows.Controls;
    using global::DataListViewVisualisationUnit.ViewModel;
    using DataPipeline.Model.Attributes;
    using DataUnits;

    /// <summary>
    /// Interaction logic for DataListViewVisualisationUnit.xaml
    /// </summary>
    [DataUnitInformation(
        name: "DataListView Visualisation Unit",
        Description = "Takes a value and adds it to the internal ListView.",
        InputDatatype = typeof(int),
        InputDescription = "Any given number.",
        OutputDatatype = typeof(void),
        OutputDescription = "None.")]
    public partial class DataListViewVisualisationUnit : UserControl
    {
        private DataListViewVisualisationUnitVM dataListViewVisualisationUnitVM;

        public DataListViewVisualisationUnit()
        {
            InitializeComponent();
            this.DataListViewVisualisationUnitVM = new DataListViewVisualisationUnitVM();
            this.DataContext = this.DataListViewVisualisationUnitVM;
        }

        public DataListViewVisualisationUnitVM DataListViewVisualisationUnitVM
        {
            get
            {
                return this.dataListViewVisualisationUnitVM;
            }

            private set
            {
                this.dataListViewVisualisationUnitVM = value ?? throw new ArgumentNullException(nameof(value), "The specified value cannot be null");
            }
        }

        public bool IsRunning { get; private set; }

        [DataInput]
        public void InputValue(object sender, ValueOutputEventArgs<int> e)
        {
            if (!this.IsRunning)
            {
                return;
            }

            this.dataListViewVisualisationUnitVM.InputValue(sender, e);
        }

        public void Start()
        {
            this.IsRunning = true;
        }

        public void Stop()
        {
            this.IsRunning = false;
        }
    }
}
