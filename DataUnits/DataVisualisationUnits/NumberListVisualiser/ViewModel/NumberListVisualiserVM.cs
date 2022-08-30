//--------------------------------------------------------------------------
// <copyright file="NumberListVisualiserVM.cs" company="FH Wiener Neustadt">
//     Copyright (c) FH Wiener Neustadt. All rights reserved.
// </copyright>
// <author>Benjamin Bogner</author>
// <summary>Contains the NumberListVisualiserVM class.</summary>
//--------------------------------------------------------------------------
namespace NumberListVisualiser.ViewModel
{
    using System;
    using System.Collections.ObjectModel;
    using DataUnits;

    /// <summary>
    /// Represents the <see cref="NumberListVisualiserVM"/> class.
    /// </summary>
    public class NumberListVisualiserVM
    {
        /// <summary>
        /// The values of this <see cref="NumberListVisualiserVM"/>.
        /// </summary>
        private ObservableCollection<int> values;

        /// <summary>
        /// Initialises a new instance of the <see cref="NumberListVisualiserVM"/> class.
        /// </summary>
        public NumberListVisualiserVM()
        {
            this.Values = new ObservableCollection<int>();
        }

        /// <summary>
        /// Gets the values of this <see cref="NumberListVisualiserVM"/>.
        /// </summary>
        /// <value>The values of this <see cref="NumberListVisualiserVM"/>.</value>
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
        /// </summary>
        /// <param name="sender">The sender of the event.</param>
        /// <param name="e">The arguments of the event.</param>
        public void InputValue(object sender, ValueOutputEventArgs<int> e)
        {
            this.Values.Add(e.Value);
        }
    }
}
