//---------------------------------------------------------------------------------
// <copyright file="DataListViewVisualisationUnit.xaml.cs" company="FH Wiener Neustadt">
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
    /// Represents the <see cref="DataListViewVisualisationUnit"/> class.
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
        /// <summary>
        /// The <see cref="ViewModel.DataListViewVisualisationUnitVM"/> of this <see cref="DataListViewVisualisationUnit"/>.
        /// </summary>
        private DataListViewVisualisationUnitVM dataListViewVisualisationUnitVM;

        /// <summary>
        /// Initialises a new instance of the <see cref="DataListViewVisualisationUnit"/> class.
        /// </summary>
        public DataListViewVisualisationUnit()
        {
            this.InitializeComponent();
            this.DataListViewVisualisationUnitVM = new DataListViewVisualisationUnitVM();
            this.DataContext = this.DataListViewVisualisationUnitVM;
        }

        /// <summary>
        /// Gets the <see cref="ViewModel.DataListViewVisualisationUnitVM"/> of this <see cref="DataListViewVisualisationUnit"/>.
        /// </summary>
        /// <value>The <see cref="ViewModel.DataListViewVisualisationUnitVM"/> of this <see cref="DataListViewVisualisationUnit"/>.</value>
        public DataListViewVisualisationUnitVM DataListViewVisualisationUnitVM
        {
            get => this.dataListViewVisualisationUnitVM;

            private set
            {
                this.dataListViewVisualisationUnitVM = value ?? throw new ArgumentNullException(nameof(value), "The specified value cannot be null");
            }
        }

        /// <summary>
        /// Gets a value indicating whether this data unit is running.
        /// </summary>
        /// <value>The value indicating whether this data unit is running.</value>
        public bool IsRunning { get; private set; }

        /// <summary>
        /// Inputs a new value into this data unit if it is running.
        /// </summary>
        /// <param name="sender">The sender of the event.</param>
        /// <param name="e">The arguments of the event.</param>
        [DataInput]
        public void InputValue(object sender, ValueOutputEventArgs<int> e)
        {
            if (!this.IsRunning)
            {
                return;
            }

            this.dataListViewVisualisationUnitVM.InputValue(sender, e);
        }

        /// <summary>
        /// Starts this data unit.
        /// </summary>
        public void Start()
        {
            this.IsRunning = true;
        }

        /// <summary>
        /// Stops this data unit.
        /// </summary>
        public void Stop()
        {
            this.IsRunning = false;
        }
    }
}
