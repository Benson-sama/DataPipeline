//-----------------------------------------------------------------------------
// <copyright file="StringListVisualiser.xaml.cs" company="FH Wiener Neustadt">
//     Copyright (c) FH Wiener Neustadt. All rights reserved.
// </copyright>
// <author>Benjamin Bogner</author>
// <summary>Contains the StringListVisualiser class.</summary>
//-----------------------------------------------------------------------------
namespace StringListVisualiser.View
{
    using System;
    using System.Windows.Controls;
    using System.Windows.Data;
    using DataPipeline.Model.Attributes;
    using DataUnits;
    using global::StringListVisualiser.ViewModel;

    /// <summary>
    /// Represents the <see cref="StringListVisualiser"/> class.
    /// </summary>
    [DataUnitInformation(
        name: "String List Visualiser",
        Description = "Takes a value and adds it to the list of strings.",
        InputDatatype = typeof(string),
        InputDescription = "Any given string.",
        OutputDatatype = typeof(void),
        OutputDescription = "None.")]
    public partial class StringListVisualiser : UserControl
    {
        /// <summary>
        /// The locker object for the value collection.
        /// </summary>
        private readonly object collectionLocker;

        /// <summary>
        /// The <see cref="ViewModel.StringListVisualiserVM"/> of this <see cref="StringListVisualiser"/>.
        /// </summary>
        private StringListVisualiserVM stringListVisualiserVM;

        /// <summary>
        /// Initialises a new instance of the <see cref="StringListVisualiser"/> class.
        /// </summary>
        public StringListVisualiser()
        {
            this.InitializeComponent();
            this.collectionLocker = new object();
            this.StringListVisualiserVM = new StringListVisualiserVM();

            // It is important to enable collection synchronisation before setting the data context.
            BindingOperations.EnableCollectionSynchronization(this.StringListVisualiserVM.Values, this.collectionLocker);

            this.DataContext = this.StringListVisualiserVM;
        }

        /// <summary>
        /// Gets the <see cref="ViewModel.StringListVisualiserVM"/> of this <see cref="StringListVisualiser"/>.
        /// </summary>
        /// <value>The <see cref="ViewModel.StringListVisualiserVM"/> of this <see cref="StringListVisualiser"/>.</value>
        public StringListVisualiserVM StringListVisualiserVM
        {
            get => this.stringListVisualiserVM;

            private set
            {
                this.stringListVisualiserVM = value ?? throw new ArgumentNullException(nameof(value), "The specified value cannot be null");
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
        public void InputValue(object sender, ValueOutputEventArgs<string> e)
        {
            if (!this.IsRunning)
            {
                return;
            }

            this.StringListVisualiserVM.InputValue(sender, e);
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
