//--------------------------------------------------------------------------
// <copyright file="StringListVisualiserVM.cs" company="FH Wiener Neustadt">
//     Copyright (c) FH Wiener Neustadt. All rights reserved.
// </copyright>
// <author>Benjamin Bogner</author>
// <summary>Contains the StringListVisualiserVM class.</summary>
//--------------------------------------------------------------------------
namespace StringListVisualiser.ViewModel
{
    using System;
    using System.Collections.ObjectModel;
    using DataUnits;

    /// <summary>
    /// Represents the <see cref="StringListVisualiserVM"/> class.
    /// </summary>
    public class StringListVisualiserVM
    {
        /// <summary>
        /// The values of this <see cref="StringListVisualiserVM"/>.
        /// </summary>
        private ObservableCollection<string> values;

        /// <summary>
        /// Initialises a new instance of the <see cref="StringListVisualiserVM"/> class.
        /// </summary>
        public StringListVisualiserVM()
        {
            this.Values = new ObservableCollection<string>();
        }

        /// <summary>
        /// Gets the values of this <see cref="StringListVisualiserVM"/>.
        /// </summary>
        /// <value>The values of this <see cref="StringListVisualiserVM"/>.</value>
        public ObservableCollection<string> Values
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
        public void InputValue(object sender, ValueOutputEventArgs<string> e)
        {
            this.Values.Add(e.Value);
        }
    }
}
