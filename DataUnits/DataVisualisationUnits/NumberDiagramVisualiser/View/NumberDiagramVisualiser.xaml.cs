//--------------------------------------------------------------------------------
// <copyright file="NumberDiagramVisualiser.xaml.cs" company="FH Wiener Neustadt">
//     Copyright (c) FH Wiener Neustadt. All rights reserved.
// </copyright>
// <author>Benjamin Bogner</author>
// <summary>Contains the NumberDiagramVisualiser class.</summary>
//--------------------------------------------------------------------------------
namespace NumberDiagramVisualiser.View
{
    using System;
    using System.Windows.Controls;
    using System.Windows.Data;
    using DataPipeline.Model.Attributes;
    using DataUnits;
    using global::NumberDiagramVisualiser.ViewModel;

    /// <summary>
    /// Represents the <see cref="NumberDiagramVisualiser"/> class.
    /// </summary>
    [DataUnitInformation(
        name: "Number Diagram Visualiser",
        Description = "Takes a value and displays it in a time history diagram. " +
                      "Normalises values to min 0 and max 100 value.",
        InputDatatype = typeof(int),
        InputDescription = "Any given number.",
        OutputDatatype = typeof(void),
        OutputDescription = "None.")]
    public partial class NumberDiagramVisualiser : UserControl
    {
        /// <summary>
        /// The locker object for the value collection.
        /// </summary>
        private readonly object valuesLocker;

        /// <summary>
        /// The <see cref="ViewModel.NumberDiagramVisualiserVM"/> of this <see cref="NumberDiagramVisualiser"/>.
        /// </summary>
        private NumberDiagramVisualiserVM numberDiagramVisualiserVM;

        /// <summary>
        /// Initialises a new instance of the <see cref="NumberDiagramVisualiser"/> class.
        /// </summary>
        public NumberDiagramVisualiser()
        {
            this.InitializeComponent();
            this.valuesLocker = new object();
            this.NumberDiagramVisualiserVM = new NumberDiagramVisualiserVM();

            // It is important to enable collection synchronisation before setting the data context.
            BindingOperations.EnableCollectionSynchronization(this.NumberDiagramVisualiserVM.Values, this.valuesLocker);

            this.DataContext = this.NumberDiagramVisualiserVM;
        }

        /// <summary>
        /// Gets the <see cref="ViewModel.NumberDiagramVisualiserVM"/> of this <see cref="NumberDiagramVisualiser"/>.
        /// </summary>
        /// <value>The <see cref="ViewModel.NumberDiagramVisualiserVM"/> of this <see cref="NumberDiagramVisualiser"/>.</value>
        public NumberDiagramVisualiserVM NumberDiagramVisualiserVM
        {
            get => this.numberDiagramVisualiserVM;

            private set
            {
                this.numberDiagramVisualiserVM = value ?? throw new ArgumentNullException(nameof(value), "The specified value cannot be null");
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

            this.NumberDiagramVisualiserVM.InputValue(sender, e);
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
