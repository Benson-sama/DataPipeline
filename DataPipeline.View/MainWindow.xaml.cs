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
    using System.Windows.Input;
    using System.Windows.Media;
    using System.Windows.Shapes;
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
            this.dataVisualisationUnits = this.configAppVM.DataVisualisationUnits.Select(x => x.Instance as UserControl);
            this.dataVisualisationUnitsControl.ItemsSource = dataVisualisationUnits;
        }

        /// <summary>
        /// Loads all extensions of the <see cref="ConfigurationApplicationVM"/> class.
        /// </summary>
        /// <param name="sender">The sender of the event.</param>
        /// <param name="e">The <see cref="RoutedEventArgs"/> of the event.</param>
        private void LoadButton_Click(object sender, RoutedEventArgs e)
        {
            this.configAppVM.LoadExtensions();
        }
    }
}
