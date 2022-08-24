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

    public class DataListViewVisualisationUnitVM
    {
        private ObservableCollection<int> values;

        public DataListViewVisualisationUnitVM()
        {
            this.Values = new ObservableCollection<int>();
        }

        public ObservableCollection<int> Values
        {
            get
            {
                return this.values;
            }

            private set
            {
                this.values = value ?? throw new ArgumentNullException(nameof(value), "The specified value cannot be null");
            }
        }

        public void InputValue(object sender, ValueOutputEventArgs<int> e)
        {
            this.Values.Add(e.Value);
        }
    }
}
