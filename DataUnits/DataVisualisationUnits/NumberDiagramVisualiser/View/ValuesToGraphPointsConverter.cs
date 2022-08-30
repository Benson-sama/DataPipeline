//-------------------------------------------------------------------------------------
// <copyright file="ValuesToGraphPointsConverter.cs" company="FH Wiener Neustadt">
//     Copyright (c) FH Wiener Neustadt. All rights reserved.
// </copyright>
// <author>Benjamin Bogner</author>
// <summary>Contains the ValuesToGraphPointsConverter class.</summary>
//-------------------------------------------------------------------------------------
namespace NumberDiagramVisualiser.View
{
    using System;
    using System.Collections.ObjectModel;
    using System.Globalization;
    using System.Linq;
    using System.Windows;
    using System.Windows.Data;
    using System.Windows.Media;

    /// <summary>
    /// Represents the <see cref="ValuesToGraphPointsConverter"/> class.
    /// </summary>
    public class ValuesToGraphPointsConverter : IValueConverter
    {
        /// <summary>
        /// Converts the <see cref="ObservableCollection{T}"/> to a <see cref="PointCollection"/>.
        /// </summary>
        /// <param name="value">The specified <see cref="ObservableCollection{T}"/>.</param>
        /// <param name="targetType">The target type, which is unused.</param>
        /// <param name="parameter">The parameter, which is unused.</param>
        /// <param name="culture">The culture, which is unused.</param>
        /// <returns>The converted <see cref="PointCollection"/>.</returns>
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var values = (ObservableCollection<int>)value;
            PointCollection points = new PointCollection();

            int i = 5;
            foreach (var p in values.Reverse().Take(71).Reverse())
            {
                points.Add(new Point(i, 100 - p));
                i += 2;
            }

            return points;
        }

        /// <summary>
        /// This method is not implemented.
        /// </summary>
        /// <param name="value">The value, which is unused.</param>
        /// <param name="targetType">The target type, which is unused.</param>
        /// <param name="parameter">The parameter, which is unused.</param>
        /// <param name="culture">The culture, which is unused.</param>
        /// <returns>Returns nothing.</returns>
        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
