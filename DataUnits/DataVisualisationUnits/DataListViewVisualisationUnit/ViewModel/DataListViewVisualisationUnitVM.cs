//-----------------------------------------------------------------------------------
// <copyright file="DataListViewVisualisationUnitVM.cs" company="FH Wiener Neustadt">
//     Copyright (c) FH Wiener Neustadt. All rights reserved.
// </copyright>
// <author>Benjamin Bogner</author>
// <summary>Contains the DataListViewVisualisationUnitVM class.</summary>
//-----------------------------------------------------------------------------------
namespace DataListViewVisualisationUnit.ViewModel
{
    using System;
    using System.Collections.ObjectModel;
    using DataUnits;

    /// <summary>
    /// Represents the <see cref="DataListViewVisualisationUnitVM"/> class.
    /// </summary>
    public class DataListViewVisualisationUnitVM
    {
        /// <summary>
        /// The values of this <see cref="DataListViewVisualisationUnitVM"/>.
        /// </summary>
        private ObservableCollection<int> values;

        /// <summary>
        /// Initialises a new instance of the <see cref="DataListViewVisualisationUnitVM"/> class.
        /// </summary>
        public DataListViewVisualisationUnitVM()
        {
            this.Values = new ObservableCollection<int>();
        }

        /// <summary>
        /// Gets the values of this <see cref="DataListViewVisualisationUnitVM"/>.
        /// </summary>
        /// <value>The values of this <see cref="DataListViewVisualisationUnitVM"/>.</value>
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
