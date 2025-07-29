using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Shapes;
using LIS_Simple_Graph_Plotter.Services;
using LIS_Simple_Graph_Plotter.Model;
using LIS_Simple_Graph_Plotter.ViewModel;
using OxyPlot;
using OxyPlot.Series;
using Microsoft.Win32;
using System.Windows.Controls;
using System.IO;
using System.Windows.Input;

namespace LIS_Simple_Graph_Plotter
{
    /// <summary>
    /// Contains the interaction logic for the main window of the application
    /// Includes initialization, loading and user interactions for the main program
    /// </summary>
    public partial class MainWindow : Window
    {
        /// <summary>
        /// Main view model managing plot and interaction logic
        /// </summary>
        public PlotViewModel ViewModel { get; set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="MainWindow"/> class.
        /// </summary>
        public MainWindow()
        {
            InitializeComponent();
        }

        /// <summary>
        /// Handles the <c>Loaded</c> event of the main window
        /// Initializes the view model and loads recent plot settings
        /// </summary>
        /// <param name="sender">In: Mainwindow as sender</param>
        /// <param name="e">In: The event data associated with the <c>Loaded</c> event</param>
        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            ViewModel = new PlotViewModel();
            this.DataContext = ViewModel;
        }

        /// <summary>
        /// Save graph and all axis information to vector graphic
        /// </summary>
        /// <param name="sender">In: SavePlotToSVGButton as sender</param>
        /// <param name="e">In: Clicked event arguments</param>
        private void SavePlotToSVGButton_Click(object sender, RoutedEventArgs e)
        {
            SavePlotToSVG();
        }

        /// <summary>
        /// Save graph and all axis information to vector graphic via context menu
        /// </summary>
        /// <param name="sender">In: SaveSVGMenuItem as sender</param>
        /// <param name="e">In: Clicked event arguments</param>
        private void SaveSVGMenuItem_Click(object sender, RoutedEventArgs e)
        {
            SavePlotToSVG();
        }

        /// <summary>
        /// Open a safe file dialog and export the plot to an svg to store it in the selected location
        /// </summary>
        private void SavePlotToSVG()
        {
            var dialog = new SaveFileDialog
            {
                Title = "Save current plot to file",
                Filter = "Scalable Vector Graphics (*.svg)|*.svg",
                DefaultExt = "svg",
                AddExtension = true
            };

            if (dialog.ShowDialog() == true)
            {
                string filePath = dialog.FileName;

                try
                {
                    ViewModel.SavePlotToSVG(filePath);
                    MessageBox.Show($"Plot saved to " + filePath, "Image saved", MessageBoxButton.OK, MessageBoxImage.Information);
                }
                catch (UnauthorizedAccessException)
                {
                    MessageBox.Show("You do not have permission to write to the selected location.", "Access Denied", MessageBoxButton.OK, MessageBoxImage.Error);
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"An unexpected error occurred, check if there is enough space on your hard drive: " + ex.Message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
        }

        /// <summary>
        /// Save configuration of selected graph to file
        /// </summary>
        /// <param name="sender">In: SavePlotConfigurationButton as sender</param>
        /// <param name="e">In: Clicked event arguments</param>
        private void SavePlotConfigurationButton_Click(object sender, RoutedEventArgs e)
        {
            ViewModel.SaveConfiguration();
        }
    }
}
