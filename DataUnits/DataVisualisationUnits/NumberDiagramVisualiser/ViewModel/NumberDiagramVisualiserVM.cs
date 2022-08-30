//-----------------------------------------------------------------------------
// <copyright file="NumberDiagramVisualiserVM.cs" company="FH Wiener Neustadt">
//     Copyright (c) FH Wiener Neustadt. All rights reserved.
// </copyright>
// <author>Benjamin Bogner</author>
// <summary>Contains the NumberDiagramVisualiserVM class.</summary>
//-----------------------------------------------------------------------------
namespace NumberDiagramVisualiser.ViewModel
{
    using System;
    using System.Collections.ObjectModel;
    using System.ComponentModel;
    using DataUnits;

    /// <summary>
    /// Represents the <see cref="NumberDiagramVisualiserVM"/> class.
    /// </summary>
    public class NumberDiagramVisualiserVM : INotifyPropertyChanged
    {
        /// <summary>
        /// The values of this <see cref="NumberDiagramVisualiserVM"/>.
        /// </summary>
        private ObservableCollection<int> values;

        /// <summary>
        /// Initialises a new instance of the <see cref="NumberDiagramVisualiserVM"/> class.
        /// </summary>
        public NumberDiagramVisualiserVM()
        {
            this.Values = new ObservableCollection<int>();
        }

        /// <summary>
        /// The event that gets fired when a property changed.
        /// </summary>
        public event PropertyChangedEventHandler PropertyChanged;

        /// <summary>
        /// Gets the values of this <see cref="NumberDiagramVisualiserVM"/>.
        /// </summary>
        /// <value>The values of this <see cref="NumberDiagramVisualiserVM"/>.</value>
        public ObservableCollection<int> Values
        {
            get => this.values;

            private set
            {
                this.values = value ?? throw new ArgumentNullException(nameof(value), "The specified value cannot be null");
            }
        }

        /// <summary>
        /// Adds a value to the internal values collection.
        /// This method stores values ranging from 0 to 100,
        /// normalising everything outside the range to its min or max value.
        /// </summary>
        /// <param name="sender">The sender of the event.</param>
        /// <param name="e">The arguments of the event.</param>
        public void InputValue(object sender, ValueOutputEventArgs<int> e)
        {
            int value = e.Value > 100 ? 100 : e.Value;
            value = value < 0 ? 0 : value;

            this.Values.Add(value);
            this.PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(this.Values)));
        }
    }
}
