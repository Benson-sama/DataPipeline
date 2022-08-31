//-------------------------------------------------------------------
// <copyright file="Mainwindow.xaml.cs" company="FH Wiener Neustadt">
//     Copyright (c) FH Wiener Neustadt. All rights reserved.
// </copyright>
// <author>Benjamin Bogner</author>
// <summary>Contains the MainWindow class.</summary>
//-------------------------------------------------------------------
namespace DataPipeline.View
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Windows;
    using System.Windows.Controls;
    using DataPipeline.Model.ReflectedDataUnits;
    using DataPipeline.ViewModel;

    /// <summary>
    /// Represents the <see cref="MainWindow"/> class.
    /// </summary>
    public partial class MainWindow : Window
    {
        /// <summary>
        /// The <see cref="ConfigurationApplicationVM"/> of the <see cref="MainWindow"/>.
        /// </summary>
        private readonly ConfigurationApplicationVM configAppVM;

        /// <summary>
        /// The data visualisation units of the <see cref="MainWindow"/>.
        /// </summary>
        private IEnumerable<UserControl> dataVisualisationUnits;

        /// <summary>
        /// Initialises a new instance of the <see cref="MainWindow"/> class.
        /// </summary>
        public MainWindow()
        {
            this.InitializeComponent();
            this.configAppVM = new ConfigurationApplicationVM();
            this.DataContext = this.configAppVM;
            this.configAppVM.LoadExtensions();
            this.dataUnitsListView.ItemsSource = this.configAppVM.DataUnits;
            this.sourceDataUnitComboBox.ItemsSource = this.configAppVM.SourceDataUnits;
            this.destinationDataUnitComboBox.ItemsSource = this.configAppVM.DestinationDataUnits;
            this.dataVisualisationUnits = this.configAppVM.DataVisualisationUnits.Select(x => x.Instance as UserControl).Where(x => x != null);
            this.dataVisualisationUnitsControl.ItemsSource = this.dataVisualisationUnits;
            this.connectionsListView.DataContext = this.configAppVM.Connections;
        }

        /// <summary>
        /// Shows the information of the selected <see cref="ReflectedDataUnit"/> in a <see cref="MessageBox"/>.
        /// </summary>
        /// <param name="sender">The sender of the event.</param>
        /// <param name="e">The <see cref="RoutedEventArgs"/> of the event.</param>
        private void InfoButton_Click(object sender, RoutedEventArgs e)
        {
            var button = e.Source as Button;
            var dataUnit = button.DataContext as ReflectedDataUnit;

            if (dataUnit == null)
            {
                return;
            }

            MessageBox.Show(
                $"Description: {dataUnit.Attribute.Description}\n" +
                $"Input datatype: {dataUnit.Attribute.InputDatatype}\n" +
                $"Input description: {dataUnit.Attribute.InputDescription}\n" +
                $"Output datatype: {dataUnit.Attribute.OutputDatatype}\n" +
                $"Output description: {dataUnit.Attribute.OutputDescription}\n",
                $"{dataUnit.Attribute.Name}");
        }

        /// <summary>
        /// Loads all extensions of the <see cref="ConfigurationApplicationVM"/> class.
        /// </summary>
        /// <param name="sender">The sender of the event.</param>
        /// <param name="e">The <see cref="RoutedEventArgs"/> of the event.</param>
        private void LoadButton_Click(object sender, RoutedEventArgs e)
        {
            if (this.configAppVM.IsRunning)
            {
                MessageBox.Show(
                    "Unable to load extensions when the Data Pipeline is activated. " +
                    "Stop it and try again.",
                    "Error");

                return;
            }

            this.configAppVM.LoadExtensions();
        }

        /// <summary>
        /// Toggles the state of the <see cref="ConfigurationApplicationVM"/>.
        /// </summary>
        /// <param name="sender">The sender of the event.</param>
        /// <param name="e">The arguments of the event.</param>
        private void StateButton_Click(object sender, RoutedEventArgs e)
        {
            if (!this.configAppVM.IsRunning)
            {
                this.configAppVM.Start();
                Button button = sender as Button;
                button.Content = "Stop";
            }
            else if (this.configAppVM.IsRunning)
            {
                this.configAppVM.Stop();
                Button button = sender as Button;
                button.Content = "Start";
            }
        }

        /// <summary>
        /// Ensures that the <see cref="ConfigurationApplicationVM"/> is stopped when closing the <see cref="MainWindow"/>.
        /// </summary>
        /// <param name="sender">The sender of the event.</param>
        /// <param name="e">The arguments of the event.</param>
        private void Window_Closed(object sender, EventArgs e)
        {
            this.configAppVM.Stop();
        }

        /// <summary>
        /// Links the selected data units from the source and destination combo boxes.
        /// This can only be done if the data pipeline is not running.
        /// An error is shown in a <see cref="MessageBox"/> if one of the data units is not selected.
        /// </summary>
        /// <param name="sender">The sender of the event.</param>
        /// <param name="e">The arguments of the event.</param>
        private void LinkButton_Click(object sender, RoutedEventArgs e)
        {
            if (this.configAppVM.IsRunning)
            {
                MessageBox.Show(
                    "Unable to link data units when the Data Pipeline is activated. " +
                    "Stop it and try again.",
                    "Error");

                return;
            }

            ReflectedDataUnit firstDU = this.sourceDataUnitComboBox.SelectedItem as ReflectedDataUnit;
            ReflectedDataUnit secondDU = this.destinationDataUnitComboBox.SelectedItem as ReflectedDataUnit;

            if (firstDU == null || secondDU == null)
            {
                MessageBox.Show("Both source and destination data units need to be selected.", "Error");
                return;
            }

            if (this.configAppVM.Link(firstDU, secondDU))
            {
                MessageBox.Show($"Successfully linked: {firstDU} + {secondDU}");
            }
            else
            {
                MessageBox.Show(
                       $"Unable to link: {firstDU} + {secondDU}\n" +
                       $"The source unit may already be connected or datatypes are not compatible.");
            }
        }

        /// <summary>
        /// Unlinks the selected connection between data units.
        /// This can only be done if the data pipeline is not running.
        /// </summary>
        /// <param name="sender">The sender of the event.</param>
        /// <param name="e">The arguments of the event.</param>
        private void UnlinkButton_Click(object sender, RoutedEventArgs e)
        {
            if (this.configAppVM.IsRunning)
            {
                MessageBox.Show(
                    "Unable to unlink data units when the Data Pipeline is activated. " +
                    "Stop it and try again.",
                    "Error");

                return;
            }

            var button = e.Source as Button;

            if (!(button.DataContext is KeyValuePair<ReflectedDataUnit, ReflectedDataUnit>))
            {
                return;
            }

            var connection = (KeyValuePair<ReflectedDataUnit, ReflectedDataUnit>)button.DataContext;

            if (this.configAppVM.Unlink(connection))
            {
                MessageBox.Show($"Successfully unlinked: {connection.Key} -> {connection.Value}");
            }
            else
            {
                MessageBox.Show($"Unable to unlink: {connection.Key} -> {connection.Value}\n");
            }
        }
    }
}
