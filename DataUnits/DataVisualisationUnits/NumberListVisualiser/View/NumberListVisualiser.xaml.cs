//-----------------------------------------------------------------------------
// <copyright file="NumberListVisualiser.xaml.cs" company="FH Wiener Neustadt">
//     Copyright (c) FH Wiener Neustadt. All rights reserved.
// </copyright>
// <author>Benjamin Bogner</author>
// <summary>Contains the NumberListVisualiser class.</summary>
//-----------------------------------------------------------------------------
namespace NumberListVisualiser.View
{
    using System;
    using System.Windows.Controls;
    using System.Windows.Data;
    using DataPipeline.Model.Attributes;
    using DataUnits;
    using global::NumberListVisualiser.ViewModel;

    /// <summary>
    /// Represents the <see cref="NumberListVisualiser"/> class.
    /// </summary>
    [DataUnitInformation(
        name: "Number List Visualiser",
        Description = "Takes a value and adds it to the list of numbers.",
        InputDatatype = typeof(int),
        InputDescription = "Any given number.",
        OutputDatatype = typeof(void),
        OutputDescription = "None.")]
    public partial class NumberListVisualiser : UserControl
    {
        /// <summary>
        /// The locker object for the value collection.
        /// </summary>
        private readonly object collectionLocker;

        /// <summary>
        /// The <see cref="ViewModel.NumberListVisualiserVM"/> of this <see cref="NumberListVisualiser"/>.
        /// </summary>
        private NumberListVisualiserVM numberListVisualiserVM;

        /// <summary>
        /// Initialises a new instance of the <see cref="NumberListVisualiser"/> class.
        /// </summary>
        public NumberListVisualiser()
        {
            this.InitializeComponent();
            this.collectionLocker = new object();
            this.NumberListVisualiserVM = new NumberListVisualiserVM();

            // It is important to enable collection synchronisation before setting the data context.
            BindingOperations.EnableCollectionSynchronization(this.NumberListVisualiserVM.Values, this.collectionLocker);

            this.DataContext = this.NumberListVisualiserVM;
        }

        /// <summary>
        /// Gets the <see cref="ViewModel.NumberListVisualiserVM"/> of this <see cref="NumberListVisualiser"/>.
        /// </summary>
        /// <value>The <see cref="ViewModel.NumberListVisualiserVM"/> of this <see cref="NumberListVisualiser"/>.</value>
        public NumberListVisualiserVM NumberListVisualiserVM
        {
            get => this.numberListVisualiserVM;

            private set
            {
                this.numberListVisualiserVM = value ?? throw new ArgumentNullException(nameof(value), "The specified value cannot be null");
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

            this.NumberListVisualiserVM.InputValue(sender, e);
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

        /// <summary>
        /// Scrolls to the end of the <see cref="ScrollViewer"/> if the extent height has changed.
        /// </summary>
        /// <param name="sender">The sender of the event.</param>
        /// <param name="e">The arguments of the event.</param>
        private void ScrollViewer_ScrollChanged(object sender, ScrollChangedEventArgs e)
        {
            if (e.ExtentHeightChange <= 0)
            {
                return;
            }

            ScrollViewer sw = sender as ScrollViewer;
            sw.ScrollToEnd();
        }
    }
}
