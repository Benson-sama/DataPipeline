//-------------------------------------------------------------------
// <copyright file="Mainwindow.xaml.cs" company="FH Wiener Neustadt">
//     Copyright (c) FH Wiener Neustadt. All rights reserved.
// </copyright>
// <author>Benjamin Bogner</author>
// <summary>Contains the MainWindow class.</summary>
//-------------------------------------------------------------------
namespace DataPipeline.View
{
    using System.Windows;
    using System.Windows.Input;
    using System.Windows.Media;
    using System.Windows.Shapes;
    using DataPipeline.ViewModel;

    /// <summary>
    /// Represents the <see cref="MainWindow"/> class.
    /// </summary>
    public partial class MainWindow : Window
    {
        /// <summary>
        /// The <see cref="ConfigurationApplicationVM"/> of the <see cref="MainWindow"/>.
        /// </summary>
        private readonly ConfigurationApplicationVM extensionLinkerVM;

        /// <summary>
        /// Initialises a new instance of the <see cref="MainWindow"/> class.
        /// </summary>
        public MainWindow()
        {
            this.InitializeComponent();
            this.extensionLinkerVM = new ConfigurationApplicationVM();
        }

        /// <summary>
        /// Loads all extensions of the <see cref="ConfigurationApplicationVM"/> class.
        /// </summary>
        /// <param name="sender">The sender of the event.</param>
        /// <param name="e">The <see cref="RoutedEventArgs"/> of the event.</param>
        private void LoadButton_Click(object sender, RoutedEventArgs e)
        {
            this.extensionLinkerVM.LoadExtensions();
        }
    }
}