//-------------------------------------------------------------------
// <copyright file="Mainwindow.xaml.cs" company="FH Wiener Neustadt">
//     Copyright (c) FH Wiener Neustadt. All rights reserved.
// </copyright>
// <author>Benjamin Bogner</author>
// <summary>Contains the MainWindow class.</summary>
//-------------------------------------------------------------------
namespace DataPipeline.View
{
    using System.Collections.Generic;
    using System.Linq;
    using System.Windows;
    using System.Windows.Controls;
    using DataPipeline.Model;
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
            this.dataVisualisationUnits = this.configAppVM.DataVisualisationUnits.Select(x => x.Instance as UserControl);
            this.dataVisualisationUnitsControl.ItemsSource = this.dataVisualisationUnits;
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
                MessageBox.Show("Unable to load extensions when the Data Pipeline is activated. " +
                                "Stop it and try again.", "Error");
                return;
            }

            this.configAppVM.LoadExtensions();
        }

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

        private void Window_Closed(object sender, System.EventArgs e)
        {
            this.configAppVM.Stop();
        }

        private void LinkButton_Click(object sender, RoutedEventArgs e)
        {
            ReflectedDataUnit firstUnit = this.sourceDataUnitComboBox.SelectedItem as ReflectedDataUnit;
            ReflectedDataUnit secondUnit = this.destinationDataUnitComboBox.SelectedItem as ReflectedDataUnit;

            if (firstUnit == null || secondUnit == null)
            {
                MessageBox.Show("Both source and destination data units need to be selected.", "Error");
                return;
            }

            ReflectedDataUnitSelector firstSelector = new ReflectedDataUnitSelector();
            firstUnit.Accept(firstSelector);
            ReflectedDataUnitSelector secondSelector = new ReflectedDataUnitSelector();
            secondUnit.Accept(secondSelector);

            if (firstSelector.ReflectedDVU != null || secondSelector.ReflectedDSU != null)
            {
                return;
            }

            if (firstSelector.ReflectedDSU != null)
            {
                if (secondSelector.ReflectedDPU != null)
                {
                    this.configAppVM.Link(firstSelector.ReflectedDSU, secondSelector.ReflectedDPU);
                }
                else
                {
                    this.configAppVM.Link(firstSelector.ReflectedDSU, secondSelector.ReflectedDVU);
                }
            }
            else
            {
                if (secondSelector.ReflectedDPU != null)
                {
                    this.configAppVM.Link(firstSelector.ReflectedDPU, secondSelector.ReflectedDPU);
                }
                else
                {
                    this.configAppVM.Link(firstSelector.ReflectedDPU, secondSelector.ReflectedDVU);
                }
            }

            MessageBox.Show($"Linked: {firstUnit} + {secondUnit}");
        }

        private void InfoButton_Click(object sender, RoutedEventArgs e)
        {
            var button = e.Source as Button;
            var dataUnit = button.DataContext as ReflectedDataUnit;
            
            if (dataUnit == null)
            {
                return;
            }

            MessageBox.Show($"Description: {dataUnit.Attribute.Description}\n" +
                            $"Input datatype: {dataUnit.Attribute.InputDatatype}\n" +
                            $"Input description: {dataUnit.Attribute.InputDescription}\n" +
                            $"Output datatype: {dataUnit.Attribute.OutputDatatype}\n" +
                            $"Output description: {dataUnit.Attribute.OutputDescription}\n",
                            $"{dataUnit.Attribute.Name}");
        }
    }
}
